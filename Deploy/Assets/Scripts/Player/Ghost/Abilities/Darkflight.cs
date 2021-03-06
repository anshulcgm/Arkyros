﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkflight : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 540;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    SoundManager soundManager;
    Rigidbody rigidbody;

    DateTime start;
    Stats stats;
    private bool cast;

    public GameObject DarkFlightParticleEffect;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<AnimationController>();
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        soundManager = GetComponent<SoundManager>();

        cooldown = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<PlayerScript>().IsPressed("q") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            start = DateTime.Now;
            GameObject particleEffect = Instantiate(DarkFlightParticleEffect, transform.position, Quaternion.Euler(90, 0, 0));
            //anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            //placeholder time, divide by 60 for cooldown in seconds



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cast = true;

            rigidbody.AddForce(transform.up * 20, ForceMode.Impulse); //jumps super high

            soundManager.playOneShot("DarkflightTakeoff");
            //anim.PlayLoopingAnim("Fly_Forward");

            stats.buffs[(int)buff.Gravityless] += 240; //lets you fly

            soundManager.play("DarkflightFlight");

            cooldown = maxCooldown;
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (/*collision.gameObject.tag.Equals("planet") &&*/ cast) //lands back on the ground
        {
            soundManager.playOneShot("DarkflightLanding");
            anim.PlayLoopingAnim("Idle");
            cast = false;
        }
    }
}
