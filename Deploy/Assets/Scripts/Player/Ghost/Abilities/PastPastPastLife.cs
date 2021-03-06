﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastPastPastLife : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 1800;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    SoundManager soundManager;

    public GameObject P3LParticleEffect;

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
        if (GetComponent<PlayerScript>().IsPressed("r") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            GameObject particleEffect = Instantiate(P3LParticleEffect, transform.position, Quaternion.Euler(-90, 0, 0));


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = maxCooldown;
            cast = true;

            anim.StartOverlayAnim("Swing_Circle", 0.5f, 2f);
            soundManager.playOneShot("P3LSprout");
            soundManager.play("P3LHeal"); //duration

            int initdmg = 80;
            int heal = 80;
            int radius = 150;

            Collider[] Colliders = Physics.OverlapSphere(transform.position, radius);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].tag == "Enemy")
                {
                    stats.dealDamage(Colliders[i].gameObject, 20);

                }
                else if (Colliders[i].tag == "Allies")
                {
                    Colliders[i].gameObject.GetComponent<Stats>().heal(20);
                    //heal them

                }   
            }
            
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
