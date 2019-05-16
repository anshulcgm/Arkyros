using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private GameObject player;
    private Rigidbody r;
    private float speed;
    public float distance;
    public GameObject explosion;

    private Animator anim;

	// Use this for initialization
	void Start () {
        speed = GetComponent<StatManager>().kamikazeMovementSpeed;
        r = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame

	void Update () {
        transform.LookAt(player.transform); //Ensures they're always looking at the player
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = this.gameObject.transform.position;
        if (Vector3.Distance(playerPos, enemyPos) < distance){
            anim.SetTrigger("Dive");
            Debug.Log("Player is at " + playerPos);
            r.velocity = (playerPos - enemyPos).normalized * speed; //Kamikazes move towards player when player is within certain distance
        }
        else
        {
            r.velocity = Vector3.zero;
        }
    }

    //Kamikazes explode when they hit anything but other enemies 
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            GameObject particleEffect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Player")
        {
            //Adjust player health here
        }
		
    }

    //Need a method for Kamikaze movement at random times
}
