using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour
{
    public float cooldown = 0;

    private GameObject camera;

    private Animator anim;

    Rigidbody rigidbody;
    Stats stats;

    //DateTime start;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            //start = DateTime.Now;
            anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds



            /*
             * All the code for the ability that you want to write
             * transform.forward for the direction the player is 
             * maybe setting colliders
             * instantiating new objects
             * to damage enemy, EnemyGameObject.GetComponent<StatManager>().changeHealth(amount), amount can be positive or negative
             */
        }






        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
