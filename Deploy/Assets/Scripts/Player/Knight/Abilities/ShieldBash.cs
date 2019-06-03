using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBash : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 240;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;
    public ShieldCollider shield;

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
        if (Input.GetMouseButtonDown(1) && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            anim.StartOverlayAnim("ShieldBash", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration
            soundManager.playOneShot("ShieldBash");
            
            
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = maxCooldown;
            cast = true;

            anim.StartOverlayAnim("ShieldBash", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration
            soundManager.playOneShot("ShieldBash");
            shield.setActive(20);
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
