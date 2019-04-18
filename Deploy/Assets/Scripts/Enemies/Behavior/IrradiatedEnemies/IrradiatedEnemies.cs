using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrradiatedEnemies: Enemy
{

        private float radius;
        private float speedBuff;
        private float hpBuff;
        private float attackBuff;
    //private float reloadBuff;

 
    

        //Constructor
        public IrradiatedEnemies(float maxhp, int ms, float defense, GameObject referenceObject, float radius, float speedBuff, float hpBuff, float attackBuff) : base(maxhp, ms, defense,referenceObject)
        {
            this.radius = radius;
            this.speedBuff = speedBuff;
            this.hpBuff = hpBuff;
            this.attackBuff = attackBuff;
       
        }


        public void spawnEnemy(Dictionary<GameObject, int> enemiesToSpawn, float radius)
        {
            if (enemyStats.getHealth() < enemyStats.getMaxHealth() * 0.1)
            {
               foreach(KeyValuePair<GameObject, int> kvp in enemiesToSpawn)
               {
                   for(int i = 0; i < kvp.Value; i++)
                   {
                       RandomEnemySpawn.spawnEnemyWithinRadius(kvp.Key, radius, referenceObject.transform.position);
                   }
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
    

}

