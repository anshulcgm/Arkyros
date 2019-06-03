using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleave : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 240;

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
    public ScytheCollider scythe;
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
        if (GetComponent<PlayerScript>().M2Down() && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            scythe.setActive(24);
            anim.StartOverlayAnim("Circle_strike_2", 0.5f, 1f); //this tells the animator to play the right animation
            soundManager.playOneShot("Cleave");
            Debug.Log("Cleave");

            cooldown = maxCooldown;
            cast = true;
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
