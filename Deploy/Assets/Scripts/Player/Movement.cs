using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float mouseSensitivity;
    public GameObject camPitch;
    public GameObject playerModel;
    public GameObject playerDir;

    public Rigidbody r;
    public float slerpSpeed;

    public float jumpForce;

    public float gravity;

    public float moveSpeed;

    public bool grounded = false;

    public AnimationController anim;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        r = GetComponent<Rigidbody>();
        anim.PlayLoopingAnim("Idle");
    }

    float tiltAroundX, tiltAroundY = 0;
    Vector3 moveAmount = Vector3.zero;
    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.FromToRotation(transform.up, transform.position.normalized) * transform.rotation;

        tiltAroundY += Input.GetAxis("Mouse X") * mouseSensitivity;
        tiltAroundX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        camPitch.transform.localRotation = Quaternion.Euler(tiltAroundX, tiltAroundY, 0);        
        Quaternion targetDirRot = Quaternion.Euler(0, tiltAroundY, 0);
        playerDir.transform.localRotation = targetDirRot;

        moveAmount = Vector3.zero;
        Vector2 angleAdd = Vector2.zero;
        bool moving = false;
        if(Input.GetKey("w")){
            moving = true;
            angleAdd.y += 1;       
            moveAmount += playerDir.transform.forward;
        }
        if(Input.GetKey("a")){
            moving = true;
            angleAdd.x += 1;
            moveAmount -= playerDir.transform.right;
        }
        if(Input.GetKey("s")){
            moving = true;
            angleAdd.y -= 1;
            moveAmount -= playerDir.transform.forward;
        }
        if(Input.GetKey("d")){
            moving = true;
            angleAdd.x -= 1;
            moveAmount += playerDir.transform.right;
        }
        moveAmount = moveAmount.normalized;

        if(Input.GetKey(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, 1)){
            r.AddForce(jumpForce * transform.position.normalized);            
        }

        if(moving){
            float angle = Mathf.Atan2(angleAdd.y, angleAdd.x) * Mathf.Rad2Deg - 90.0f;
            Quaternion targetModelRot = Quaternion.Euler(0, tiltAroundY + angle, 0);
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, targetModelRot, Time.deltaTime * slerpSpeed);
        }
         r.AddForce(-gravity * transform.position.normalized);
         
    }

    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = moveAmount * Time.fixedDeltaTime * moveSpeed;
        Debug.DrawLine(transform.position, transform.position + moveAmount, Color.red);        
        r.MovePosition(r.position + localMove);
    }
}
