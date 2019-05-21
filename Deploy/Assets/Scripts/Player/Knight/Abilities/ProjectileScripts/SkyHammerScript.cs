using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyHammerScript : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;
    public GameObject SkyHammer;

    private AnimationController anim;
    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("planet")) {
            //soundManager.playOneShot("HammerDown");
            Collider[] hits = Physics.OverlapSphere(transform.position, 20);
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    stats.dealDamage(hit.gameObject, 20);

                }
            }
        }
    }
    void FixedUpdate() {
       
    }
}
