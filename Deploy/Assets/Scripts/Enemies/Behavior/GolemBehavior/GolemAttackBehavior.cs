using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttackBehavior : MonoBehaviour {

    public float groundPoundRange;
    public float chargeRange;
    public float shootRange;
    public float noActionRange;

    private float chargeSpeed;
    public float groundPoundRadius;

    private GameObject mainCamera;


    private Rigidbody rb;

    private Animator anim;

    private GameObject player;


    public GameObject projectile;
	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        chargeSpeed = GetComponent<StatManager>().golemChargeSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = transform.GetChild(0).GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");


    }
	
	// Update is called once per frame
	void Update () {

        //Logic for running behaviors, ground pound is a close range attack, charge is mid-range attack and projectile launch is far range-attack
        float playerDist = Vector3.Distance(player.transform.position, transform.position);
        if (playerDist < groundPoundRange)
        {
            //mainCamera.GetComponent<cameraSoundManager>().enemyInRange = true;
            groundPound();
        }
        else if (playerDist > groundPoundRange && playerDist < chargeRange)
        {
            //mainCamera.GetComponent<cameraSoundManager>().enemyInRange = true;
            charge();
        }
        else if (playerDist > chargeRange && playerDist < shootRange)
        {
            //mainCamera.GetComponent<cameraSoundManager>().enemyInRange = true;
            setShootTrigger();
        }
        else
        {
            //mainCamera.GetComponent<cameraSoundManager>().enemyInRange = false;
            Debug.Log("Golem is too far from player to enact behavior");
        }
	}

    //Attack Behaviors
    public void groundPound()
    {
        anim.SetTrigger("GroundPound");
        //Debug.Log("GroundPound trigger set");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, groundPoundRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.tag == "Player")
            {
               //Adjust player health here
            }
        }
    }

    public void charge()
    {
        anim.SetTrigger("Charge");
        //Debug.Log("Charge trigger set");
        Vector3 playerPos = player.transform.position;
        Vector3 direction = (playerPos - this.transform.position).normalized;
        rb.velocity = direction * chargeSpeed;
        //Code for player damage
    }

    public void setShootTrigger()
    {
        anim.SetTrigger("Shoot");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    }
}
