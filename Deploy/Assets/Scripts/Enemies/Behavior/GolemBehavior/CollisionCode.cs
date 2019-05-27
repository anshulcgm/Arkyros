using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCode : MonoBehaviour {

    // Use this for initialization

    public float golemShootDamage;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Stats>().takeDamage(golemShootDamage);
        }
    }
}
