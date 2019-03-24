using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupiterPull : MonoBehaviour
{

    // radius of ability
    public float radius;
    public float speed;
    public float cooldown;



    private Animator anim;

    DateTime start;
    //public GameObject damageDealt;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("e") && cooldown == 0)
        {
            start = DateTime.Now;
            anim.SetTrigger("JupiterPull");
            cooldown = 240; // divide by 60 for cooldown in seconds

            Collider[] hits = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hit in hits)
            {
                // Detects if the object is an "enemy" and if so destroys it
                if (hit.gameObject.tag == "Enemy")
                {
                    hit.gameObject.GetComponent<Rigidbody>().velocity = new Vector3 (0,0,0);

                }
            }
        }

        if((DateTime.Now - start).TotalSeconds < 1)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius);
            Debug.Log("Gottem");
            foreach (Collider hit in hits)
            {
                // Detects if the object is an "enemy" and if so destroys it
                if (hit.gameObject.tag == "Enemy")
                {
                    Debug.Log(hit.gameObject.name);

                    hit.gameObject.GetComponent<Rigidbody>().AddForce(-speed * (hit.gameObject.transform.position - this.gameObject.transform.position).normalized);

                    //Instantiate(damageDealt, hit.gameObject.transform.position, Quaternion.identity);
                }
            }
        }

        if(cooldown > 0)
        {
            cooldown--;
        }
    }
}
