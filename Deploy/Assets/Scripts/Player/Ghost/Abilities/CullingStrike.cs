using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingStrike : MonoBehaviour
{
    public float cooldown;
    public int maxCooldown = 240;

    private GameObject camera;

    public AnimationController anim;

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
        if (Input.GetMouseButton(1) && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {            
            scythe.setActive(40);
            anim.StartOverlayAnim("Culling_Strike", 0.5f, 1f); //this tells the animator to play the right animation
            soundManager.playOneShot("CullingStrike");

            cooldown = maxCooldown;
            cast = true;
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}

