using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncestorsCurse : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    public GameObject AppleTree;

    private bool buffActive;
    private bool cast;

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            anim.StartOverlayAnim("AncestorsCurse", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            soundManager.playOneShot("4AncestorsCurse");
            anim.StartOverlayAnim("AncestorsCurse", 0.5f, 1.0f);

            DateTime abilityStart = DateTime.Now;
            while ((DateTime.Now - abilityStart).TotalSeconds < 9)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, 20);//20 is a placeholder radius
                foreach (Collider hit in hits)
                {
                    if (hit.gameObject.tag.Equals("Enemy"))
                    {
                        var enemy = hit.gameObject;
                        enemy.GetComponent<StatManager>().enemyMultiplySpeed(.8f);
                    }
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
