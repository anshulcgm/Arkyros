using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 600;

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

    public GameObject IntimidateParticleEffect;

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
            GameObject particleEffect = Instantiate(IntimidateParticleEffect, transform.position, Quaternion.Euler(90, 0, 0));
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = maxCooldown;
            cast = true;

            anim.StartOverlayAnim("Intimidate", 0.5f, 1f); 
            soundManager.playOneShot("Intimidate");
            foreach (Collider col in Physics.OverlapSphere(transform.position, 20))
            {
                if(col.gameObject.tag == "Enemy")
                {
                    col.gameObject.GetComponent<StatManager>().enemyMultiplyDefense(0.75f);
                    col.gameObject.GetComponent<StatManager>().enemyMultiplySpeed(0.75f);
                }
            }
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
