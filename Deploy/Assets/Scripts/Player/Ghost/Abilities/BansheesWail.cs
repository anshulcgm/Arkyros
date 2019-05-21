using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheesWail : MonoBehaviour
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

    SoundManager soundManager;
    //might not always be Ghost, need different one for each class.

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
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
            
            soundManager.playOneShot("BansheesWail");

            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 20);
            foreach (Collider hit in hits)
            {
                // Detects if the object is an "enemy" and if so slows it
                if (hit.gameObject.tag == "Enemy")
                {
                    // channel for 1.2 seconds
                    // lower enemy speed by 60% of their base speed
                    // slow lasts 3.5 seconds
                    // animation will be something like a warcry, i can get footage of Merveil doing it
                    // cooldown = 12 seconds???

                    //hit.gameObject.GetComponent<StatManager>().enemyMultiplySpeed(0.4);

                }
            }


            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}

