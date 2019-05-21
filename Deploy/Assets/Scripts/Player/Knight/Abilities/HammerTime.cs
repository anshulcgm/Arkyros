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
    DateTime hammerSwing;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    SoundManager soundManager;

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
            hammerSwing = DateTime.Now;

            soundManager.play("HammerTime");

            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            
            if((DateTime.Now - hammerSwing).TotalSeconds > 1)
            {
                hammerSwing = DateTime.Now;
                anim.StartOverlayAnim("Swing_Heavy", 0.5f, 1f);
                Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 3/*placeholder position*/, 4/*placeholder radius*/);
                foreach (Collider hit in hits)
                {
                    if (hit.gameObject.tag == "Enemy")
                    {
                        stats.dealDamage(hit.gameObject, 20);
                    }
                }

            }
            
            
            cooldown = 600;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }

        if((DateTime.Now - start).TotalSeconds > 4)
        {
            soundManager.stop();
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
