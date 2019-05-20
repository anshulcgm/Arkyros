using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoMoriProjectile : MonoBehaviour
{
    DateTime start;
    Rigidbody rigidBody;
    GameObject Source;

    GhostSoundManager gsm;
    private int projectileSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
        rigidBody = GetComponent<Rigidbody>();
        gsm = GetComponent<GhostSoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((DateTime.Now - start).TotalSeconds > 3) //turns around after 3 seconds
        {
            //transform.position = Vector3.MoveTowards(transform.position, Source.transform.position, .03f);
            var ray = transform.position - Source.transform.position;

            rigidBody.velocity = ray * projectileSpeed;
        }
        gsm.playMMReturn();
    }

    public void SetSource(GameObject source)
    {
        Source = source;

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //other. take damage

        }
        
        if(other.gameObject == Source)
        {
            Destroy(this.gameObject);
        }
    }


}
