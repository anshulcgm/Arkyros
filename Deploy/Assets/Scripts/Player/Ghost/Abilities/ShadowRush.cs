using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRush: MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private AnimationController anim;


    private bool buffActive;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            start = DateTime.Now;
            //anim.SetBool("ShadowRush", true); //this tells the animator to play the right animation
            cooldown = 360;                   //placeholder time, divide by 60 for cooldown in seconds
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 2; //double speed
            buffActive = true;
            soundManager.play("ShadowRush");
        }
        if ((DateTime.Now - start).TotalSeconds > 6) //when duration of ability is over, set back to original speed
        {
            if (buffActive)
            {
                stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] /= 2; //original speed
                soundManager.stop();
                buffActive = false;
            }
        }
        if (cooldown > 0)
        {
            cooldown--;
        }
    }
}