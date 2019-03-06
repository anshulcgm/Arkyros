using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnockbackMove : MonoBehaviour {

    private Rigidbody rb;
    public GameObject enemy;
    public float speed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector3 enemyPos = enemy.transform.position;
            Vector3 direction = (this.transform.position - enemyPos).normalized;
            rb.velocity = direction*speed;
        }
    }
}
