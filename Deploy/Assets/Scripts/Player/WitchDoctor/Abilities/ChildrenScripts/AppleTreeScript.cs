using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AppleTreeScript : MonoBehaviour
{
    DateTime start;
    public GameObject Apple;
    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
        
    
 
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((DateTime.Now - start).TotalSeconds > 5)
        {
            Destroy(this.gameObject);
        }
        else {

            GameObject appleObj = Instantiate(Apple, transform.position, Quaternion.identity);//-1 is placeholder value, meant for tree height
            var cooldown = 60;
            if (cooldown > 0) {
                cooldown--;
            }
        }
        
    }
}
