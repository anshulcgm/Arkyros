using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravityBody))]
public class FirstPersonController : MonoBehaviour
{

    // public vars
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 220;
    public float finalSpeed;
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


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
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
            if (moveDir.x > 0)
            {
                anim.SetTrigger("WalkRight");
            }
            else anim.SetTrigger("WalkLeft");

        }
        else if (Math.Abs(moveDir.x) < Math.Abs(moveDir.z))
        {
            if (moveDir.z > 0)
            {
                if (isSprinting)
                {
                    anim.SetTrigger("RunForward");
                }
                else anim.SetTrigger("WalkForward");
            }
            else anim.SetTrigger("WalkBackwards");
        }
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        //Attack
        if (Input.GetMouseButton(0) && attackDelay == 0)
        {
            //Debug.Log("Attack was pressed");
            anim.SetTrigger("Swing_Heavy");
            attackStart = DateTime.Now;
            attackDelay = 60; //can attack 60 frames later

        }

        if ((DateTime.Now - attackStart).TotalSeconds < 1)
        {
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
                anim.SetTrigger("Jump");
                rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            }
        }

        //Sprint
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

        // Grounded check
        Ray ray = new Ray(transform.position + transform.up, -transform.up);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, 1 + .1f)) //1 is placeholder for model height
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        
    }

    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }
}
