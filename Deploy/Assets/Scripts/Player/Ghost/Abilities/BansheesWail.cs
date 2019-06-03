using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheesWail : MonoBehaviour
{
    public float cooldown;
    public int maxCooldown = 720;

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

    public GameObject BansheeWailParticleEffect;

    //might not always be Ghost, need different one for each class.

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
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            anim.StartOverlayAnim("Summon_Area", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration
            GameObject particleEffect = Instantiate(BansheeWailParticleEffect, transform.position, Quaternion.identity);
            soundManager.playOneShot("BansheesWail");
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            foreach (Collider col in Physics.OverlapSphere(transform.position, 100))
            {
                if (col.gameObject.tag == "Enemy")
                {
                    col.gameObject.GetComponent<StatManager>().enemyMultiplySpeed(0.5f);
                }
            }


            cooldown = maxCooldown;
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}

