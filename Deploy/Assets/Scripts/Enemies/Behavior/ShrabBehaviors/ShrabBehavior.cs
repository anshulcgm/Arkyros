using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrabBehavior : MonoBehaviour
{
    public float shurikenRange;
    public List<Shrab> shrabsInRange;
    public float pincerMovementRange;
    public GameObject player;

    public float shrabMovementSpeed; 

    public GameObject leftPlayerLeg;
    public GameObject rightPlayerLeg;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); 
        this.shurikenRange = 15;
        this.shrabsInRange = new List<Shrab>();
        this.pincerMovementRange = 10;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        pincerMovement();
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

                anim.SetBool("Attack", false);
                float step = shrabMovementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, leftPlayerLeg.transform.position, step);
            }
            else
            {
                anim.SetBool("Attack", true);
            }
        }
        else
        {
            Debug.Log("going for left leg");
            if (Vector3.Distance(transform.position, rightPlayerLeg.transform.position) >= 0.5f)
            {
                anim.SetBool("Attack", false);
                float step = shrabMovementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, rightPlayerLeg.transform.position, step);
            }
            else
            {
                anim.SetBool("Attack", true);
            }
        }
    }

    public void waterShurikenAttack()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(GetComponent<SphereCollider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
}
