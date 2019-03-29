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
        Vector3 vel = Vector3.zero;
            if (Input.GetKey(KeyCode.Space))
            {
                vel += -character.forward * jumpSpeed;
            }
            if (Input.GetKey(KeyCode.W))
            {
                vel += -character.up * moveSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                vel += character.forward * jumpSpeed;
            }
        r.velocity = vel;
    }

}
