using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldThrowScript : MonoBehaviour
{
    Stats stats;
    DateTime start;

    void Start() {
        start = DateTime.Now;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<StatManager>().changeHealth(-30);
        } 
        Destroy(this.gameObject);

    }

    void Update()
    {
        if ((DateTime.Now - start).TotalSeconds > 4)//bullet lifetime of 4 seconds
        {
            Destroy(this.gameObject);
        }
    }


}
