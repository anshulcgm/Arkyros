using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerTime : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;
    DateTime hammerSwing;
    DateTime firstSwing;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    SoundManager soundManager;
    public GameObject particleSmash;
    bool particleSpawned;
    public GameObject HammerHead;
    bool done;

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
        if (Input.GetKey("f") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            particleSpawned = false;
            done = false;
            


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            hammerSwing = DateTime.Now;
            firstSwing = DateTime.Now;
            soundManager.playOneShot("HammerTime");


            //First Swing, Rest follow in next if bracket
            anim.StartOverlayAnim("Swing_Heavy", 0.5f, 0.8f);
            particleSpawned = false;

            

            cooldown = 600;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }

        if((DateTime.Now - firstSwing).TotalSeconds > 0.8 && cast && !done)
        {
            Collider[] hits = Physics.OverlapSphere(HammerHead.transform.position, 30);

            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(hit.gameObject, 600);
                }
            }

            done = true;
        }

        if ((DateTime.Now - hammerSwing).TotalSeconds > 1 && cast)
        {
            hammerSwing = DateTime.Now;
            anim.StartOverlayAnim("Swing_Heavy", 0.5f, 0.8f);
            particleSpawned = false;

            
            

        }

        if ((DateTime.Now - hammerSwing).TotalSeconds > 0.8 && cast && !particleSpawned)//timed to explode with hammer connection
        {
            Instantiate(particleSmash, HammerHead.transform.position, transform.rotation);
            particleSpawned = true;

            //dmg
            Collider[] hits = Physics.OverlapSphere(HammerHead.transform.position, 60);

            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(hit.gameObject, 600);
                }
            }

            
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
