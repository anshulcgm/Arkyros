using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(GravityBody))]
public class GhostController : MonoBehaviour
{

    // public vars
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 220;
    private float finalSpeed;

    //public bool enemyInRange = false;
    //public LayerMask groundedMask;

    private float speed_Multiplier = 1;
    private bool isSprinting;
    private int attackDelay;
    private int blastForce = 20;

    private AnimationController anim;
    DateTime attackStart;

    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;

    // System vars
    Camera cam;
    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;

    Rigidbody rigidbody;


    SoundManager soundManager;

    void Awake()
    {
        //enemies = GameObject.FindGameObjectsWithTag("enemy");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<AnimationController>();
        soundManager = GetComponent<SoundManager>();
        cam = Camera.main;


    }

    void Update()
    {

        // Look rotation:
        /*
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
        */

        // Calculate movement:
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        Vector3 moveDir = desiredMoveDirection.normalized;
        //finalSpeed = stats.get((int)stat.Speed);
        finalSpeed = walkSpeed * speed_Multiplier;
        Vector3 targetMoveAmount = moveDir * finalSpeed;

        



        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.3f);



        /*
        if (Math.Abs(moveDir.x) > Math.Abs(moveDir.z))
        {
            if (!Input.GetButtonDown("Jump"))
            {
                if (moveDir.x > 0)
                {
                    anim.PlayLoopingAnim("Move_Right");
                }
                else
                {

                    anim.PlayLoopingAnim("Move_Left");
                }
            }
        }
        else if (Math.Abs(moveDir.x) < Math.Abs(moveDir.z))
        {
            if (!Input.GetButtonDown("Jump"))
            {
                if (moveDir.z > 0)
                {
                    if (isSprinting)
                    {

                        soundManager.play("Sprint");
                        //setAllTriggersFalse();
                        anim.PlayLoopingAnim("Move_ForwardLunge");
                        speed_Multiplier = 2; //sprint speed
                        Camera.main.transform.localPosition = new Vector3(0f, 16f, 8f);
                    }
                    else
                    {

                        soundManager.play("walk");
                        anim.PlayLoopingAnim("Move_Forward");
                        Camera.main.transform.localPosition = new Vector3(0f, 16f, 3.1f);
                    }
                }
                else
                {
                    //setAllTriggersFalse();
                    anim.PlayLoopingAnim("Move_Backward");
                }
            }

        }
        */

        if (moveDir.x <= 0.01f && moveDir.z <= 0.01f)
        {
            /*
            anim.SetBool("RunForwardBool", false);
            anim.SetBool("WalkBackwardsBool", false);
            anim.SetBool("WalkRightBool", false);
            anim.SetBool("WalkLeftBool", false);
            anim.SetBool("WalkForwardBool", false);
            */

            anim.PlayLoopingAnim("Idle");


            soundManager.playOneShot("CapeFlutter");
        }
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        //Attack
        if (Input.GetMouseButton(0) && attackDelay == 0)
        {
            //Debug.Log("Attack was pressed");
            //setAllTriggersFalse();
            anim.StartOverlayAnim("Swing_Heavy_1", 0.5f, 0.5f);
            attackStart = DateTime.Now;
            attackDelay = 60; //can attack 60 frames later

        }

        if ((DateTime.Now - attackStart).TotalSeconds < 1)
        {

            soundManager.playOneShot("BasicScytheAttack");
            Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 20, 50);
            Debug.Log("REEEEEEEEE");
            foreach (Collider hit in hits)
            {
                // Detects if the object is an "enemy" and if so destroys it
                if (hit.gameObject.tag == "Enemy")
                {
                    Debug.Log(hit.gameObject.name);

                    hit.gameObject.GetComponent<Rigidbody>().AddForce(blastForce * (hit.gameObject.transform.position - this.gameObject.transform.position).normalized);

                    //Instantiate(damageDealt, hit.gameObject.transform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            //anim.SetBool("Swing_HeavyBool", false);

        }

        if (attackDelay > 0)
        {
            attackDelay--;
        }


        // Jump
        if (Input.GetButtonDown("Jump"))
        {

            if (grounded)
            {
                //Debug.Log("Jump was pressed");
                //rigidbody.velocity = (transform.up * jumpForce);
                //setAllTriggersFalse();
                //anim.SetBool("JumpBool", true);
                rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            }
        }


        //Sprint
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            if (!isSprinting)
            {
                isSprinting = true;
            }
            else if (isSprinting)
            {
                speed_Multiplier = 1;

                isSprinting = false;
            }
        }

        // Grounded check
        Ray ray = new Ray(transform.position + transform.up, -transform.up);
        //RaycastHit rayHit;

        if (Physics.Raycast(ray, 1.1f)) //1 is placeholder for model height
        {
            //anim.SetBool("JumpBool", false);
            grounded = true;
        }
        else
        {
            grounded = false;
        }


    }

    /*
    public void setAllTriggersFalse()
    {
        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            anim.SetBool(parameter.name, false);
        }
    }
    */

    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }
}

