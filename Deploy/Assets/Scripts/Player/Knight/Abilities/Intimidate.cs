using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : MonoBehaviour
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
            anim.StartOverlayAnim("AnimationName", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration

            //or

            anim.PlayLoopingAnim("AnimationName"); // mostly only for movement, probably not used in an ability

            soundManager.playOneShot("Intimidate");
            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {

            foreach (Collider col in Physics.OverlapSphere(transform.position, 20))
            {
                if(col.gameObject.tag == "Enemy")
                {
                    col.gameObject.GetComponent<StatManager>().enemyMultiplyDefense(0.75f);
                    col.gameObject.GetComponent<StatManager>().enemyMultiplySpeed(0.75f);
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
