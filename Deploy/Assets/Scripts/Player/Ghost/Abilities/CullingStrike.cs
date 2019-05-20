using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingStrike : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private Animator anim;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    GhostSoundManager ghostSoundManager;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
            anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {

            ghostSoundManager.playCullingStrike();


            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}

