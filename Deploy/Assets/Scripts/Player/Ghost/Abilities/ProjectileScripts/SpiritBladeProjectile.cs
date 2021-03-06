﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBladeProjectile : MonoBehaviour
{
    DateTime start;
    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if((DateTime.Now - start).TotalSeconds > 4)//bullet lifetime of 4 seconds
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<StatManager>().changeHealth(-20);


        }
        Destroy(this.gameObject);
    }
}
