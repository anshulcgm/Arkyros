using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapintheDark : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 300;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;
    Vector3 target;

    private bool buffActive;
    private bool cast;

    public GameObject LeapDarkParticleEffect;

    SoundManager soundManager;
    public ScytheCollider scythe;
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
        if (GetComponent<PlayerScript>().IsPressed("q") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            GameObject particleEffect = Instantiate(LeapDarkParticleEffect, transform.position, Quaternion.Euler(90, 0, 0));
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = maxCooldown;
            cast = true;
            model.transform.rotation = camera.transform.rotation;
            Vector3 newpos = model.transform.position + model.transform.forward * 100;
            RaycastHit hit;
            if (Physics.Raycast(model.transform.position + transform.up * 8, model.transform.forward, out hit, 100))
            {
                newpos = hit.point;          
            }
            soundManager.playOneShot("LeapInTheDark", 0.5f);
            transform.position = newpos;
            //Instantiate(particleExplosion, transform.position, Quaternion.identity);
            // the attack will be taken care by the animation??
            scythe.setActive(30); 
            anim.StartOverlayAnim("Swing_Heavy_1", 0.5f, 0.5f);
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
