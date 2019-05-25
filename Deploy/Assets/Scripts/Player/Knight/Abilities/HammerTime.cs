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
    public GameObject particleSmash;
    bool particleSpawned;

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
        if (Input.GetKey("r") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            particleSpawned = false;
            


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            hammerSwing = DateTime.Now;
            soundManager.play("HammerTime");


            //First Swing, Rest follow in next if bracket
            anim.StartOverlayAnim("Swing_Heavy", 0.5f, 0.5f);
            particleSpawned = false;

            //dmg
            Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 8/*placeholder position*/, 4/*placeholder radius*/);

            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(hit.gameObject, 20);
                }
            }

            cooldown = 600;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }

        if ((DateTime.Now - hammerSwing).TotalSeconds > 1 && cast)
        {
            hammerSwing = DateTime.Now;
            anim.StartOverlayAnim("Swing_Heavy", 0.5f, 0.5f);
            particleSpawned = false;

            //dmg
            Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 20/*placeholder position*/, 4/*placeholder radius*/);
            
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(hit.gameObject, 20);
                }
            }

        }

        if ((DateTime.Now - hammerSwing).TotalSeconds > 0.7 && cast && !particleSpawned)//timed to explode with hammer connection
        {
            Instantiate(particleSmash, transform.position + transform.forward * 20, transform.rotation);
            particleSpawned = true;
        }



        if ((DateTime.Now - start).TotalSeconds > 7)
        {
            cast = false;
            soundManager.stop();
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
