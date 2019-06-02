using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravityBody))]
public class SphericalMovementPlayer : MonoBehaviour
{

    // public vars
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 220;
    public LayerMask groundedMask;
    AnimationController anim;

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

        anim = GetComponent<AnimationController>();
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
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

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
                    anim.PlayLoopingAnim("Move_Forward");

                }
                else
                {
                    anim.PlayLoopingAnim("Move_Backward");
                }
            }

        }

        if (moveDir.x <= 0.01f && moveDir.z <= 0.01f)
        {
            anim.PlayLoopingAnim("Idle");
        }



        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rigidbody.AddForce(transform.up * jumpForce);
            }
        }

        // Grounded check
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
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