using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool isMoving = false;
    public bool isAirborne = false;

    public float mouseSensitivity;
    public GameObject playerModel;
    public GameObject playerDir;

    public CinemachineFreeLook c;


    public Rigidbody r;
    public float slerpSpeed;

    public float jumpForce;

    public float gravity;

    public float moveSpeed;
    Stats stats;

    //public bool grounded = false;

    public LayerMask layerMask;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        r = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
    }

    float tiltAroundX, tiltAroundY = 0;
    Vector3 moveAmount = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        r.rotation = Quaternion.FromToRotation(transform.up, r.position.normalized) * r.rotation;

        Quaternion targetDirRot = Quaternion.Euler(0, c.m_XAxis.Value, 0);
        playerDir.transform.localRotation = targetDirRot;

        moveAmount = Vector3.zero;
        Vector2 angleAdd = Vector2.zero;
        bool moving = false;
        if (Input.GetKey("w"))
        {
            moving = true;
            angleAdd.y += 1;
            moveAmount += playerDir.transform.forward;
        }
        if (Input.GetKey("a"))
        {
            moving = true;
            angleAdd.x += 1;
            moveAmount -= playerDir.transform.right;
        }
        if (Input.GetKey("s"))
        {
            moving = true;
            angleAdd.y -= 1;
            moveAmount -= playerDir.transform.forward;
        }
        if (Input.GetKey("d"))
        {
            moving = true;
            angleAdd.x -= 1;
            moveAmount += playerDir.transform.right;
        }
        moveAmount = moveAmount.normalized;

        if (Input.GetKey(KeyCode.Space) && Physics.Raycast(transform.position + transform.up, -transform.up, 10f, layerMask))
        {
            r.AddForce(jumpForce * transform.position.normalized, ForceMode.Impulse);
        }

        if (moving)
        {
            float angle = Mathf.Atan2(angleAdd.y, angleAdd.x) * Mathf.Rad2Deg - 90.0f;
            Quaternion targetModelRot = Quaternion.Euler(0, c.m_XAxis.Value + angle, 0);
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, targetModelRot, Time.deltaTime * slerpSpeed);
        }

        if(stats.buffs[(int)buff.Gravityless] == 0)
        {
            r.AddForce(-gravity * transform.position.normalized);
        }        

        isMoving = moving;
        isAirborne = !Physics.Raycast(transform.position + transform.up, -transform.up, 10f, layerMask);
    }

    void FixedUpdate()
    {
        moveSpeed = stats.getFinal((int)stat.Speed);
        // Apply movement to rigidbody
        Vector3 localMove = moveAmount * Time.fixedDeltaTime * moveSpeed;
        Debug.DrawLine(transform.position, transform.position + moveAmount, Color.red);
        r.MovePosition(r.position + localMove);
    }
}
