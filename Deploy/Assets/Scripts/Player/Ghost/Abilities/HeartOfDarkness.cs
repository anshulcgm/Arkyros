using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class HeartOfDarkness : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private AnimationController anim;

    DateTime start;
    DateTime lastCast;

    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast = false;

    SoundManager soundManager;
    public GameObject reee;

    float sat;

    Vector3 target;
    bool u_gottem = false;
    bool gottem_r = false;
    Vector3 lastPosn = Vector3.zero;

    public LayerMask self;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        soundManager = GetComponent<SoundManager>();
        cooldown = 0;

        sat = 1;
    }


    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("r") && cooldown == 0 && !gottem_r)      //place key, any key can be pressed.
        {
            gottem_r = true;
            cast = true; //ability not yet cast
            start = DateTime.Now;
            rigidbody.AddForce(transform.position.normalized * 5000);
            soundManager.playOneShot("HeartofDarknessTeleport");
            anim.PlayLoopingAnim("Fly_Forward");
            stats.setBuffDuration((int)buff.Gravityless, 240);
            
        }
        if (cast)
        {
            
            sat = Mathf.Max(1 - (float)(DateTime.Now - start).TotalSeconds, 0);
            lastCast = DateTime.Now;

            camera.GetComponent<ColorCorrectionCurves>().saturation = sat;
            if ((DateTime.Now - start).TotalSeconds < 4)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, self))
                    {
                        target = hit.point;
                        u_gottem = true;
                        cast = false;
                        
                    }
                    cooldown = 300;
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
                cooldown = 300;
                cast = false;
                
            }
        }
        else
        {
            sat = Mathf.Min((float)(DateTime.Now - lastCast).TotalSeconds, 1);
            camera.GetComponent<ColorCorrectionCurves>().saturation = sat;
            gottem_r = false;
        }


        if (u_gottem)
        {
            stats.setBuffDuration((int)buff.Gravityless, 0);
            Debug.Log("HoD");

            rigidbody.isKinematic = true;
            transform.position = target;
            Instantiate(reee, transform.position, Quaternion.identity);
            soundManager.playOneShot("HeartofDarknessDivebomb");

            rigidbody.isKinematic = false;
            u_gottem = false;
            Collider[] stuff = Physics.OverlapSphere(target, 20);
            foreach (Collider c in stuff)
            {
                if(c.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(c.gameObject, 1000);
                }
                
            }
            
            
            
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}