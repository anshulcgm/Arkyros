using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punishment : MonoBehaviour
{
    public float cooldown;

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
    //might not always be Ghost, need different one for each class.

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<AnimationController>();
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
        if (GetComponent<PlayerScript>().M2Down() && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            



            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

            anim.StartOverlayAnim("Punishment", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration
            soundManager.play("1Punishment");

            Collider[] stuff = Physics.OverlapSphere(model.transform.position + model.transform.forward * 6, 6);
            foreach (Collider c in stuff)
            {
                if (c.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(c.gameObject, 20);

                }

            }



            

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
