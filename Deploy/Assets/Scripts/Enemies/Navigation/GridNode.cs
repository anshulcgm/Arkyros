using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode{

    //Node position values
    public float x;
    public float y;
    public float z;

    //Node costs 
    public float hCost;
    public float gCost;

    public float fCost
    {
        get //Easy to get because fCost is the sum of gCost and hCost
        {
            return gCost + hCost;
        }
    }

    public GridNode parentNode;
    public bool isWalkable = true;

    public GameObject worldObject;

    public NodeType nodeType;
    public enum NodeType
    {
        ground,
        air
    }

}
