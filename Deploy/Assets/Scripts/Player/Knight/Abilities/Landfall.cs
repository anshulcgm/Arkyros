using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landfall : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private AnimationController anim;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    private int numForward = 700;
    private int numUp = 400;
    private int sphereRadius = 20;
    private int enemySetback = 400;


    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
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
            anim.StartOverlayAnim("Landfall", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration

            //or

            anim.PlayLoopingAnim("Landfall"); // mostly only for movement, probably not used in an ability


            //put any setup code here, before the ability is actually cast
            soundManager.playOneShot("LandfallJump");



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {

            soundManager.play("LandfallSustain");
            GetComponent<Rigidbody>().AddForce(transform.forward * numForward);
            GetComponent<Rigidbody>().AddForce(transform.up * numUp);

            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("planet") && cast) //lands back on the ground
        {
            soundManager.stop();
            soundManager.playOneShot("LandfallFall");
            Collider[] enemies = Physics.OverlapSphere(transform.position, sphereRadius);
            foreach(Collider col in enemies)
            {
                //collision.gameObject.GetComponent<StatManager>().changeHealth(20);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * -enemySetback);
            }
        }
    }
}
