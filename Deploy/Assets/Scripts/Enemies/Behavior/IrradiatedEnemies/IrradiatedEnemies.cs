using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrradiatedEnemies: Enemy
{

    private float radius;
    private float speedBuff;
    private float hpBuff;
    private float attackBuff;

    private float pAttackDebuff; //Possible Value: .9
    private float pSpeedDebuff; //Possible Value: .9

    //private float reloadBuff;




    //Constructor
    public IrradiatedEnemies(float maxhp, int ms, float defense, GameObject referenceObject, float radius, float speedBuff, float hpBuff, float attackBuff, float pAttackDebuff, float pSpeedDebuff) : base(maxhp, ms, defense,referenceObject)
    {
            this.radius = radius;
            this.speedBuff = speedBuff;
            this.hpBuff = hpBuff;
            this.attackBuff = attackBuff;
            this.pAttackDebuff = pAttackDebuff;
            this.pSpeedDebuff = pSpeedDebuff;
    }


    public void spawnEnemy(Dictionary<GameObject, int> enemiesToSpawn, float radius)
    {
        //The dictionary represents the various enemies that will be spawned and the key is how many of them will be spawned
        if (enemyStats.getHealth() < enemyStats.getMaxHealth() * 0.1)
        {
        //Spawn code for other enemies - Anshul Task 6
        float maxHPProportion = Random.Range(1, 101) * 0.01f; //Randomizes how much health they will have
                
        foreach (KeyValuePair<GameObject, int> kvp in enemiesToSpawn)
            {
                for(int i = 0; i < kvp.Value; i++)
                {
                    RandomEnemySpawn.spawnEnemyWithinRadius(kvp.Key, radius, referenceObject.transform.position, maxHPProportion);
                }
            }
        }
    }

    public void updateMovement()
    {
        if (base.hp < base.maxhp * 0.2)
        { //if IR enemy health is lower than 20% make it move
            ms = 10; //place holder value
        }
        else
        {
            ms = 0;
        } //add clause for when it is far from player
    }


    public void spawnEnemy()
    {
        if (base.hp < base.maxhp * 0.1)
        { //if IR enemy health is lower than 10% spawn other enemies
            //spawn function
        }
    }

    public void updateEnemiesAttack()
    { //determines whether an object is in the radius of irradiated enemy
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            { //if the colliding object is "enemy" tagged
                hitColliders[i].gameObject.attack += attackBuff; //increase attack stat
            }
            i++;
        }
    }

    public void updateEnemiesHP()
    {  //determines whether an object is in the radius of irradiated enemy
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Enemy"))  //if the colliding object is "enemy" tagged
            {
                if (maxhpBuff != 0)
                {
                    hitColliders[i].gameObject.maxhp *= maxhpBuff;  //increase the max health of enemy
                }

            }
            i++;
        }
    }

    public void updateEnemiesReload()
    {  //determines whether an object is in the radius of irradiated enemy
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Enemy")) //if the colliding object is "enemy" tagged
            {
                hitColliders[i].gameObject.reload -= reloadBuff;  //decrease the reload speed
            }
            i++;
        }
    }

    public void updateEnemiesSpeed()
    {  //determines whether an object is in the radius of irradiated enemy
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Enemy")) //if the colliding object is "enemy" tagged
            {
                hitColliders[i].gameObject.speed += speedBuff; //increase speed of enemy
            }
            i++;
        }
    }





    public void mutate(int factor)
    {
        //mutation of the irradiation area
        radius = radius * factor;

        public float geHpBuff()
        {
            return hpBuff;
        }
        public float getSpeedBuff()
        {
            return speedBuff;
        }
        public float getAttackBuff()
        {
            return attackBuff;
        }
    

}

