using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapClass;

namespace Pathfinding
{
    public class Pathfinder
    {
        MapCreator gridBase;
        public GridNode startPosition;
        public GridNode endPosition;

        public volatile bool jobDone = false;

        PathfindMaster.PathfindingJobComplete completeCallback;
        List<GridNode> foundPath;

        public Pathfinder(GridNode start, GridNode target, PathfindMaster.PathfindingJobComplete callback)
        {
            startPosition = start;
            endPosition = target;
            completeCallback = callback;
            gridBase = MapCreator.GetInstance();
        }

        public void FindPath()
        {
            foundPath = FindPathActual(startPosition, endPosition);

            jobDone = true;
        }

        public void NotifyComplete()
        {
            if(completeCallback != null)
            {
                completeCallback(foundPath);
            }
        }

        public List<GridNode> FindPathActual(GridNode start, GridNode target)
        {
            List<GridNode> foundPath = new List<GridNode>();

            //Two lists, one for nodes that need to be checked and one for nodes that have already been checked 
            List<GridNode> openSet = new List<GridNode>();
            HashSet<GridNode> closedSet = new HashSet<GridNode>();

            //Start adding to the open set
            openSet.Add(start);

            while(openSet.Count > 0)
            {
                GridNode currentNode = openSet[0];

                for(int i = 0; i < openSet.Count; i++)
                {
                    if(openSet[i].fCost < currentNode.fCost || (openSet[i].fCost 
                        == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {
                        if (!currentNode.Equals(openSet[i]))
                        {
                            currentNode = openSet[i];
                        }
                    }
                }

                //remove current node from the open set and add to the closed set 
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                //if the current node is the target node
                if (currentNode.Equals(target))
                {
                    foundPath = RetracePath(start, currentNode);
                    break;
                }

                foreach(GridNode neighbour in GetNeighbours(currentNode, true))
                {
                    if (!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            //calculate the new costs
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, target);
                            //Assign parent node
                            neighbour.parentNode = currentNode;
                            //Add neighbour node to open set
                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }

            return foundPath;
        }

        private List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
        {
            List<GridNode> path = new List<GridNode>();

            GridNode currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);

                currentNode = currentNode.parentNode;
            }

            //Reverse the list 
            path.Reverse();

            return path;
        }

        private List<GridNode> GetNeighbours(GridNode node, bool GetVerticalNeighbours = false)
        {
            List<GridNode> retList = new List<GridNode>();

            for(int x = -1; x <= 1; x++)
            {
                for(int yIndex = -1; yIndex <= 1; yIndex++)
                {
                    for(int z = -1; z <= 1; z++)
                    {
                        int y = yIndex;

                        if (!GetVerticalNeighbours)
                        {
                            y = 0;
                        }

                        if(x == 0 && y == 0 && z == 0)
                        {

                        }
                        else
                        {
                            GridNode searchPos = new GridNode();

                            searchPos.x = node.x + x;
                            searchPos.y = node.y + y;
                            searchPos.z = node.z + z;

                            GridNode newNode = GetNeighbourNode(searchPos, true, node);

                            if(newNode != null)
                            {
                                retList.Add(newNode);
                            }
                        }
                    }
                }
            }

            return retList;
        }

        private GridNode GetNeighbourNode(GridNode adjPos, bool searchTopDown, GridNode currentNodePos)
        {
            GridNode retVal = null;

            GridNode node = GetNode(adjPos.x, adjPos.y, adjPos.z);

            if(node != null && node.isWalkable)
            {
                retVal = node;
            }
            else if (searchTopDown)
            {
                adjPos.y -= 1;
                GridNode bottomBlock = GetNode(adjPos.x, adjPos.y, adjPos.z);

                if(bottomBlock != null && bottomBlock.isWalkable)
                {
                    retVal = bottomBlock;
                }

                else
                {
                    adjPos.y += 2;
                    GridNode topBlock = GetNode(adjPos.x, adjPos.y, adjPos.z);
                    if(topBlock != null && topBlock.isWalkable)
                    {
                        retVal = topBlock;
                    }
                }
            }

            int originalX = (int)(adjPos.x - currentNodePos.x);
            int originalZ = (int)(adjPos.z - currentNodePos.z);

            if (Mathf.Abs(originalX) == 1 && Mathf.Abs(originalZ) == 1)
            {
                // the first block is originalX, 0 and the second to check is 0, originalZ
                //They need to be pathfinding walkable
                GridNode neighbour1 = GetNode(currentNodePos.x + originalX, currentNodePos.y, currentNodePos.z);
                if (neighbour1 == null || !neighbour1.isWalkable)
                {
                    retVal = null;
                }

                GridNode neighbour2 = GetNode(currentNodePos.x, currentNodePos.y, currentNodePos.z + originalZ);
                if (neighbour2 == null || !neighbour2.isWalkable)
                {
                    retVal = null;
                }
            }

            //and here's where we can add even more additional checks
            if (retVal != null)
            {
                //Example, do not approach a node from the left
                /*if(node.x > currentNodePos.x) {
                    node = null;
                }*/
            }

            return retVal;

        }

        private GridNode GetNode(float x, float y, float z)
        {
            int x1 = Mathf.RoundToInt(x);
            int y1 = Mathf.RoundToInt(y);
            int z1 = Mathf.RoundToInt(z);
            GridNode n = null;

            lock (gridBase)
            {
                n = gridBase.GetNode(x1, y1, z1);
            }
            return n;
        }

        private int GetDistance(GridNode posA, GridNode posB)
        {
            //We find the distance between each node
            //not much to explain here

            int distX = (int)Mathf.Abs(posA.x - posB.x);
            int distZ = (int)Mathf.Abs(posA.z - posB.z);
            int distY = (int)Mathf.Abs(posA.y - posB.y);

            if (distX > distZ)
            {
                return 14 * distZ + 10 * (distX - distZ) + 10 * distY;
            }

            return 14 * distX + 10 * (distZ - distX) + 10 * distY;
        }
    }
}
