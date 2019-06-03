using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldsUp : MonoBehaviour
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
    public GameObject shieldTemp;

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
        if (Input.GetMouseButtonDown(1) && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            

            


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            anim.StartOverlayAnim("Stance", 0.5f, 4f); // mostly only for movement, probably not used in an ability
            Instantiate(shieldTemp, transform.position + transform.forward * 4, transform.rotation);


            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
