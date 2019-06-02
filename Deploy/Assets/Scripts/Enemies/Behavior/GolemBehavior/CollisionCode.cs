using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCode : MonoBehaviour {

    // Use this for initialization

    public float golemShootDamage;


    public float destroyTimer = 5.0f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        destroyTimer -= Time.deltaTime;
        if(destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Center")
        {
            collision.gameObject.GetComponent<Stats>().takeDamage(golemShootDamage);
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
        Destroy(gameObject);
    }
}
