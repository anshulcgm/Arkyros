using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBlade : MonoBehaviour
{
    public float cooldown;
    public float maxCooldown = 240;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    private bool buffActive;

    public GameObject SpiritBladeEnergySlash;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;
    TargetCenterScreen tcs;

    private int projectileSpeed = 40;
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
        if (GetComponent<PlayerScript>().M2Down() && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            start = DateTime.Now;
            

            //put any setup code here, before the ability is actually cast
            

        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            model.transform.rotation = camera.transform.rotation;
            anim.StartOverlayAnim("Swing_Light", 0.5f, 1f);
            soundManager.playOneShot("SpiritBlade");

            GameObject clone = Instantiate(SpiritBladeEnergySlash, model.transform.position + model.transform.forward * 5 + transform.up * 6, model.transform.rotation);

            float x = Screen.width / 2f;
            float y = Screen.height / 2f;

            var ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

            clone.GetComponent<Rigidbody>().velocity = ray.direction * projectileSpeed;

            cooldown = maxCooldown;
            cast = true;

        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
