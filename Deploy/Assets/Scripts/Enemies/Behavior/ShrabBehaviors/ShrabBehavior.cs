using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrabBehavior : MonoBehaviour
{
    public float shurikenRange;
    //public List<Shrab> shrabsInRange;
    public float pincerMovementRange;

    public GameObject pincer_R_Top;

    public GameObject player;
    public GameObject projectile; 

    public float shrabMovementSpeed; 

    public GameObject leftPlayerLeg;
    public GameObject rightPlayerLeg;

    private Animator anim;

    public float timer; //How often should the shrab shoot shurikens
    private float variableTimer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); 
        this.shurikenRange = 15;
        //this.shrabsInRange = new List<Shrab>();
        this.pincerMovementRange = 10;

        variableTimer = timer; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        pincerMovement();
        if(Vector3.Distance(transform.position, player.transform.position) > 3.0f)
        {
            waterShurikenAttack();
        }
        /*
        foreach(Shrab s in Shrab.shrabList)
        {
            if(Vector3.Distance(s.referenceObject.transform.position, transform.position) < pincerMovementRange)
            {
                shrabsInRange.Add(s);
            }
        }
        int numShrabs = 0;
        foreach(Shrab s in shrabsInRange)
        {
            numShrabs += s.getNumShrabs();
        }
        if(numShrabs >= 12)
        {
            pincerMovement();
        }
        */
    }

    public void pincerMovement()
    {
        if (Vector3.Distance(transform.position, leftPlayerLeg.transform.position) < Vector3.Distance(transform.position, rightPlayerLeg.transform.position))
        {
            Debug.Log("going for right leg");
            if (Vector3.Distance(transform.position, leftPlayerLeg.transform.position) >= 0.5f)
            {
                anim.SetBool("Moving", true);
                anim.SetBool("Attack", false);
                float step = shrabMovementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, leftPlayerLeg.transform.position, step);
            }
            else
            {
                anim.SetBool("Moving", false);
                anim.SetBool("Attack", true);
            }
        }
        else
        {
            Debug.Log("going for left leg");
            if (Vector3.Distance(transform.position, rightPlayerLeg.transform.position) >= 0.5f)
            {
                anim.SetBool("Moving", true);
                anim.SetBool("Attack", false);
                float step = shrabMovementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, rightPlayerLeg.transform.position, step);
            }
            else
            {
                anim.SetBool("Moving", false);
                anim.SetBool("Attack", true);
                //Adjust player health here
            }
        }
    }

    public void waterShurikenAttack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject laser = (GameObject)Instantiate(projectile, pincer_R_Top.transform.position, Quaternion.identity);
            timer = variableTimer;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
}
