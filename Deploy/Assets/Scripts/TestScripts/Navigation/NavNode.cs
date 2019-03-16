using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NavNode
{
    public NodeState State { get; internal set; }
    public bool[] Connections { get; internal set; }

    public NavNode(bool[] Connections)
    {
        State = NodeState.UNTESTED;
        this.Connections = Connections;
    }
}

