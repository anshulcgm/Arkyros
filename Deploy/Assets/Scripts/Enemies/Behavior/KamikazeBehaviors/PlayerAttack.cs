﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private GameObject player;
    private Rigidbody r;
    private float speed;
    public float distance;
    public GameObject explosion;

    private Animator anim;

    private float oTime;
    public float maxDis;
    public float timer;
    private Vector3 final;

    private float diveSpeed;

    //public static GameObject[] playerList;
    //Use dive speed from Kamikaze IQ logic
	// Use this for initialization
	void Start () {
        speed = GetComponent<StatManager>().kamikazeMovementSpeed;
        r = GetComponent<Rigidbody>();
       // playerList = GameObject.FindGameObjectsWithTag("Player");
        //player = GameObject.FindGameObjectWithTag("Player");    
        anim = GetComponent<Animator>();
        oTime = timer;
        final = Random.insideUnitSphere * maxDis + transform.position;


    }
	
	// Update is called once per frame
    
	void Update () {

        player = RandomEnemySpawn.getClosestPlayer(transform, RandomEnemySpawn.playerList).gameObject;
        diveSpeed = GetComponent<StatManager>().flyingKam.getDiveSpeed();
        transform.LookAt(player.transform); //Ensures they're always looking at the player
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = transform.position;
        //Debug.Log("Player position is " + playerPos);
        //Debug.Log("Distance between player and kamikaze is " + Vector3.Distance(playerPos, enemyPos));
        if (Vector3.Distance(playerPos, enemyPos) < distance){
            anim.SetTrigger("Dive");
            Debug.Log("Player is at " + playerPos);
            r.velocity = (playerPos - enemyPos).normalized * diveSpeed; //Kamikazes move towards player when player is within certain distance
        }
        else
        {
            neutralMovement();
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
            collision.gameObject.GetComponent<Stats>().takeDamage(GetComponent<StatManager>().flyingKam.getDiveSpeed() * 2.0f);
        }
		
    }

    public void neutralMovement()
    {
        //Debug.Log("In update of neutral movement");
        //within the beginning of Update(), start moving towards final
        r.velocity = (final - transform.position).normalized * speed;

        //rotation
        Quaternion rotation = Quaternion.LookRotation(final - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);

        //once destination reached, or "reached", wait for x seconds and find new final
        if (Vector3.Distance(transform.position, final) < 1.0f)
        {

            timer -= Time.deltaTime;
            if (timer >= 0)
            {
                r.velocity = Vector3.zero;
            }
            else
            {
                final = Random.insideUnitSphere * maxDis + transform.position;
                timer = oTime;
            }

        }
    }

    //Need a method for Kamikaze movement at random times
}
