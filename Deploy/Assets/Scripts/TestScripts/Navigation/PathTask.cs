﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTask
{
    public static double[] cost;

    //holds the start and finish positions
    public Vector3 start;
    public Vector3 finish;

    //holds the path info
    public PathHolder pathHolder { get; private set; }

    //holds the old path info
    public PathHolder oldPathHolder { get; private set; }

    public bool finishedTask { get; private set; }

    public int costIncrease;

    //initialization
    public PathTask(Vector3 start, Vector3 finish, PathHolder oldPathHolder, int costIncrease)
    {
        this.start = start;
        this.finish = finish;
        pathHolder = null;
        this.oldPathHolder = oldPathHolder;
        finishedTask = false;

        this.costIncrease = costIncrease;

        //decrease costs for old path
        if (oldPathHolder != null && oldPathHolder.nodePath != null)
        {
            foreach (int n in oldPathHolder.nodePath)
            {
                cost[n] -= costIncrease;
            }
        }
    }

    //completes the task. If the task has already been completed, it does nothing.
    public void finishTask(List<int> nodePath, List<Vector3> path, float pathLength)
    {
        //if we haven't already finished this task
        if(!finishedTask)
        {
               
            //make a new pathHolder
            pathHolder = new PathHolder();
            //give it all these characteristicts
            pathHolder.nodePath = nodePath;
            pathHolder.path = path;
            pathHolder.pathLength = pathLength;
            //make sure you can't finish the task again.
            finishedTask = true;

            if(costIncrease == 0)
            {
                return;
            }

            if(nodePath != null)
            {
                foreach (int n in nodePath)
                {
                    cost[n] += costIncrease;
                }
            }
        }        
    }
}

//holds the node path, the Vector 3 path, and the length of the path
public class PathHolder
{
    public List<int> nodePath;
    public List<Vector3> path;
    public float pathLength;
}
