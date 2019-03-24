using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttackBehavior : MonoBehaviour {

    public float groundPoundRange;
    public float chargeRange;
    public float shootRange;
    public float noActionRange;

    public float chargeSpeed;
    public float groundPoundRadius;

  


    private Rigidbody rb;

    private Animator anim;

    private GameObject player;


    public GameObject projectile;
	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = transform.GetChild(0).GetComponent<Animator>();
     
        //projectileReloadTime = (2.49600f - 0.7072f)/(0.5f * projectilesPerBatch); //Reload time at which it makes sense to shoot based on the animation
        
        //transform.LookAt(player.transform);


    }
	
	// Update is called once per frame
	void Update () {

        Plane2 plane = new Plane2(transform.up, transform.position);
        Vector2 mappedPoint = plane.GetMappedPoint(player.transform.position) - plane.GetMappedPoint(transform.position);
        if (mappedPoint.magnitude < 10f)
        {
            Debug.Log("Magnitiude tiny");
            return;
        }
        //Logic for running behaviors
        float playerDist = Vector3.Distance(player.transform.position, transform.position);
        if (playerDist < groundPoundRange)
        {
            groundPound();
        }
        //else if (playerDist > groundPoundRange && playerDist < chargeRange)
        //{
        //    //charge();
        //}
        else if (playerDist > chargeRange && playerDist < shootRange)
        {
            setShootTrigger();
        }
        else
        {
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
               //Insert player damage code here
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
