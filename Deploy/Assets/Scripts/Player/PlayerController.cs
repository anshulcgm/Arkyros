using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 50;
    //public float sprint_Speed = 5;
    public float jump_Force = 10;

    private float speed_Multiplier = 1;
    private bool isSprinting;
    private Rigidbody rb;
    void Start()
    {
        speed_Multiplier = 1.0f;
        isSprinting = false;
        rb = GetComponent<Rigidbody>();
    }
	bool isGrounded(){
		return rb.position.y < 1.0;
	}
    void FixedUpdate()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = 0.0f;
        /*
        if (Input.GetKeyDown(KeyCode.Q) && !isSprinting)
        {
            speed_Multiplier *= sprint_Speed;
            isSprinting = true;
        }
        if (Input.GetKeyDown(KeyCode.Q) && isSprinting)
        {
            speed_Multiplier /= sprint_Speed;
            isSprinting = false;
        }
        */

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isSprinting)
            {
                speed_Multiplier = 2; //sprint speed
                isSprinting = true;
            }
            else if (isSprinting)
            {
                speed_Multiplier = 1;
                isSprinting = false;
            }
        }

        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
		Vector3 targetDirection = new Vector3(moveHorizontal * getSpeed(), moveUp, moveVertical*getSpeed());
		targetDirection = Camera.current.transform.TransformDirection(targetDirection);
		targetDirection.y = 0.0f;
        rb.AddForce(targetDirection);
    }
    float getSpeed() {
        return speed * speed_Multiplier;
    }
    void Jump() {
        rb.AddForce(new Vector3(0, jump_Force, 0), ForceMode.Impulse);
    }
  
}
