﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landfall : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 480;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    public int numForward = 80;
    public int numUp = 80;
    private int sphereRadius = 20;
    private int enemySetback = 150;

    public GameObject particleLanding;
    bool particleSpawned;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        tcs = GetComponent<TargetCenterScreen>();

        soundManager = GetComponent<SoundManager>();

        cooldown = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<PlayerScript>().IsPressed("q") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            particleSpawned = false;
            //transform.rotation = camera.transform.rotation;


            //put any setup code here, before the ability is actually cast




        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {            
            //anim.PlayLoopingAnim("Flight"); //this tells the animator to play the right animation, what strength, what duration
            anim.StartOverlayAnim("Jump", 0.5f, 1f);
            soundManager.playOneShot("LandfallJump");
            soundManager.play("LandfallSustain");

            model.transform.rotation = camera.transform.rotation;
            rigidbody.AddForce(model.transform.forward * numForward, ForceMode.Impulse);
            rigidbody.AddForce(transform.up * numUp, ForceMode.Impulse);

            anim.PlayLoopingAnim("Flight");

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
        
        if (/*collision.gameObject.tag.Equals("planet") &&*/ cast) //lands back on the ground
        {
            //soundManager.stop();
            if (!particleSpawned)
            {
                Instantiate(particleLanding, transform.position + transform.up, transform.rotation);//particle effect once
                soundManager.playOneShot("LandfallFall");
                particleSpawned = true;
            }
            
            anim.PlayLoopingAnim("Standard"); //Idle
            //soundManager.stop();
            

            Collider[] enemies = Physics.OverlapSphere(transform.position, sphereRadius);
            foreach(Collider col in enemies)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(col.gameObject, 600);
                    //knockbacks
                    collision.gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.transform.position - transform.position).normalized * enemySetback, ForceMode.Impulse);
                }
                
            }
        }
    }
}
