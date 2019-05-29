using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;
using System;

public class FPCPlanet : MonoBehaviour {
	// public vars
	public float mouseSensitivityX = 1;
	public float mouseSensitivityY = 1;
	public float walkSpeed = 6;

    public float rotSpeed = 10;
	public LayerMask groundedMask;

	public SmoothFollowCam sm;
	
	// System vars
	bool grounded;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	float verticalLookRotation;
	public Transform cameraTransform;
	Rigidbody rigidbody;
	
	
	void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		rigidbody = GetComponent<Rigidbody> ();
	}
		
    Vector3 fullRot = Vector3.zero;
    float desiredVertRot;
	void Update() {
		// Look rotation:
		// Calculate movement:
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		
		Vector3 moveDir = new Vector3(inputX,0, inputY).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount,targetMoveAmount,ref smoothMoveVelocity,.15f);
		
		
		// Grounded check
		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask)) {
			grounded = true;
		}
		else {
			grounded = false;
		}
		desiredVertRot = verticalLookRotation;
	
	    if(!Input.GetMouseButton(1)){
			sm.rotationDamping = 30;
			sm.heightDamping = 30;

        }else{
			sm.rotationDamping = 3;
			sm.heightDamping = 10;
        }
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
            verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation,-75,75);
            cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
        
	}

	DateTime lastFrame = DateTime.MaxValue;
	public void FixedUpdate(){
		double delta = (DateTime.Now - lastFrame).TotalSeconds;
		if(delta > 0)
		{
			//turn mouse-based movement on and off
			Vector3 localMove;
			if(Input.GetMouseButton(0)){
				localMove = cameraTransform.TransformDirection(moveAmount) * Time.fixedDeltaTime;		    
			}else{
				localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
			}
			rigidbody.MovePosition(rigidbody.position + localMove);
		}
		lastFrame = DateTime.Now;
	}
}
