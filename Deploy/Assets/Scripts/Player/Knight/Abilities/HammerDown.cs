using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDown : MonoBehaviour
{
    public float cooldown;
    
    private GameObject camera;
    public GameObject SkyHammer;

    public AnimationController anim;
    public GameObject model;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    SoundManager soundManager;
    public LayerMask layerMask;
    bool called;

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
        if (Input.GetKey("f") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;

        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            called = true;
            anim.StartOverlayAnim("BuffActivation", 0.5f, 1f);
            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }

        if((DateTime.Now - start).TotalSeconds > 0.6 && called)
        {
            called = false;
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 200f, layerMask))
            {
                Vector3 target = hit.point;
                GameObject skyHammerObj = Instantiate(SkyHammer, target + transform.up * 100, transform.rotation);
                skyHammerObj.GetComponent<Rigidbody>().AddForce(-transform.position.normalized * 150, ForceMode.Impulse);
            }

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
