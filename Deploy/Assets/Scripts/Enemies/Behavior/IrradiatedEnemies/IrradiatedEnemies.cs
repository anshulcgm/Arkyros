using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrradiatedEnemies: Enemy
{

        private float radius;
        private float speedBuff;
        private float maxhpBuff;
        private float attackBuff;
        private float reloadBuff;

        
    

        //Constructor
        public IrradiatedEnemies(float maxhp, int ms, float defense, GameObject referenceObject, float radius, float speedBuff, float maxhpBuff, float attackBuff, float reloadBuff) : base(maxhp, ms, defense,referenceObject)
        {
            this.radius = radius;
            this.speedBuff = speedBuff;
            this.maxhpBuff = maxhpBuff;
            this.attackBuff = attackBuff;
            this.reloadBuff = reloadBuff;
        }


        public void spawnEnemy()
        {
            if (enemyStats.getHealth() < enemyStats.getMaxHealth() * 0.1)
            {
              //Spawn code for other enemies 
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
                    if (maxhpBuff != 0)
                    {
                        //hitColliders[i].gameObject.maxhp *= maxhpBuff;
                    }

                }
                i++;
            }
        }

        public void updateEnemiesReload()
        {
            Collider[] hitColliders = Physics.OverlapSphere(referenceObject.transform.position, radius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].CompareTag("Enemy"))
                {
                    //hitColliders[i].gameObject.reload -= reloadBuff;
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
                    //hitColliders[i].gameObject.speed += speedBuff;
                }
                i++;
            }
        }





        public void mutate(int factor)
        {
            //mutation of the irradiation area
            radius = radius * factor;
        }

        public float getMaxHpBuff()
        {
            return maxhpBuff;
        }
        public float getSpeedBuff()
        {
            return speedBuff;
        }
        public float getAttackBuff()
        {
            return attackBuff;
        }
        public float getReloadBuff()
        {
            return reloadBuff;
        }

}

