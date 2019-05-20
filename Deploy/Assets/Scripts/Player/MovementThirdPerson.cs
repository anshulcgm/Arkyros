using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementThirdPerson : MonoBehaviour
{
    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    //public Animator anim;
    public float Speed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController controller;
    public bool isGrounded;

    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        InputMagnitude();
    }

    void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Veritcal");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if(blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
        }
    }

    void InputMagnitude()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Veritcal");

        //animations

        //Calculate the Input Magnitude
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //Physically move player
        if(Speed > allowPlayerRotation)
        {
            //anim stuff
            PlayerMoveAndRotation();
        }
        else if (Speed < allowPlayerRotation)
        {
            //anim stuff
        }
    }
}
