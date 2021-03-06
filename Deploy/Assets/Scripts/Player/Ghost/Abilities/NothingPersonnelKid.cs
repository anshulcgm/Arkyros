﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingPersonnelKid : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 600;

    private GameObject camera;
    
    public AnimationController anim;
    public GameObject model;

    private bool buffActive;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;
    TargetCenterScreen tcs;

    private bool cast;

    SoundManager soundManager;
    //public GameObject ParticleTrail;
    public GameObject ParticleHit;


    GameObject enemy;
    GameObject clone;
    bool cloneSpawned;
    bool particleSpawned;
    bool voiceLinePlayed;
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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<PlayerScript>().IsPressed("f") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            cloneSpawned = false;
            particleSpawned = false;
            voiceLinePlayed = false;
            start = DateTime.Now;
            //anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            enemy = tcs.getTarget();
            
            //put any setup code here, before the ability is actually cast
            if (!cloneSpawned)
            {
                //clone = Instantiate(ParticleTrail, transform.position, transform.rotation);
                cloneSpawned = true;
            }
            //Transform cloneStart = clone.transform;
            //clone.transform.position = Vector3.MoveTowards(clone.transform.position, enemy.transform.position,  (clone.transform.position - enemy.transform.position).magnitude);



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast && enemy != null)
        {
            
            if(Vector3.Distance(enemy.transform.position, transform.position) < 200)
            {

                transform.position = enemy.transform.position - enemy.transform.forward * 3;

                if (!particleSpawned)
                {
                    Instantiate(ParticleHit, enemy.transform.position, enemy.transform.rotation);
                    particleSpawned = true;
                }
                

                
                model.transform.LookAt(enemy.transform);
                camera.transform.LookAt(enemy.transform); //might not spin camera around
                
                
                anim.StartOverlayAnim("Swing_Heavy_1", 0.5f, 0.5f);
                scythe.setActive(120);
                if (!voiceLinePlayed)
                {
                    soundManager.playOneShot("NPKTeleport");
                    soundManager.playOneShot("NPKVoiceLine");
                    voiceLinePlayed = true;
                }               

                //stats.dealDamage(enemy, 600);
                Debug.Log("REEEEEEE");
            }
            cooldown = maxCooldown;
            cast = true;
        }
        
        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
