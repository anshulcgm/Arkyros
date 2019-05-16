using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathReciever
{
    //the owner of this class
    public GameObject owner = null;
    //the current path
    public List<Vector3> path;
    //the current pathTask
    PathTask currentTask = null;
    //the current pathLength
    public float pathLength = 0.0f;

    public int costIncrease = 0;

    public WalkingMap map;

    public PathReciever(WalkingMap map, GameObject owner, int costIncrease)
    {
        this.owner = owner;
        this.costIncrease = costIncrease;
        this.map = map;
    }
	
    //attempts to update the path, if possible. Returns whether the update succeeded or not
    public bool tryUpdatePath()
    {
        if (currentTask != null && currentTask.finishedTask)
        {
            path = currentTask.pathHolder.path;
            if(path == null)
            {
                return true;
            }

            Vector3 prev = Vector3.zero;
            bool start = true;
            foreach (Vector3 v in path)
            {
                if (!start) { Debug.DrawLine(prev, v, Color.green, 1.0f); }

                start = false;
                prev = v;
            }
            return true;
        }
        return false;
    }

    //updates the pathTask if it hasn't been completed, makes a new pathTask if it has.
    public void updatePathTask(Vector3 start, Vector3 finish)
    {        
        if (currentTask == null || currentTask.finishedTask)
        {            
            //get the old pathHolder, and set the path if the task has been finished.
            PathHolder oldPathHolder = null;
            if (currentTask != null && currentTask.pathHolder != null)
            {
                oldPathHolder = currentTask.pathHolder;
                path = currentTask.pathHolder.path;
            }

            //create a new task, set it to the current task
            currentTask = new PathTask(start, finish, oldPathHolder, costIncrease);
            //add the task
            map.AddTask(currentTask);
        }
        else
        {
            currentTask.start = start;
            currentTask.finish = finish;
        }
    }

    public bool hasPathTo(Vector3 destination)
    {
        if (path == null || path.Count == 0)
        {
            return false;
        }
        return Vector3.Distance(destination, path[path.Count - 1]) < 0.5f;
    }
}
