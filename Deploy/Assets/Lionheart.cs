using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lionheart : MonoBehaviour {


    public int radius = 100;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if (GetComponent<PlayerScript>().IsPressed("g"))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.right, 0.0f);
            foreach (RaycastHit hit in hits)
            {
                // Detects if the object is an "ally" and if so increases speed
                if (hit.collider.gameObject.tag == "Ally")
                {
                    // Increase allies' speed by a large amount for a very short time
                }
            }
        }





    }
}
