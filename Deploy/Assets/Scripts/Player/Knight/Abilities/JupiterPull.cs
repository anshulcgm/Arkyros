using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupiterPull : MonoBehaviour
{

    // radius of ability
    public float radius;
    public float speed;
    public float cooldown;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;
    //public GameObject damageDealt;
    private bool cast;
    SoundManager soundManager;
    Stats stats;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        soundManager = GetComponent<SoundManager>();
        stats = GetComponent<Stats>();
        speed = 200;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("r") && cooldown == 0)
        {
            cast = false;
            start = DateTime.Now;
            //anim.SetBool("JupiterPullBool", true);
            soundManager.playOneShot("JPCharge");

            Collider[] hits = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hit in hits)
            {
                // Detects if the object is an "enemy" and if so destroys it
                if (hit.gameObject.tag == "Enemy")
                {
                    hit.gameObject.GetComponent<Rigidbody>().velocity = new Vector3 (0,0,0);

                }
            }
        }

        if((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            anim.StartOverlayAnim("Pull", 0.5f, 1f);
            Collider[] hits = Physics.OverlapSphere(transform.position, 400);
            Debug.Log("Gottem");
            foreach (Collider hit in hits)
            {
                // Detects if the object is an "enemy" and if so pulls it
                if (hit.gameObject.tag == "Enemy")
                {
                    Debug.Log(hit.gameObject.name);
                    stats.dealDamage(hit.gameObject, 600);
                    hit.gameObject.GetComponent<Rigidbody>().AddForce(-speed * (hit.gameObject.transform.position - this.gameObject.transform.position).normalized, ForceMode.Impulse);

                    soundManager.playOneShot("JPGravity");
                }
            }

            cooldown = 240; // divide by 60 for cooldown in second
            cast = true;
        }



        if (cooldown > 0)
        {
            cooldown--;
        }
    }
}
