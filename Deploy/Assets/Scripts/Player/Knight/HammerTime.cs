using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerTime : MonoBehaviour
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

    //KnightSoundManager knightSoundManager;
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
            anim.StartOverlayAnim("HammerTime", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration

            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            for (int i = 0; i < 7; i++)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 3/*placeholder position*/, 4/*placeholder radius*/);
                foreach (Collider hit in hits)
                {
                    if (hit.gameObject.tag == "Enemy")
                    {
                        stats.dealDamage(hit.gameObject, 20);
                    }
                }
                int frame_delay = 60; //for 1 second time delay
                if (frame_delay > 0)
                {
                    frame_delay--;
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
