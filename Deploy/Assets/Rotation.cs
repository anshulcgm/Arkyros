using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Plane plane = new Plane(transform.position.normalized, transform.position);
        Vector2 mapTransform = plane.GetMappedPoint(transform.position);
        Vector2 mapCam = plane.GetMappedPoint(cam.transform.position);

        Vector2 desiredMoveDirection = mapTransform - mapCam;
        float angle = Mathf.Atan2(desiredMoveDirection.x, desiredMoveDirection.y);
        angle *= Mathf.Rad2Deg;

        Vector3 reee = mapTransform.x * plane.xDir + mapTransform.y * plane.yDir;
        Vector3 reeee = mapCam.x * plane.xDir + mapCam.y * plane.yDir;
        Debug.DrawLine(reee + transform.position, reeee + transform.position, Color.red);
        //Debug.Log(reee + "" + reeee);

        transform.localRotation = Quaternion.Euler(0, angle, 0);

        
    }
}
