using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlyEnemyBehavior : MonoBehaviour
{
    private GameObject player;
    public ShlyEnemy shly;
    private Rigidbody r;
    public float speed;

    public float pelletDropStoppingDistance;
    public float searchRadius;
    public float sprainEffectRadius;

    private Animator anim;

    private Rigidbody rb;

    private bool sprainInProgress = false;

    public float pelletTimer;
    private float oPelletTimer;

    public GameObject projectile; 
    // Start is called before the first frame update
    void Start()
    {
        //speed = GetComponent<StatManager>().kamikazeMovementSpeed;
        r = GetComponent<Rigidbody>();
        //player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        
        /*foreach (ShlyEnemy e in Enemy.enemyList) {
            ShlyEnemy.shlyList.Add(e); //adds all shlies to shlylist
        }*/
        rb = GetComponent<Rigidbody>();
        oPelletTimer = pelletTimer;
        pelletTimer = -0.01f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        if(shly == null)
        {
            shly = GetComponent<StatManager>().shly;
        }
        if (shly.getAggregateNum() > 1)
        {
            if (!shly.sprainActive)
            {
                bullCharge(player.transform.position);
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
    public void bullCharge(Vector3 target)
    {
        Debug.Log("In bull charge");
        float step = Time.deltaTime * speed;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
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
    
}
