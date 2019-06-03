using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleObjScript : MonoBehaviour
{
    DateTime start;
    Stats stats;
    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if ((DateTime.Now - start).TotalSeconds > 4)//bullet lifetime of 4 seconds
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("player"))
        {
            other.gameObject.GetComponent<Stats>().heal(20);//20 is an arbitrary value

        }
        Destroy(this.gameObject);
    }
}
