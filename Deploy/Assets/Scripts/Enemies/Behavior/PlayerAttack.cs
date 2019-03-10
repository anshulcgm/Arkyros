using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public GameObject player;
    private Rigidbody r;
    public float speed;
    public float distance;

	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = this.gameObject.transform.position;
        if (Vector3.Distance(playerPos, enemyPos) < distance){
            r.velocity = (playerPos - enemyPos).normalized * speed;
        }
        else
        {
            r.velocity = Vector3.zero;
        }
    }
}
