using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCameraController : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;


    public Transform player;
    public float speedH = 4.0F;
    public float speedV = 4.0F;

    private float yaw = 0.0F;
    private float pitch = 0.0F;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 0, -5);
    }

    // Update is called once per frame
    void Update()
    {
        //offset = Camera.current.transform.TransformDirection(offset);
        //offset.y = 5;
        //transform.position = player.transform.position + offset;

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, Y_ANGLE_MIN, Y_ANGLE_MAX);
        //Camera.current.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position);
    }
}
