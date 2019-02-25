using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupiterPull : MonoBehaviour
{

    // radius of ability
    public int radius = 100;
    DateTime start;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e"))
        {
            start = DateTime.Now;
        }
        if((DateTime.Now - start).TotalSeconds < 1)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.right, 0.0f);
            foreach (RaycastHit hit in hits)
            {
                // Detects if the object is an "enemy" and if so destroys it
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-(hit.collider.gameObject.transform.position - this.gameObject.transform.position).normalized);
                }
            }
        }
    }
}
