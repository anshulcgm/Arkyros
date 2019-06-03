using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBanner : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 1080;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;
    public GameObject Banner;

    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    SoundManager soundManager;

    public GameObject WarBannerParticleEffect;

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
        if (GetComponent<PlayerScript>().IsPressed("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            GameObject particleEffect = Instantiate(WarBannerParticleEffect, transform.position, Quaternion.Euler(90, 0, 0));
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = maxCooldown;
            cast = true;
            soundManager.playOneShot("WarBanner");
            anim.StartOverlayAnim("BannerPlant", 0.5f, 1f);
            model.transform.rotation = camera.transform.rotation;
            Instantiate(Banner, model.transform.position + model.transform.forward * 15, Quaternion.identity);
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
