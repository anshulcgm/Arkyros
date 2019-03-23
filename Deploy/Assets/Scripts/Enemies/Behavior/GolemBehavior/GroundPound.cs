using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPound : MonoBehaviour
{

    public float sphereR;

    // Use this for initialization
    void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereR);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.tag == "Player")
            {
                Destroy(col.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
