using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlyEnemyBehavior : MonoBehaviour
{
    private GameObject player;
    public ShlyEnemy shly;
    private Rigidbody r;

    public float speed;
    public float bullChargeSpeed;

    public float pelletDropStoppingDistance;
    public float searchRadius;
    public float sprainEffectRadius;

    public GameObject explosion;

    private Animator anim;

    private Rigidbody rb;

    private bool sprainInProgress = false;

    public float pelletTimer;
    private float oPelletTimer;

    public float neutralMovementTimer;
    private float oNeutralMovementTimer;

    public GameObject projectile;

    private Vector3 center;

    public float maxDisToTravel;

    private Vector3 finalPos;

    public float idleBehaviorRadius;
    public float attackRadius;
    // Start is called before the first frame update
    void Start()
    {
        //speed = GetComponent<StatManager>().kamikazeMovementSpeed;
        r = GetComponent<Rigidbody>();
        oNeutralMovementTimer = neutralMovementTimer;
        //player = GameObject.FindGameObjectWithTag("Player");
        //anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        
        /*foreach (ShlyEnemy e in Enemy.enemyList) {
            ShlyEnemy.shlyList.Add(e); //adds all shlies to shlylist
        }*/
        rb = GetComponent<Rigidbody>();
        oPelletTimer = pelletTimer;
        pelletTimer = -0.01f;
        player = GameObject.FindGameObjectWithTag("Center");
        finalPos = Random.insideUnitSphere * maxDisToTravel + transform.position;
        

    }

    // Update is called once per frame
    void Update()
    {
        float disToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(disToPlayer <= idleBehaviorRadius && disToPlayer > attackRadius)
        {
            neutralMovement();
        }
        else if(disToPlayer <= attackRadius)
        {
            center = player.GetComponent<SkinnedMeshRenderer>().bounds.center;
            speed = GetComponent<StatManager>().shly.enemyStats.getSpeed();
            bullChargeSpeed = GetComponent<StatManager>().shly.getBullChargeSpeed();
            transform.LookAt(player.transform);
            if (shly == null)
            {
                shly = GetComponent<StatManager>().shly;
            }
            if (shly.getAggregateNum() > 1)
            {
                if (!shly.sprainActive)
                {
                    bullCharge(center);
                }
            }
            else
            {
                if (!shly.sprainActive)
                {
                    PelletDrop();
                }
            }
            shly.sprain(sprainEffectRadius);
        }
       
    }  
    public void bullCharge(Vector3 target)
    {
        Debug.Log("In bull charge");
        rb.velocity = (target - transform.position).normalized * bullChargeSpeed;
    }

    public void PelletDrop()
    {
        Debug.Log("In pellet drop function");
        if(Vector3.Distance(transform.position, player.transform.position) <= pelletDropStoppingDistance)
        {
            Debug.Log("Starting to shoot pellets");
            pelletTimer -= Time.deltaTime;
            if(pelletTimer < 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                pelletTimer = oPelletTimer;
            }
        }
        else
        {
            Debug.Log("moving towards player");
            float step = Time.deltaTime * speed;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);  
        }
    }

    public void neutralMovement()
    {
        //Debug.Log("In update of neutral movement");
        //within the beginning of Update(), start moving towards final
        rb.velocity = (finalPos - transform.position).normalized * speed;

        //rotation
        Quaternion rotation = Quaternion.LookRotation(finalPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);

        //once destination reached, or "reached", wait for x seconds and find new final
        if (Vector3.Distance(transform.position, finalPos) < 1.0f)
        {

            neutralMovementTimer -= Time.deltaTime; 
            if (neutralMovementTimer >= 0)
            {
                r.velocity = Vector3.zero;
            }
            else
            {
                finalPos = Random.insideUnitSphere * maxDisToTravel + transform.position;
                neutralMovementTimer = oNeutralMovementTimer;
            }

        }
    }
    public void OnCollisionEnter(Collision collision)
    {/*
        Debug.Log("In OnCollisionEnter");
        //Adjust player transform and health here
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with player");
            collision.gameObject.transform.position += rb.velocity * 3.0f;
        }
    */
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Stats>().takeDamage(GetComponent<StatManager>().shly.getBullChargeDamage());
        }
        Instantiate(explosion, collision.GetContact(0).point, Quaternion.identity);
      
    }
}
