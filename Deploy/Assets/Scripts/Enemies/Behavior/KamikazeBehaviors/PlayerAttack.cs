using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private GameObject player;
    private Rigidbody r;
    public float speed;
    public float distance;
    public GameObject explosion;

    private Animator anim;

	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = this.gameObject.transform.position;
        if (Vector3.Distance(playerPos, enemyPos) < distance){
            anim.SetTrigger("Dive");
            Debug.Log("Player is at " + playerPos);
            r.velocity = (playerPos - enemyPos).normalized * speed;
        }
        else
        {
            r.velocity = Vector3.zero;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject particleEffect = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
