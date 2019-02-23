using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour {
	private Rigidbody rb;
	private GameObject Player;
	private GameObject Enemy;

	public float speed;
	// Use this for initialization
	void Start () {
		Enemy = GameObject.Find("Enemy");
		Player = GameObject.Find("Player");
		Debug.Log("Player Location = " + Player.transform.position);
		Debug.Log("Enemy Location = " + Enemy.transform.position);
		rb = GetComponent<Rigidbody>();
		rb.velocity = (Player.transform.position - Enemy.transform.position).normalized * speed;
		Debug.Log("Direction = " +  (Player.transform.position - Enemy.transform.position).normalized);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		Destroy(this.gameObject);
	}
}
