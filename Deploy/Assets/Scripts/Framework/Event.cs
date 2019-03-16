using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//this file has interfaces for various events.

public interface IUnityEvent
{
    string EventMessage { get; }
}

public interface IRadiusEvent : IUnityEvent
{
    float Radius { get; }
    Transform Position { get; }
}

public interface ILineOfSightEvent : IUnityEvent
{
    Vector3 Origin { get; }
}

public interface ITimedEvent: IUnityEvent
{
    float Duration { get; }
    DateTime CreationTime { get; }
}

public interface ITargetEvent : IUnityEvent
{
    int TargetInstanceId { get; }
}

