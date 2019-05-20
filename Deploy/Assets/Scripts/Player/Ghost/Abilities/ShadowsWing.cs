using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsWing : MonoBehaviour
{
    public float cooldown = 0;

    private GameObject camera;

    private AnimationController anim;


    private bool buffActive;

    private bool cast;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        soundManager = GetComponent<SoundManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            start = DateTime.Now;
            //anim.SetBool("ShadowsWing", true); //this tells the animator to play the right animation
            soundManager.play("ShadowsWing");
        }

        if ((DateTime.Now - start).TotalSeconds < 5 && Input.GetKey("e") && !cast)
        {
            
            //put any setup code here, before the ability is actually cast
            //allStats[(int)stats.Defense, (int)statModifier.Multiplier] * 3; //Triple Defense
            cast = true;
            buffActive = true;
        }
        if(((DateTime.Now - start).TotalSeconds > 5) && !Input.GetKey("e") && cast && buffActive)
        {
            cooldown = (float)(DateTime.Now - start).TotalSeconds * 60;
            //allStats[(int)stats.Defense, (int)statModifier.Multiplier] / 3; //return to original Defense
            soundManager.stop();
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}