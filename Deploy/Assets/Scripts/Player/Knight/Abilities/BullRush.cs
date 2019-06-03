using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullRush : MonoBehaviour
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

    private int dashNum = 150;
    private int enemySetback = 200;

    bool dashing;

    SoundManager soundManager;
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
        if (Input.GetKey("q") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            anim.StartOverlayAnim("Charge", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration

        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            model.transform.rotation = camera.transform.rotation;
            rigidbody.AddForce(camera.transform.forward * dashNum, ForceMode.Impulse); //dash forward
            dashing = true;
            soundManager.playOneShot("BullRush");


            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }

        if ((DateTime.Now - start).TotalSeconds > 2 && cast)
        {
            dashing = false;
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && dashing)
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(model.transform.forward * enemySetback, ForceMode.Impulse);
            stats.dealDamage(collision.gameObject, 20);
            
        }
    }
}
