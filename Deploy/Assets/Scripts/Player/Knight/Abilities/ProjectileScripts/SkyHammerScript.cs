using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyHammerScript : MonoBehaviour
{

    private GameObject camera;


    SoundManager soundManager;
    public GameObject particleBoom;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("planet")) {
            //soundManager.playOneShot("HammerDown");

            Instantiate(particleBoom, transform.position, transform.rotation);
            Debug.Log("SkyHammer Down");
            Collider[] hits = Physics.OverlapSphere(transform.position, 20);
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    hit.gameObject.GetComponent<StatManager>().changeHealth(600);

                }
            }
        }
    }
    void FixedUpdate() {
       
    }
}
