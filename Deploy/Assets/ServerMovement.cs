using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMovement : MonoBehaviour
{
    public bool isMoving = false;
    public bool isAirborne = false;

    public GameObject player;

    public Rigidbody r;

    public float jumpForce;

    public float gravity;

    public float moveSpeed;
    Stats stats;

    //public bool grounded = false;

    public LayerMask layerMask;

    PlayerScript p;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        r = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        p = GetComponent<PlayerScript>();
    }
    float tiltAroundX, tiltAroundY = 0;
    Vector3 moveAmount = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        moveAmount = Vector3.zero;
        Vector2 angleAdd = Vector2.zero;
        bool moving = false;
        if (p.IsPressed('w'))
        {
            moving = true;
            angleAdd.y += 1;
            moveAmount += player.transform.forward;
        }
        if (p.IsPressed('a'))
        {
            moving = true;
            angleAdd.x += 1;
            moveAmount -= player.transform.right;
        }
        if (p.IsPressed('s'))
        {
            moving = true;
            angleAdd.y -= 1;
            moveAmount -= player.transform.forward;
        }
        if (p.IsPressed('d'))
        {
            moving = true;
            angleAdd.x -= 1;
            moveAmount += player.transform.right;
        }
        moveAmount = moveAmount.normalized;

        if (p.IsPressed(' ') && Physics.Raycast(transform.position + transform.up, -transform.up, 10f, layerMask))
        {
            r.AddForce(jumpForce * transform.position.normalized, ForceMode.Impulse);
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
        // Apply movement to rigidbody
        Vector3 localMove = moveAmount * Time.fixedDeltaTime * moveSpeed;
        Debug.DrawLine(transform.position, transform.position + moveAmount, Color.red);
        r.MovePosition(r.position + localMove);
    }
}
