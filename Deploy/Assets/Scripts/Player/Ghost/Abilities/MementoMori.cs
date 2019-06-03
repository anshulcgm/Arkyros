using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoMori : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 480;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;

    public GameObject MementoMoriProjectile;
    public GameObject self;

    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;
    private int projectileSpeed = 30;

    SoundManager soundManager;
    //might not always be Ghost, need different one for each class.

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
        self = gameObject;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("f") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            //anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            soundManager.playOneShot("MementoMoriThrow");
            anim.StartOverlayAnim("Swing_Light", 0.5f, 1f);
            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            model.transform.rotation = camera.transform.rotation;
            GameObject clone = Instantiate(MementoMoriProjectile, model.transform.position + model.transform.forward * 5 + transform.up * 6, Quaternion.Euler(90,90,90));

            float x = Screen.width / 2f;
            float y = Screen.height / 2f;

            var ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

            clone.GetComponent<Rigidbody>().velocity = ray.direction * projectileSpeed;
            clone.GetComponent<MementoMoriProjectile>().SetSource(self);


            cooldown = maxCooldown;
            cast = true;

        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
