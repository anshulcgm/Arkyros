using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingPersonnelKid : MonoBehaviour
{
    public float cooldown = 0;

    private GameObject camera;

    private AnimationController anim;


    private bool buffActive;

    Rigidbody rigidbody;
    Stats stats;

    DateTime start;
    TargetCenterScreen tcs;

    private bool cast;

    SoundManager soundManager;
    public GameObject ParticleTrail;
    public GameObject ParticleHit;

    GameObject enemy;
    GameObject clone;
    bool cloneSpawned;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        tcs = GetComponent<TargetCenterScreen>();
        soundManager = GetComponent<SoundManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            cloneSpawned = false;
            start = DateTime.Now;
            //anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            enemy = tcs.getTarget();
            
            //put any setup code here, before the ability is actually cast
            if (!cloneSpawned)
            {
                clone = Instantiate(ParticleTrail, transform.position, transform.rotation);
                cloneSpawned = true;
            }
            //Transform cloneStart = clone.transform;
            clone.transform.position = Vector3.MoveTowards(clone.transform.position, enemy.transform.position,  (clone.transform.position - enemy.transform.position).magnitude);



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast && enemy != null)
        {
            
            if(Vector3.Distance(enemy.transform.position, transform.position) < 200)
            {

                transform.position = enemy.transform.position + enemy.transform.forward;
                Instantiate(ParticleHit, enemy.transform.position, enemy.transform.rotation);

                
                transform.LookAt(enemy.transform);
                camera.transform.LookAt(enemy.transform); //might not spin camera around
                
                
                anim.StartOverlayAnim("Swing_Heavy_1", 0.5f, 1.1f);
                soundManager.playOneShot("NPKTeleport");
                soundManager.playOneShot("NPKVoiceLine");

                stats.dealDamage(enemy, 600);
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
