using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSurf : MonoBehaviour
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
        if (Input.GetKey("q") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = maxCooldown;
            cast = true;
            anim.StartOverlayAnim("Surf", 0.5f, 5f);
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 2; //double speed
            buffActive = true;
            Debug.Log("start");
            soundManager.playOneShot("ShieldSurf");
        }

        if ((DateTime.Now - start).TotalSeconds > 5 && buffActive) //when duration of ability is over, set back to original speed
        {

            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] /= 2; //original speed
            soundManager.stop();
            Debug.Log("end");
            buffActive = false;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
