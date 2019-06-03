using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldThrow : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 240;

    private GameObject camera;
    public GameObject Shield;
    private float projectileSpeed;

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

        projectileSpeed = 80; //placeholder value 
        cooldown = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1) && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            model.transform.rotation = camera.transform.rotation;
            anim.StartOverlayAnim("ShieldThrow", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration
            soundManager.playOneShot("ShieldThrow");
            GameObject shieldObj = Instantiate(Shield, model.transform.position + model.transform.forward * 12 + transform.up * 8, Quaternion.identity);

            float x = Screen.width / 2f;
            float y = Screen.height / 2f;

            var ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

            shieldObj.GetComponent<Rigidbody>().velocity = ray.direction * projectileSpeed;

            cooldown = maxCooldown;
            cast = true;
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
