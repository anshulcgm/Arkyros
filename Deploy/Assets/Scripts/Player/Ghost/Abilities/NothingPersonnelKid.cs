using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingPersonnelKid : MonoBehaviour
{
    public float cooldown = 0;

    private GameObject camera;

    private AnimationController anim;


    private bool buffActive;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;
    TargetCenterScreen tcs;

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            start = DateTime.Now;
            //anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            

            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            if(Vector3.Distance(tcs.target.transform.position, transform.position) < 20)
            {
                transform.position = Vector3.Lerp(transform.position, tcs.target.transform.position, 1);

                anim.StartOverlayAnim("Swing_Circle_1", 0.5f, 1f);
                soundManager.playOneShot("NPKTeleport");
                soundManager.playOneShot("NPKVoiceLine");
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
