using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDown : MonoBehaviour
{
    public float cooldown;
    
    private GameObject camera;
    public GameObject SkyHammer;

    private AnimationController anim;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    //KnightSoundManager knightSoundManager;
    //might not always be Ghost, need different one for each class.

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        tcs = GetComponent<TargetCenterScreen>();

        cooldown = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            anim.StartOverlayAnim("HammerDown", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration
               


            //put any setup code here, before the ability is actually cast
            
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit)) {
                    Vector3 target = hit.point;
                    GameObject skyHammerObj = Instantiate(SkyHammer, target + transform.up * 100, Quaternion.identity);
                    skyHammerObj.GetComponent<Rigidbody>().AddForce(-transform.position.normalized * 50, ForceMode.Impulse);
                }

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
