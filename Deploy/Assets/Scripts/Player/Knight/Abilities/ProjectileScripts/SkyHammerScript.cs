using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyHammerScript : MonoBehaviour
{

    private GameObject camera;

    DateTime start;
    SoundManager soundManager;
    public GameObject particleBoom;
    bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        exploded = false;
        soundManager = GetComponent<SoundManager>();
        start = DateTime.Now;
        
    }

    // Update is called once per frame
    void Update()
    {
        if((DateTime.Now - start).TotalSeconds > 30)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (/*collision.gameObject.tag.Equals("planet") && */ !exploded)
        {
        
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

        soundManager.playOneShot("HammerDown");
        exploded = true;

        }
    }
    void FixedUpdate()
    {
       
    }
}
