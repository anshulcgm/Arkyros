using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkflight : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            //start = DateTime.Now;
            anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds


            rigidbody.AddForce(transform.up * 1000, ForceMode.Impulse); //jumps super high
            //reee flying code
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
