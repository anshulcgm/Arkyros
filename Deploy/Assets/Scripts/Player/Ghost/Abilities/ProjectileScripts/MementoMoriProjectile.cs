using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoMoriProjectile : MonoBehaviour
{
    DateTime start;
    Rigidbody rigidBody;
    GameObject Source;
    Vector3 startPoint;

    SoundManager soundManager;
    private int projectileSpeed = 1;
    bool comingBack;

    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
        rigidBody = GetComponent<Rigidbody>();
        soundManager = GetComponent<SoundManager>();
        startPoint = transform.position;
        comingBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = 
        if ((DateTime.Now - start).TotalSeconds > 3) //turns around after 3 seconds
        {
            if (!comingBack)
            {
                rigidBody.velocity = Vector3.zero;
                soundManager.playOneShot("MementoMoriReturn");
                comingBack = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, Source.transform.position + Source.transform.up * 5, 1f);
            //var ray = transform.position - startPoint;
            //rigidBody.velocity = Vector3.zero;
            //rigidBody.velocity = ray * projectileSpeed;
            
            
        }
        
    }

    public void SetSource(GameObject source)
    {
        Source = source;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
            other.gameObject.GetComponent<StatManager>().changeHealth(-30);
        }
        
        else if(other.gameObject == Source)
        {
            Destroy(this.gameObject);
        }
    }


}
