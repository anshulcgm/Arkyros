﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullRush : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private AnimationController anim;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    private int dashNum = 500;
    private int enemySetback = 400;

    GhostSoundManager ghostSoundManager;
    //might not always be Ghost, need different one for each class.

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        tcs = GetComponent<TargetCenterScreen>();

        cooldown = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            anim.StartOverlayAnim("BullRush", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {

            /*
             * All the code for the ability that you want to write
             * transform.forward for the direction the player is 
             * maybe setting colliders
             * instantiating new objects
             * to damage enemy, EnemyGameObject.GetComponent<StatManager>().changeHealth(amount), amount can be positive or negative
             */
            GetComponent<Rigidbody>().AddForce(transform.forward * dashNum); //dash forward


            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && !cast)
        {
            //collision.gameObject.GetComponent<Stats>().heal(20);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * -enemySetback);
        }
    }
}
