using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToCollisionHandler : MonoBehaviour
{
    CollisionHandler c;

    // Start is called before the first frame update
    void Start()
    {
        c = GameObject.FindGameObjectWithTag("CollisionHandler").GetComponent<CollisionHandler>();
    }

    bool sentTransform = false;
    // Update is called once per frame
    void Update()
    {
        if(!sentTransform && CollisionHandler.initialized){
            c.AddTransform(transform);
            sentTransform = true;
        }
    }
}
