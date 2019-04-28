using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBlade : MonoBehaviour
{
    public float cooldown = 0;

    private GameObject camera;

    private Animator anim;

    private bool buffActive;

    public GameObject SpiritBladeEnergySlash;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;
    TargetCenterScreen tcs;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        tcs = GetComponent<TargetCenterScreen>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            start = DateTime.Now;
            anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds

            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1)
        {
            Instantiate(SpiritBladeEnergySlash, transform.position + transform.forward, Quaternion.identity);


        }






        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
