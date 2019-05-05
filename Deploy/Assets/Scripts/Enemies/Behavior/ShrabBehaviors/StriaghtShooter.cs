using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StriaghtShooter : MonoBehaviour
{
	private Rigidbody rb;
	private GameObject Player;

	public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
		
        rb.velocity = (Player.transform.position - transform.position).normalized * speed;
    }

	void OnCollisionEnter(Collision collision) {
		Destroy(this.gameObject);
	}
}
