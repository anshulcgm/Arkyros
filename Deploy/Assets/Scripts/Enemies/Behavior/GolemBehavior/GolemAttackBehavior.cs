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

    private float gravity = -10f;

    public GameObject projectile;

    //Spherical Movement fields
    private float speed;
    private GameObject planet;

    public float maxIdleDistance;

    public float idleTimer;
    private float oIdleTimer;

    Vector3 targetPoint;

    private bool searchForNewIdlePoint = true;

    public static GameObject[] playerCenterlist;
	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        chargeSpeed = 50;
        speed = 20;
        planet = GameObject.FindGameObjectWithTag("planet");
        //player = GameObject.FindGameObjectWithTag("PlayerCenter");
        anim = transform.GetChild(0).GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //playerCenterlist = GameObject.FindGameObjectsWithTag("PlayerCenter");
        oIdleTimer = idleTimer;
        targetPoint = randomPointInRadius();
    }
	
	// Update is called once per frame
	void Update () {
        player = RandomEnemySpawn.getClosestPlayer(transform, RandomEnemySpawn.playerCenterList).gameObject;
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
        else if(playerDist > shootRange && playerDist < noActionRange)
        {
            //mainCamera.GetComponent<cameraSoundManager>().enemyInRange = false;
            Debug.Log("Golem is too far from player to enact behavior");
            idleTimer -= Time.deltaTime;
            if(idleTimer <= 0)
            {
                targetPoint = randomPointInRadius();
                searchForNewIdlePoint = false;
                idleTimer = oIdleTimer;
            }
            if (!searchForNewIdlePoint)
            {
                sphericalMovement(targetPoint, speed);
            }
            if(Vector3.Distance(transform.position, targetPoint) <= 0.8f)
            {
                rb.velocity = Vector3.zero;
                searchForNewIdlePoint = true; 
            }
        }
	}

    //Attack Behaviors
    public void groundPound()
    {
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        orientEnemy(player.transform.position);
        //sphericalMovement(player.transform.position, speed);
        Debug.Log("In groundPound");
        anim.SetTrigger("GroundPound");
        //Debug.Log("GroundPound trigger set");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, groundPoundRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.tag == "Player")
            {
                col.gameObject.GetComponent<Stats>().takeDamage(GetComponent<StatManager>().golem.getGroundPoundDmg());
            }
        }
        /*
        if (Vector3.Distance(player.transform.position, transform.position) <= 20f)
        {
            while (Vector3.Distance(player.transform.position, transform.position) <= 30f)
            {
                rb.velocity = (-(findRaycastPointOnSphere(player.transform.position) - transform.position).normalized * 45f);
            }

        }
        */
    }

    public void charge()
    {
        rb.freezeRotation = false; 
        Debug.Log("In charge function");
        anim.SetTrigger("Charge");
        //Debug.Log("Charge trigger set");
        sphericalMovement(player.transform.position, chargeSpeed);
        //Code for player damage
    }

    public void setShootTrigger()
    {
        Debug.Log("Trying to shoot");
        anim.SetTrigger("Shoot");
        sphericalMovement(player.transform.position, speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerCenter")
        {
            collision.gameObject.GetComponent<Stats>().takeDamage(GetComponent<StatManager>().golem.getChargeDmg());
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
    public void sphericalMovement(Vector3 target, float speed)
    {
        orientEnemy(target);
        target = findRaycastPointOnSphere(target);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.position.normalized * 10.0f, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
        {
            Plane2 alignPlane = new Plane2(hit.normal, transform.position);
            if(hit.transform.gameObject.tag != "Player")
            {
                transform.position = hit.point + hit.point.normalized * 0.75f;
            }
           
            //Vector2 mappedPoint2 = alignPlane.GetMappedPoint(player.transform.position) - alignPlane.GetMappedPoint(transform.position);
            //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed);
            //if (Vector3.Distance(hit.point, transform.position) >= 1f)
            //{
              //  rb.AddForce(transform.position.normalized * gravity);
                //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed/-2f);
            //}
        }
        //adding force towards gravity, adding force towards direction faced
        //rb.AddForce(transform.forward * speed);
        //rb.velocity = (findRaycastPointOnSphere(target) - transform.position).normalized * speed;
        Debug.Log("Distance between target and position is " + Vector3.Distance(target, transform.position));
        
       
        //else
        //{
            rb.velocity = (target - transform.position).normalized * speed;
        //}
        
        //rb.AddForce(transform.position.normalized * gravity);
    }
    public void orientEnemy(Vector3 target)
    {
        Plane2 plane = new Plane2(transform.position.normalized, transform.position);
        Vector2 mappedPoint = plane.GetMappedPoint(target) - plane.GetMappedPoint(transform.position);
        Vector3 mappedPoint3D = mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir;
        Debug.Log("Mapped point magnitude is " + mappedPoint.magnitude);
        if (mappedPoint.magnitude > 1)
            transform.LookAt(mappedPoint3D + transform.position, transform.position.normalized);
    }
    public Vector3 findRaycastPointOnSphere(Vector3 point)
    {
        RaycastHit hit;
        Vector3 pointToReturn = Vector3.zero;
        if (Physics.Raycast(point + point.normalized * 5.0f, (-point).normalized, out hit, Mathf.Infinity))
        {
            pointToReturn = hit.point;
        }
        return pointToReturn;
    }
    public Vector3 randomPointInRadius()
    {
        //Code to find random point on planet within radius
        Vector3 initialPoint = Random.insideUnitSphere * maxIdleDistance + transform.position + new Vector3(0f, 30f, 0f);
        Vector3 targetPoint = Vector3.zero;
        RaycastHit hit; 
        if(Physics.Raycast(initialPoint, -initialPoint.normalized, out hit, Mathf.Infinity))
        {
            targetPoint = hit.point;
        }
        return targetPoint;
    }
}
