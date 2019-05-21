using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(GravityBody))]
public class KnightController : MonoBehaviour
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

    private Animator anim;
    DateTime attackStart;

    // System vars

    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    Transform cameraTransform;
    Rigidbody rigidbody;

    cameraSoundManager sound;
    legSound movementSound;
    hammerSound attackSound;


    void Awake()
    {
        //enemies = GameObject.FindGameObjectsWithTag("enemy");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sound = Camera.main.GetComponent<cameraSoundManager>();
        movementSound = GameObject.Find("L_Foot_IK").GetComponent<legSound>();
        attackSound = GameObject.Find("Hammer_Head").GetComponent<hammerSound>();
    }

    void Update()
    {

        // Look rotation:
        
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        // Calculate movement:
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
        finalSpeed = walkSpeed * speed_Multiplier;
        Vector3 targetMoveAmount = moveDir * finalSpeed;
        if (Math.Abs(moveDir.x) > Math.Abs(moveDir.z))
        {
            if (!Input.GetButtonDown("Jump"))
            {
                if (moveDir.x > 0)
                {
                    setAllTriggersFalse();
                    anim.SetBool("WalkRightBool", true);
                }
                else
                {
                    setAllTriggersFalse();
                    anim.SetBool("WalkLeftBool", true);
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
                        movementSound.isWalking = false;
                        movementSound.isSprinting = true;
                        setAllTriggersFalse();
                        anim.SetBool("RunForwardBool", true);
                        speed_Multiplier = 2; //sprint speed
                        Camera.main.transform.localPosition = new Vector3(0f, 16f, 8f);
                    }
                    else
                    {
                        movementSound.isWalking = true;
                        movementSound.isSprinting = false;
                        anim.SetBool("RunForwardBool", false);
                        anim.SetBool("WalkForwardBool", true);
                        Camera.main.transform.localPosition = new Vector3(0f, 16f, 3.1f);
                    }
                }
                else
                {
                    setAllTriggersFalse();
                    anim.SetBool("WalkBackwardsBool", true);
                }
            }
            
        }
       
        if(moveDir.x <= 0.01f && moveDir.z <= 0.01f)
        {
            anim.SetBool("RunForwardBool", false);
            anim.SetBool("WalkBackwardsBool", false);
            anim.SetBool("WalkRightBool", false);
            anim.SetBool("WalkLeftBool", false);
            anim.SetBool("WalkForwardBool", false);

            movementSound.isWalking = false;
            movementSound.isSprinting = false;
        }
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        //Attack
        if (Input.GetMouseButton(0) && attackDelay == 0)
        {
            //Debug.Log("Attack was pressed");
            setAllTriggersFalse();
            anim.SetBool("Swing_HeavyBool", true);
            attackStart = DateTime.Now;
            attackDelay = 60; //can attack 60 frames later

        }

        if ((DateTime.Now - attackStart).TotalSeconds < 1)
        {
            
            attackSound.isSwinging = true;
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
            anim.SetBool("Swing_HeavyBool", false);
            attackSound.isSwinging = false;
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
                setAllTriggersFalse();
                anim.SetBool("JumpBool", true);
                rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                sound.isJumping = true;
            }
        }
        else
        {
            sound.isJumping = false;
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
            anim.SetBool("JumpBool", false);
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        
    }
    public void setAllTriggersFalse()
    {
        foreach(AnimatorControllerParameter parameter in anim.parameters)
        {
            anim.SetBool(parameter.name, false);
        }
    }
    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }
}
