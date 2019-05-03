using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloakSlap : MonoBehaviour
{
    public float cooldown = 0;

    private GameObject camera;

    private Animator anim;

    private bool buffActive;

    private bool cast;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;

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
            cast = false;
            start = DateTime.Now;
            anim.SetBool("CloakSlap", true); //this tells the animator to play the right animation
            //slow down while charging
            //allStats[(int)stats.Speed, (int)statModifier.Multiplier] / 3; //decrease speed
            buffActive = true;
        }
        if ((DateTime.Now - start).TotalSeconds >= 4 && Input.GetKey("e") && !cast)
        {
            //when charged for at least 4 seconds - set cooldown and do release
            cooldown = 600;     //set cooldown, placeholder time
            cast = true;
            //maybe set collider to scythe

            //damage enemy
            //EnemyGameObject.GetComponent<StatManager>().changeHealth(amount);
            //add knockback
            //EnemyGameObject.GetComponent<StatManager>().RigidBody().addForce(amount);
        }
        //when key released and the seconds held less than 4, return to normal speed
        if ((DateTime.Now - start).TotalSeconds <= 4 && !Input.GetKey("e") && buffActive && !cast)
        {
            //allStats[(int)stats.Speed, (int)statModifier.Multiplier] * 3; //return speed
            buffActive = false;
        }
        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
