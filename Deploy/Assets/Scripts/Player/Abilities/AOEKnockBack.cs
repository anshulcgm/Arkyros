using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEKnockBack : MonoBehaviour
    
{  
    // radius of ability
    public int radius = 100;

    void Start()
    {
        //Debug.Log("radius is " + radius);
    }

    void Update()
    {
        if (Input.GetKey("e")){
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.right, 0.0f);
            foreach (RaycastHit hit in hits)
            {
                // Detects if the object is an "enemy" and if so destroys it
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
