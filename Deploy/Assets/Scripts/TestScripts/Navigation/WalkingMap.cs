using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WalkingMap
{
    List<Vector3> pts = new List<Vector3>();

    NodeState[] nodeStates;
    double[] gVal;
    double[] hVal;
    double[] fVal;
    double[] cost;
    int[] parent;
    int[] index;

    int numMillisecondsToPathfind = 5;

    List<int> nodesToClear = new List<int>();

    List<int>[] map;

    public WalkingMap(List<Vector3> points, List<int>[] map, int planetRad)
    {
        TempNode.wrapNum = (int)Math.Pow(10, (planetRad + "").Length);

        List<TempNode> sortedNodes = new List<TempNode>();
        for(int i = 0; i < points.Count; i++)
        {
            sortedNodes.Add(new TempNode(points[i], i));
        }
        sortedNodes.Sort();
        int[] newNodeMapping = new int[sortedNodes.Count];
        for(int i = 0; i < sortedNodes.Count; i++)
        {
            newNodeMapping[sortedNodes[i].originalIndex] = i;
        }

        foreach(TempNode n in sortedNodes)
        {
            pts.Add(n.position);
        }

        this.map = new List<int>[map.Length];
        LayerMask mask = ~LayerMask.GetMask(new string[] { "planet" });
        for (int i = 0; i < sortedNodes.Count; i++)
        {
            List<int> connections = map[sortedNodes[i].originalIndex];
            for(int i1 = 0; i1 < connections.Count; i1++)
            {
                connections[i1] = newNodeMapping[connections[i1]];
            }
            List<int> unblockedConnections = new List<int>();            
            foreach (int con in connections)
            {
                ObjectHandler.PullCachedObjectsTo(new int[] { sortedNodes[i].originalIndex, sortedNodes[con].originalIndex });
                if(!Physics.Linecast(pts[i], pts[con], mask) && !Physics.Linecast(pts[con], pts[i], mask))
                {
                    unblockedConnections.Add(con);
                    Debug.DrawLine(pts[i], pts[con], Color.green, 1000000.0f);
                }
            }
            this.map[i] = unblockedConnections;
        }
        gVal = new double[pts.Count];
        hVal = new double[pts.Count];
        cost = new double[pts.Count];
        parent = new int[pts.Count];
        index = new int[pts.Count];
        for(int i = 0; i < parent.Length; i++)
        {
            parent[i] = -1;
            index[i] = -1;
        }
        
    }

    #region taskManagement

    PriorityTree savedNodeQueue = null;
    List<PathTask> tasks = new List<PathTask>();
    public void AddTask(PathTask pt)
    {
        tasks.Add(pt);
    }

    public PathState TaskCompleter()
    {
        //get the start time
        DateTime timeToAbort = DateTime.Now.AddMilliseconds(numMillisecondsToPathfind);


        //set the pathState to null
        PathState pathState = PathState.NULL;


        int numIter = 0;
        //as long as we haven't exceeded time, and we have tasks to do, keep pathfinding
        while (pathState != PathState.INCOMPLETE && tasks.Count > 0)
        {
            numIter++;
            if (tasks[0] == null)
            {
                tasks.Remove(tasks[0]);
                pathState = PathState.COMPLETE;
                continue;
            }
            
            //holds the nodeQueue
            PriorityTree nodeQueue;
            float pathLength;
            //get the path (in terms of nodes)
            List<int> nodePath = GetPath(GetClosestNode(tasks[0].start), GetClosestNode(tasks[0].finish), savedNodeQueue, timeToAbort, out nodeQueue, out pathState, out pathLength);

            List<Vector3> ptsPath = null;
            if (nodePath != null)
            {
                ptsPath = new List<Vector3>();
                foreach (int node in nodePath)
                {
                    ptsPath.Add(pts[node]);
                }
            }

            //if we haven't run out of time
            if (pathState != PathState.INCOMPLETE)
            {
                //finish the pathTask
                tasks[0].finishTask(nodePath, ptsPath, pathLength);
                //remove the task from the list
                tasks.RemoveAt(0);
                //don't save this node queue
                savedNodeQueue = null;
                //clear the map
                ClearNodes();
            }
            //if the state is incomplete
            else
            {
                //save the node queue as well as the state of the map for the next pathfinding.
                savedNodeQueue = nodeQueue;
            }
        }

        return pathState;
    }
    #endregion

    #region pathfinding
    //stops everything and gets a path. If it cannot find a path in the milliseconds given, it returns null.
    public List<Vector3> GetPathBlockingCall(Vector3 start, Vector3 finish, int maxMillisecondsToBlock)
    {
        PriorityTree newNodeQueue = null; PathState pathState = PathState.NULL; float pathLength = 0;
        List<int> path = GetPath(GetClosestNode(start), GetClosestNode(finish), null, DateTime.Now.AddMilliseconds(maxMillisecondsToBlock), out newNodeQueue, out pathState, out pathLength);
        List<Vector3> ptsPath = null;
        if(path != null)
        {
            ptsPath = new List<Vector3>();
            foreach(int node in path)
            {
                ptsPath.Add(pts[node]);
            }
        }
        return ptsPath;
    }

    //does 'work' towards finding a given path. If it can't currently find the path, it outputs the work it has done and returns null.
    private List<int> GetPath(int start, int finish, PriorityTree savedNodeQueue, DateTime timeToAbort, out PriorityTree newNodeQueue, out PathState pathState, out float pathLength)
    {
        pathState = Search(start, finish, savedNodeQueue, timeToAbort, out newNodeQueue);
        pathLength = 0;
        if (pathState == PathState.COMPLETE)
        {
            List<int> path = new List<int>();
            int index = finish;            
            while(index != start)
            {
                pathLength += Vector3.Distance(pts[index], pts[parent[index]]);
                path.Add(index);
                index = parent[index];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }
        pathLength = -1;
        return null;
    }

    private PathState Search(int start, int finish, PriorityTree savedNodeQueue, DateTime timeToAbort, out PriorityTree nodeQueue)
    {
        DateTime startTime = DateTime.Now;
        int numNodesSearched = 0;
        //if we don't have any previous work to go off of...
        if (savedNodeQueue == null)
        {
            //make a new heap to store the nodes we're looking at. Give it a starting size of 1.
            nodeQueue = new PriorityTree(1, fVal, gVal, index);
            
            //calculate g,h, and f values for the start node
            gVal[start] = 0;
            hVal[start] = Heuristic(pts[start], pts[finish]);
            fVal[start] = gVal[start] + hVal[start];

            //set the state of the start node to open
            nodeStates[start] = NodeState.OPEN;
            //add the start node to the heap
            nodeQueue.Add(start);
            nodesToClear = new List<int>();
            nodesToClear.Add(start);
        }
        else
        {
            //otherwise, go off of what we have
            nodeQueue = savedNodeQueue;
        }

        //as long as we still have nodes to examine
        while (!nodeQueue.isEmpty)
        {
            //if we're over our allotted time           
            if ((DateTime.Now - timeToAbort).Ticks > 0)
            {
                //abort
                return PathState.INCOMPLETE;
            }

            numNodesSearched++;
            //get the "most promising" node from the top of the heap. (nodes are sorted by f value)
            int currentNode = nodeQueue.Remove();

            //if this node is the finish
            if (currentNode == finish)
            {
                //we're done, return.
                return PathState.COMPLETE;
            }

            //set the state of this node to closed
            nodeStates[currentNode] = NodeState.CLOSED;
            
            foreach (int node in map[currentNode])
            {
                Vector3 newDir = pts[node] - pts[currentNode];
                if (nodeStates[node] == NodeState.UNTESTED)
                {
                    parent[node] = currentNode;
                    nodeStates[node] = NodeState.OPEN;
                    gVal[node] = gVal[currentNode] + Vector3.Distance(pts[node], pts[currentNode]) * cost[node];
                    //distance-based heuristic
                    hVal[node] = Heuristic(pts[node], pts[finish]);
                    fVal[node] = gVal[node] + hVal[node];
                    nodeQueue.Add(node);
                    nodesToClear.Add(node);
                }
                else if (nodeStates[node] == NodeState.OPEN)
                {
                    //calculate the possible cost of doing this path
                    double transversalCost = (Vector3.Distance(pts[node], pts[currentNode])) * cost[node];
                    double possibleG = gVal[currentNode] + transversalCost;
                    //only change the parent to your own node and add this node to the list if it will result in a faster path
                    if (possibleG < gVal[node])
                    {
                        parent[node] = currentNode;
                        gVal[node] = possibleG;
                        fVal[node] = gVal[node] + hVal[node];
                        nodeQueue.Bubble(node);
                    }
                }
            }
        }
        nodeQueue = null;
        return PathState.NO_PATH;
    }
    #endregion

    #region sorting positions and identifying the closest mapped position to a point.
    public int GetClosestNode(Vector3 posn)
    {
        return(FindClosest(pts, GetScore(posn, TempNode.wrapNum)));
    }

    // Returns index of element closest
    // to target in arr[] 
    public static int FindClosest(List<Vector3> arr, int target)
    {
        int n = arr.Count;

        // Corner cases 
        if (target <= GetScore(arr[0], TempNode.wrapNum))
            return 0;
        if (target >= GetScore(arr[n - 1], TempNode.wrapNum))
            return n - 1;

        // Doing binary search 
        int i = 0, j = n, mid = 0;
        while (i < j)
        {
            mid = (i + j) / 2;

            if (GetScore(arr[mid], TempNode.wrapNum) == target)
                return mid;

            /* If target is less 
			than array element, 
			then search in left */
            if (target < GetScore(arr[mid], TempNode.wrapNum))
            {

                // If target is greater 
                // than previous to mid, 
                // return closest of two 
                if (mid > 0 && target > GetScore(arr[mid - 1], TempNode.wrapNum))
                    return GetClosest(arr, mid - 1, mid, target);

                /* Repeat for left half */
                j = mid;
            }

            // If target is 
            // greater than mid 
            else
            {
                if (mid < n - 1 && target < GetScore(arr[mid + 1], TempNode.wrapNum))
                    return GetClosest(arr, mid, mid + 1, target);
                i = mid + 1; // update i 
            }
        }

        // Only single element 
        // left after search 
        return mid;
    }

    // Method to compare which one 
    // is the more close We find the 
    // closest by taking the difference 
    // between the target and both 
    // values. It assumes that val2 is 
    // greater than val1 and target 
    // lies between these two. 
    public static int GetClosest(List<Vector3> arr, int ind1, int ind2,int target)
    {
        if (target - GetScore(arr[ind1], TempNode.wrapNum) >= GetScore(arr[ind2], TempNode.wrapNum) - target)
            return ind2;
        else
            return ind1;
    }

    private static int GetScore(Vector3 position, int wrapNum)
    {
        return ((int)position.x) + wrapNum * ((int)position.y) + wrapNum * wrapNum * ((int)position.z);
    }

    private class TempNode: IComparable
    {
        public static int wrapNum;
        public Vector3 position;
        public int score;
        public int originalIndex;

        public TempNode(Vector3 position, int originalIndex)
        {
            this.position = position;
            this.originalIndex = originalIndex;
            score = GetScore(position, wrapNum);
        }

        public int CompareTo(object obj)
        {
            if(obj.GetType() != typeof(TempNode))
            {
                return -1;
            }
            return score.CompareTo(((TempNode)obj).score);
        }
    }
    #endregion

    //calculates heuristic for A* search
    private float Heuristic(Vector3 start, Vector3 finish)
    {
        return Vector3.Distance(start, finish);
    }

    //resets the map
    private void ClearNodes()
    {
        foreach(int node in nodesToClear)
        {
            nodeStates[node] = NodeState.UNTESTED;
            gVal[node] = 0;
            hVal[node] = 0;
            fVal[node] = 0;
            parent[node] = -1;
            index[node] = -1;
        }
    }
}

