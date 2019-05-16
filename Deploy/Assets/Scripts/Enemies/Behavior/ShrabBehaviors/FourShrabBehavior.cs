using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourShrabBehavior : MonoBehaviour
{
    private GameObject shrab1;
    private GameObject shrab2;
    private GameObject shrab3;
    private GameObject shrab4;

    private Animator animShrab1;
    private Animator animShrab2;
    private Animator animShrab3;
    private Animator animShrab4;

    public float shurikenRange;
    //public List<Shrab> shrabsInRange;
    public float pincerMovementRange;

    public GameObject shrab1pincer_R_Top;
    public GameObject shrab2pincer_R_Top;
    public GameObject shrab3pincer_R_Top;
    public GameObject shrab4pincer_R_Top; 

    private GameObject player;
    public GameObject projectile;

    private float shrabMovementSpeed = 0f;

    public float timer; //How often should the shrab shoot shurikens
    private float variableTimer;

    void Start()
    {
        shrab1 = transform.GetChild(0).gameObject;
        shrab2 = transform.GetChild(1).gameObject;
        shrab3 = transform.GetChild(2).gameObject;
        shrab4 = transform.GetChild(3).gameObject;

        animShrab1 = shrab1.GetComponent<Animator>();
        animShrab2 = shrab2.GetComponent<Animator>();
        animShrab3 = shrab3.GetComponent<Animator>();
        animShrab4 = shrab4.GetComponent<Animator>();

        variableTimer = timer;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        shrabMovementSpeed = GetComponent<StatManager>().shrabMovementSpeed;
        pincerMovement();
        if(Vector3.Distance(transform.position, player.transform.position) >= 2.5f)
        {
            waterShurikenAttack(); 
        }
    }

    public void pincerMovement()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= 1.7f)
        {
            setAllAnims("Moving", true);
            setAllAnims("Attack", false);
            float step = shrabMovementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            Debug.Log("Moving towards player");
        }
        else
        {
            setAllAnims("Moving", false);
            setAllAnims("Attack", true);
            Debug.Log("Attacking player");
            //Adjust player health here
        }
     
    }

    public void waterShurikenAttack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Instantiate(projectile, shrab1pincer_R_Top.transform.position, Quaternion.identity);
            Instantiate(projectile, shrab2pincer_R_Top.transform.position, Quaternion.identity);
            Instantiate(projectile, shrab3pincer_R_Top.transform.position, Quaternion.identity);
            Instantiate(projectile, shrab4pincer_R_Top.transform.position, Quaternion.identity);

            timer = variableTimer;
        }
    }

    public void setAllAnims(string trigger, bool value)
    {
        
        animShrab1.SetBool(trigger, value);
        animShrab2.SetBool(trigger, value);
        animShrab3.SetBool(trigger, value);
        animShrab4.SetBool(trigger, value);
        Debug.Log("Setting " + trigger + " to " + value);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
}
