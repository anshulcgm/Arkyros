using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlyEnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public ShlyEnemy Shly;
    private Rigidbody r;
    private float speed;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<StatManager>().kamikazeMovementSpeed;
        r = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        Shly = this.GameObject;
        anim = GetComponent<Animator>();

        /*foreach (ShlyEnemy e in Enemy.enemyList) {
            ShlyEnemy.shlyList.Add(e); //adds all shlies to shlylist
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform); //Ensures they're always looking at the player
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = this.gameObject.transform.position;
        
        if (ShlyEnemy.getAggregateNum() == 6 || ShlyEnemy.getAggregateNum() == 3)
        {
            //Default attack of 6 or 3 aggregate is bullCharge
        }
        else if (ShlyEnemy.getAggregateNum() < 6)
        {
            //Default attack is pellet drop
        }
        else
        {
            r.velocity = (playerPos - enemyPos).normalized * speed; //move towards the player
        }

        die(); //will only actually die if hp <= 0
    }

    //On Death, shly will die
    public void die() {
        if (Shly.GetComponent<StatManager>().hp <= 0) {
            Destroy(this.gameObject);
        }
    }

   

}
