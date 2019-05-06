using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkflight : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private Animator anim;

    Rigidbody rigidbody;

    DateTime start;
    Stats stats;
    private bool cast;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();

        cooldown = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            start = DateTime.Now;
            anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
                                      //placeholder time, divide by 60 for cooldown in seconds


            
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            rigidbody.AddForce(transform.up * 1000, ForceMode.Impulse); //jumps super high
            stats.buffs[(int)buff.Gravityless] += 240; //lets you fly
            cooldown = 240;
            cast = true;
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
