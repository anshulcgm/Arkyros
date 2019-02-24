using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {
	public GameObject Projectile;
	public float timer = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			GameObject laser = (GameObject)Instantiate(Projectile, this.gameObject.transform.position, this.gameObject.transform.rotation);
			timer = 0.1f;
		}
	}

	
}
