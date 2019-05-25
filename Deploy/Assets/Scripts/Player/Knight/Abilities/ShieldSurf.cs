using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSurf : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private AnimationController anim;
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
        anim = GetComponent<AnimationController>();
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
            anim.StartOverlayAnim("ShieldSurf", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration

            //or

            anim.PlayLoopingAnim("ShieldSurf"); // mostly only for movement, probably not used in an ability


            //put any setup code here, before the ability is actually cast
            buffActive = true;
            cast = true;


        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 2;

            if (buffActive) {
                stats.allStats[(int)stat.Defense, (int)statModifier.Multiplier] *= 2;
                buffActive = false;
            }
       

            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
