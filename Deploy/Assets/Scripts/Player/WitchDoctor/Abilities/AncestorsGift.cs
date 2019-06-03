using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncestorsGift : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    private bool buffActive;
    GameObject target;


    Rigidbody rigidbody;
    Stats stats;

    DateTime start;
    TargetCenterScreen tcs;

    private bool cast;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<AnimationController>();
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
        if (Input.GetMouseButton(1) && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false;
            start = DateTime.Now;
            target = tcs.getAlly();


            //put any setup code here, before the ability is actually cast


        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            cooldown = 240;                        
            cast = true;
            if (Vector3.Distance(target.transform.position, transform.position) < 200)
            {
                model.transform.LookAt(target.transform);
                anim.StartOverlayAnim("AncestorGift", 0.5f, 1f);
                soundManager.playOneShot("1AncestorsGift");
                target.GetComponent<Stats>().heal(20);
            }
            
            //maybe particle here
            /*
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag.Equals("Player"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var player = hit.collider.gameObject;
                        player.GetComponent<Stats>().heal(player.GetComponent<Stats>().maxHealth * .06f);
                        var delay = 60;
                        if (delay > 0)
                        {
                            delay--;

                        }
                    }
                }





            }
            */

            
        }

        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
