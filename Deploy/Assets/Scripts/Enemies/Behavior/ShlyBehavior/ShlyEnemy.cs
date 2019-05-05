using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Toma Itagaki
public class ShlyEnemy : Enemy {

    private int aggregateNum; //number of shlies that are aggregate
    private float bullChargeSpeed; //shly speed towards players
	private float speedDebuff; //the proportion of AOE speed debuff when player is under "sprain" effect
    private GameObject referenceObject; //rf

    public static List<ShlyEnemy> shlyList;

    public ShlyEnemy(float hp, int ms, float defense, int aNum, float bCS, float sD, GameObject rf, int intel) : base( hp, ms, defense, rf)
    {
		this.referenceObject = rf;

        shlyList = new List<ShlyEnemy>();

        aggregateNum = aNum; //aggregate shlies come as 12 initially
        bullChargeSpeed = bCS;
        speedDebuff = sD;
    }


    public void pelletDrop(float radius) //basic attack where shly drops pellets
    {
        Vector3 playerPos;

        //checks whether objects within radius x are tagged players
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Player"))
            {
                playerPos = hitColliders[i].gameObject.transform.position;
                /*
                Enter in unity logic of dropping pellet                
                */

            }
            i++;
        }
    }


    public void sprain(float x) //enemies within a radius of x will slow down
    {
        if(aggregateNum >= 6) //must be aggregate shly of six or more
        {
            //checks whether objects within radius x are tagged players
            //if so, player movement speed will get speedDebuff-ed
            Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, x);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].CompareTag("Player"))
                {
                    //Include player debuff
                    //hitColliders[i].gameObject.GetComponent<StatManager>().playerMultiplySpeed(speedDebuff);
                }
                i++;
            }
        }
    }


    public void bullCharge(float radius) //knockback attack when aggregated
    {
        Vector3 playerPos;

        if(aggregateNum >= 6) //must be aggregate shly (one more more)
        {
            //checks whether objects within radius x are tagged players
            //if so, shly will charge at the player and attack
            Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].CompareTag("Player"))
                {
                    playerPos = hitColliders[i].gameObject.transform.position;
                    /*
                    UNity Logic?    
                    
                    */
                    float speed = 1; //arbitrary number
                    float step = speed * Time.deltaTime;

                    referenceObject.transform.position = Vector3.MoveTowards(referenceObject.transform.position, playerPos, step);
                    //above logic will move the aggregate shly towards the player

                    //damage dealt
                    //Include damage dealt for player
                    //hitColliders[i].gameObject.GetComponent<StatManager>().health - gameObject.GetComponent<StatManager>().attack * aggregateNum;
                    //damage dealt by bull charge attack will be the aggregate number times the shlies attack

                    /*
                    Unity logic for player knockback 
                    */
                }
                i++;
            }
        }
    }

    //public accessors
	public int getAggregateNum() {
		return aggregateNum;
	}

	public float getBullChargeSpeed() {
		return bullChargeSpeed;
	}

	public float getSpeedDebuff() {
		return speedDebuff;
	}
}
