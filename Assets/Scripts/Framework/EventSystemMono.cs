using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemMono : MonoBehaviour, IMono {
    private EventSystem eventSystem = new EventSystem();

    public IClass GetMainClass()
    {
        return eventSystem;
    }

    public void SetMainClass(IClass ic)
    {
        eventSystem = (EventSystem)ic;
    }

    void LateUpdate()
    {
        eventSystem.HandleEvents();
    }
}