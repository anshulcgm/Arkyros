using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsOfTheDamned : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 480;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    public GameObject SoulsOfTheDamnedProjectile;

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
            anim.StartOverlayAnim("Summon_Area", 0.5f, 1f);


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            int distance = 8; //spawns orbs in an square around you
            Instantiate(SoulsOfTheDamnedProjectile, transform.position + distance * transform.right + distance * transform.forward + transform.up * distance, transform.rotation);
            Instantiate(SoulsOfTheDamnedProjectile, transform.position + distance * transform.right - distance * transform.forward + transform.up * distance, transform.rotation);
            Instantiate(SoulsOfTheDamnedProjectile, transform.position - distance * transform.right + distance * transform.forward + transform.up * distance, transform.rotation);
            Instantiate(SoulsOfTheDamnedProjectile, transform.position - distance * transform.right - distance * transform.forward + transform.up * distance, transform.rotation);



            cooldown = maxCooldown;
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
