using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemMono : MonoBehaviour, IMono {
    public EventSystem EventSystem { get; set; }
    public Camera playerCam;


    public IClass GetMainClass()
    {
        return EventSystem;
    }

    public void SetMainClass(IClass ic)
    {
        EventSystem = (EventSystem)ic;
    }

    public void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("playercam").GetComponent<Camera>();
    }

    int counter = 0;
    void LateUpdate()
    {
        if(counter > 10)
        {
            ObjectHandler.HandleCachedObjects(playerCam);
            counter = 0;
        }
        counter++;
    }
}