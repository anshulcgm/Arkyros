using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRush: MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 300;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    private bool buffActive;

    Rigidbody rigidbody;
    Stats stats;
    private bool cast;

    DateTime start;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("q") && cooldown == 0)      //place key, any key can be pressed.
        {
            start = DateTime.Now;
            cast = false;
            
        }

        if((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {

            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 2; //double speed
            stats.buffs[(int)buff.Invisible] += 360;
            buffActive = true;
            Debug.Log("start");
            soundManager.playOneShot("Shadowrush");

            cooldown = maxCooldown;
            cast = true;
        }

        if ((DateTime.Now - start).TotalSeconds > 6 && buffActive) //when duration of ability is over, set back to original speed
        {

            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] /= 2; //original speed
            soundManager.stop();
            Debug.Log("end");
            buffActive = false;

        }
        if (cooldown > 0)
        {
            cooldown--;
        }
    }
}