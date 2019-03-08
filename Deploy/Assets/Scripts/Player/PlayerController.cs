using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float sprint_Speed;
    public float jump_Force;

    private float speed_Multiplier;
    private bool isSprinting;
    private bool onGround;
    private Rigidbody rb;
    void Start()
    {
        speed_Multiplier = 1.0f;
        onGround = true;
        isSprinting = false;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = 0.0f;

        if (Input.GetKeyDown(KeyCode.W) && !isSprinting)
        {
            speed_Multiplier *= sprint_Speed;
            isSprinting = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && isSprinting)
        {
            speed_Multiplier /= sprint_Speed;
            isSprinting = false;
        }
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            onGround = false;
        }
        Vector3 movement = new Vector3(moveHorizontal * getSpeed(), moveUp, moveVertical * getSpeed());
        rb.AddForce(movement);
    }
    float getSpeed()
    {
        return speed * speed_Multiplier;
    }
    void Jump()
    {
        rb.AddForce(new Vector3(0, jump_Force, 0), ForceMode.Impulse);
        onGround = true;
    }

}
