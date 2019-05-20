using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartOfDarkness : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private AnimationController anim;

    DateTime start;

    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast = false;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        soundManager = GetComponent<SoundManager>();
        cooldown = 0;
    }


    Vector3 target;
    bool u_gottem = false;
    Vector3 lastPosn = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("r") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = true; //ability not yet cast
            start = DateTime.Now;
            rigidbody.AddForce(transform.position.normalized * 1000);
            soundManager.playOneShot("HeartofDarknessTeleport");
            stats.addBuff((int)buff.Gravityless, 240);
        }
        if (cast)
        {
            if ((DateTime.Now - start).TotalSeconds < 4)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
                    {
                        target = hit.point;
                        u_gottem = true;
                        cast = false;
                        Debug.Log("hhhhhhhhhh");
                    }
                    cooldown = 1000;
                }
                if ((DateTime.Now - start).TotalSeconds > 1)
                {
                    transform.position = lastPosn;
                    return;
                }
                lastPosn = transform.position;
            }
            else
            {
                cooldown = 500;
                cast = false;
            }
        }


        if (u_gottem)
        {
            transform.position = target;
            soundManager.playOneShot("HeartofDarknessDivebomb");
            Collider[] stuff = Physics.OverlapSphere(target, 20);
            foreach (Collider c in stuff)
            {
                stats.dealDamage(c.gameObject, float.MaxValue);
            }
            u_gottem = false;
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}