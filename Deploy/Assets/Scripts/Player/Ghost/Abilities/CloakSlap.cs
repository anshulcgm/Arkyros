using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloakSlap : MonoBehaviour
{
    public float cooldown = 0;
    public int maxCooldown = 1800;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;


    private bool buffActive;

    private bool cast;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;

    SoundManager soundManager;
    public LayerMask self;
    public CloakCollider cloak;

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
    void FixedUpdate()
    {
        if (Input.GetKey("r") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            start = DateTime.Now;
        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cast = true;
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] /= 3f; //decrease speed

            //Split Windup animation for here
            anim.StartOverlayAnim("CircleSwing", 0.5f, 8f);
            Debug.Log("start");
            soundManager.playOneShot("CloakSlapCharge");
        }
        if (((DateTime.Now - start).TotalSeconds >= 3 && Input.GetKey("r") && cast) || ((DateTime.Now - start).TotalSeconds >= 8 && cast))//earliest release is 3 seconds, max is 7
        {
            cast = false;
            soundManager.stop();
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 3;
            Debug.Log("end");
            anim.StartOverlayAnim("Slap", 0.5f, 1.7f); //this tells the animator to play the right animation
            cloak.setActive(100);
            soundManager.playOneShot("CloakSlapRelease");

            /*
            RaycastHit hit;
            if(Physics.SphereCast(model.transform.position + transform.up * 5, 100, model.transform.forward, out hit, 100f, self))
            {
                Debug.Log("Slapped");
                stats.dealDamage(hit.transform.gameObject, 1000);
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(hit.transform.position - transform.position, ForceMode.Impulse);
            }
            */
            cooldown = maxCooldown;
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
