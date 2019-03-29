using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float speedH = 2.0F;
    public float speedV = 2.0F;

    private float yaw = 0.0F;
    private float pitch = 0.0F;
    private Vector3 offset;
	private bool first_person = true; 

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 10, -10);
    }
    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
		if(Input.GetKeyDown(KeyCode.F) && first_person){
			first_person = false; 
		}
		if(Input.GetKeyDown(KeyCode.F) && !first_person){
			first_person = true; 
		}
    }

    void LateUpdate()
    {
		if(first_person){
			transform.position = player.transform.position;
		}
		if(!first_person){
			transform.position = player.transform.position + offset;
		}
		
    }
}
