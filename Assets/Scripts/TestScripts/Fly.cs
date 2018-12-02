using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {
    public float delta = 0.1f;
    public Rigidbody r;
    public Transform character;
    public Animator characterAnimator;
    public float levitateSpeed;
    public float moveSpeed;
    public float sprintSpeed;

    // Use this for initialization
    void Start () {
        r = GetComponent<Rigidbody>();        
	}

    bool inAir = true;
    public float jumpSpeed;
    public float gravForce;
    public float accel;
    public float deAccel;
	// Update is called once per frame
	void Update () {


        if (!inAir)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                characterAnimator.SetBool("isJumping", true);
                characterAnimator.SetBool("isMoving", false);
                characterAnimator.SetBool("isSprinting", false);
                inAir = true;
                r.velocity -= character.forward * jumpSpeed;
                return;
            }

            characterAnimator.SetBool("isJumping", false);
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Q))
            {
                if (r.velocity.magnitude > sprintSpeed)
                {
                    Debug.Log("running counter spr " + r.velocity.magnitude);
                    r.AddForce(-r.velocity.normalized * deAccel);
                }

                r.AddForce(-character.up * accel);
                characterAnimator.SetBool("isMoving", true);
                characterAnimator.SetBool("isSprinting", true);

            }
            else if (Input.GetKey(KeyCode.W))
            {
                if (r.velocity.magnitude > moveSpeed)
                {
                    Debug.Log("running counter mov " + r.velocity.magnitude);
                    r.AddForce(-r.velocity.normalized * deAccel);
                }
                r.AddForce(-character.up * accel);
                characterAnimator.SetBool("isMoving", true);
                characterAnimator.SetBool("isSprinting", false);
            }
            else
            {
                characterAnimator.SetBool("isMoving", false);
                characterAnimator.SetBool("isSprinting", false);
                r.AddForce(-r.velocity.normalized);
            }            
        }
        r.AddForce(character.forward * gravForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        inAir = false;
    }
}
