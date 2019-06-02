using UnityEngine;
using System;

public class SmoothFollowCam : MonoBehaviour
{

    // The target we are following
    [SerializeField]
    private Transform target;
    // The distance in the x-z plane to the target
    [SerializeField]
    private float distance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField]
    private float height = 5.0f;

    [SerializeField]
    public float rotationDamping;
    [SerializeField]
    public float heightDamping;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotationDamping * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target.position, heightDamping * Time.deltaTime);

    }
}