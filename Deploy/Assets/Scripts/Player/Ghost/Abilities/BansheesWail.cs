using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheesWail : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private Animator anim;

    DateTime start;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }



    // Update is called once per frame
    void fixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            start = DateTime.Now;
            anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            cooldown = 720;                          //placeholder time, divide by 60 for cooldown in seconds

            Collider[] hits = Physics.OverlapSphere(transform.position, 20);
            foreach (Collider hit in hits)
            {
                // Detects if the object is an "enemy" and if so slows it
                if (hit.gameObject.tag == "Enemy")
                {
                    // channel for 1.2 seconds
                    // lower enemy speed by 60% of their base speed
                    // slow lasts 3.5 seconds
                    // animation will be something like a warcry, i can get footage of Merveil doing it
                    // cooldown = 12 seconds???

                    //hit.gameObject.GetComponent<StatManager>().enemyMultiplySpeed(0.4);

                }
            }
        }

        if (cooldown > 0)
        {
            cooldown--;
        }

    }
}