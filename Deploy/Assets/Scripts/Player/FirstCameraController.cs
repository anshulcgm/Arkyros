using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCameraController : MonoBehaviour
{
    public GameObject player;
    public float speedH = 4.0F;
    public float speedV = 4.0F;

    private float yaw = 0.0F;
    private float pitch = 0.0F;
    private Vector3 offset;
    //private Vector3 headCorrection;
	private bool first_person = true; 

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        transform.position = player.transform.position;
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        
    }

    void LateUpdate()
    {
		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
