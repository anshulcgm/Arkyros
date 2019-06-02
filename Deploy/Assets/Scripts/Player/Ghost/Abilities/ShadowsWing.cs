using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsWing : MonoBehaviour
{
    public float cooldown = 0;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;


    private bool buffActive;

    private bool cast;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<AnimationController>();
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
            anim.StartOverlayAnim("CircleSwing", 0.5f, 5f); //puts cloak up
            soundManager.play("ShadowsWing");
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            
            stats.allStats[(int)stat.Defense, (int)statModifier.Multiplier] *= 3; //Triple Defense
            Debug.Log("up");
            
            buffActive = true;

            cast = true;
            cooldown = 360;
        }
        if((DateTime.Now - start).TotalSeconds > 5 /*&& !Input.GetKey("e")*/ && cast && buffActive)
        {
            Debug.Log("down");
            //cooldown = (int)(DateTime.Now - start).TotalSeconds * 60;
            
            stats.allStats[(int)stat.Defense, (int)statModifier.Multiplier] /= 3; //return to original Defense
            soundManager.stop();
            buffActive = false;
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}