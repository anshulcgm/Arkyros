using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemOfLifeScript : MonoBehaviour
{
    DateTime start;
    DateTime heal;
    int lifetime = 8;
    int radius = 20;
    int healAmount = 20;

    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
        heal = DateTime.Now;

    }

    // Update is called once per frame
    void Update()
    {
        if((DateTime.Now - heal).TotalSeconds >= 1)
        {
            heal = DateTime.Now;
            Collider[] stuff = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in stuff)
            {
                if (c.gameObject.tag == "Player")
                {
                    c.gameObject.GetComponent<Stats>().heal(healAmount);
                }

            }
        }

        if((DateTime.Now - start).TotalSeconds > lifetime)//life time of 8 seconds
        {
            Destroy(this.gameObject);
        }
    }

    public void augment()
    {
        lifetime += 12;
        radius += 10;
        healAmount += 10;
    }
}
