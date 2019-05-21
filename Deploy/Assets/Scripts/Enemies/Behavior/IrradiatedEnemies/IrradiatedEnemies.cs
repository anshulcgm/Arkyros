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
    public IrradiatedEnemies(float maxhp, int ms, float defense, GameObject referenceObject, float radius, float speedBuff, float hpBuff, float attackBuff, float pAttackDebuff, float pSpeedDebuff) : base(maxhp, ms, defense, referenceObject)
    {
        this.radius = radius;
        this.speedBuff = speedBuff;
        this.hpBuff = hpBuff;
        this.attackBuff = attackBuff;
        this.pAttackDebuff = pAttackDebuff;
        this.pSpeedDebuff = pSpeedDebuff;
    }


    public void spawnEnemy(EnemyType[] types, Dictionary<GameObject, int> enemiesToSpawn, float radius)
    {
        Debug.Log("In spawnEnemy function");
        if (enemyStats.getHealth() <= enemyStats.getMaxHealth() * 0.1)
        {
            //Spawn code for other enemies - Anshul Task 6
            float maxHPProportion = Random.Range(1, 101) * 0.01f;
            int j = 0;
            foreach (KeyValuePair<GameObject, int> kvp in enemiesToSpawn)
            {
                EnemyType typeToPass = types[j];
                for (int i = 0; i < kvp.Value; i++)
                {
                    RandomEnemySpawn.spawnEnemyWithinRadius(typeToPass, kvp.Key, radius, referenceObject.transform.position, maxHPProportion);
                }
                j++;
            }
        }
    }



    public void updateEnemiesAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                //hitColliders[i].gameObject.attack += attackBuff;
                //Will write code for this when we can change specialized values of enemy classes
                hitColliders[i].gameObject.GetComponent<StatManager>().multiplyAttack(attackBuff);
            }
            i++;
        }
    }

    public void updateEnemiesHP()
    {
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                if (hpBuff != 0)
                {
                    //hitColliders[i].gameObject.maxhp *= maxhpBuff;
                    hitColliders[i].gameObject.GetComponent<StatManager>().changeHealth(hpBuff);
                }

            }
            i++;
        }
    }


    public void updateEnemiesSpeed()
    {
        Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                hitColliders[i].gameObject.GetComponent<StatManager>().enemyMultiplySpeed(speedBuff);
            }
            i++;
        }
    }





    public void mutate(int factor)
    {
        //mutation of the irradiation area
        radius = radius * factor;
    }

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

    public float getRadius()
    {
        return radius;
    }

}

