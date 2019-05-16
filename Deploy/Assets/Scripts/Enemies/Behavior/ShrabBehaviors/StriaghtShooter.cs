using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StriaghtShooter : MonoBehaviour
{

	private Rigidbody rb;
	private GameObject Player;
	private Vector3 current;
	private float oTime;

	public float speed;
	public float timer;
	



    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

		rb = GetComponent<Rigidbody>();
		current = Player.transform.position - transform.position;
		oTime = timer;

        rb = GetComponent<Rigidbody>();
        current = Player.transform.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {

		rb.velocity = (current).normalized * speed;

		timer -= Time.deltaTime;

		if (timer < 0) {
			Destroy(this.gameObject);
		}

        rb.velocity = (current).normalized * speed;
    

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "Player")
        {
            //Adjust player health here
        }
        
    }
}
