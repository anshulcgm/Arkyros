using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpiritSummons : MonoBehaviour
{
    public float cooldown;
    public int maxCooldown = 600;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;
    SoundManager soundManager;

    private bool buffActive;
    private bool cast;

    public GameObject SpiritBombParticleEffect;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
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
        if (Input.GetKey("f") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            Debug.Log("here");
            GameObject particleEffect = Instantiate(SpiritBombParticleEffect, new Vector3(transform.position.x, transform.position.y + 4.5f, transform.position.z), Quaternion.Euler(0, 0, 0));



            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cast = true;
            cooldown = 240;
            anim.StartOverlayAnim("Summon_Area", 0.5f, 1f);
            soundManager.playOneShot("SpiritSummons");
            
            int radius = 50;
            Collider[] Colliders = Physics.OverlapSphere(transform.position, radius);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].tag == "Enemy")
                {
                    stats.dealDamage(Colliders[i].gameObject, 40);
                }
            }
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}