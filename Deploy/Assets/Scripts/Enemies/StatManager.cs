using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{

    //Monobehavior class that is attached to every enemy gameobject, allows both player and enemy to make changes to all enemy stats

    public float kamikazeMaxHP;
    public float kamikazeDefense;
    public float kamikazeMovementSpeed;
    public float kamikazeIQ;

    public float golemMaxHp;
    public float golemDefense;
    public float golemMovementSpeed;
    public float golemChargeSpeed;
    public float golemProjectileSpeed;
    public float golemKnockbackDmg;
    public float golemGroundPoundDmg;
    public float golemProjectileDmg;

    public float IRMaxHp;
    public float IRDefense;
    public float IRMovementSpeed;
    public float IRRadiusAffect;
    public float IRSpeedBuff;
    public float IRMaxHpBuff;
    public float IRAttackBuff;
    public float IRPlayerAttackDebuff; 
    public float IRPlayerSpeedDebuff;

    private KamikazeEnemy flyingKam;
    private Golem golem;
    public IrradiatedEnemies IREnemy;

    private EnemyType type;
    
    //Start function sets enemytype for the script so that the right variables are changed
    void Start()
    {
        if(gameObject.name == "KamakaziBirdShort(Clone)")
        {
            flyingKam = new KamikazeEnemy(kamikazeMaxHP, (int)kamikazeMovementSpeed, kamikazeDefense, gameObject, (int)kamikazeIQ);
            type = EnemyType.FlyingKamikaze;
            Debug.Log("Instantiated flying kamikaze");
        }
        else if(gameObject.name == "GolemParent(Clone)")
        {
            golem = new Golem(golemMaxHp, (int)golemMovementSpeed, golemDefense, gameObject, golemProjectileSpeed, golemChargeSpeed, golemKnockbackDmg, golemGroundPoundDmg, golemProjectileDmg);
            type = EnemyType.Brawler;
            Debug.Log("Instantiated golem");
        }
        else if(gameObject.name == "IREnemy(Clone)")
        {
            IREnemy = new IrradiatedEnemies(IRMaxHp, (int)IRMovementSpeed, IRDefense, gameObject, IRRadiusAffect, IRSpeedBuff, IRMaxHpBuff, IRAttackBuff, IRPlayerAttackDebuff, IRPlayerSpeedDebuff);
            type = EnemyType.IrradiatedEnemy;
            Debug.Log("Instantiated IR enemy");
        }
        //Change values in Enemy Behavior scripts to align with these values
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //methods for adjusting and debuffing base stats
    public void changeHealth(float amount)
    {
        if(type == EnemyType.FlyingKamikaze)
        {
            flyingKam.enemyStats.updateHealth(amount);
        }
        else if(type == EnemyType.Brawler)
        {
            golem.enemyStats.updateHealth(amount);
        }
        else if(type == EnemyType.IrradiatedEnemy)
        {
            IREnemy.enemyStats.updateHealth(amount);
        }
    }

    //multiplies speed by a constant such as *0.5 
    public void enemyMultiplySpeed(float multiplier)
    {
        if (type == EnemyType.FlyingKamikaze)
        {
            flyingKam.enemyStats.multiplySpeed(multiplier);
        }
        else if (type == EnemyType.Brawler)
        {
            golem.enemyStats.multiplySpeed(multiplier);
        }
        else if (type == EnemyType.IrradiatedEnemy)
        {
            IREnemy.enemyStats.multiplySpeed(multiplier);
        }
    }

    //adds a flat amount to speed, such as +5
    public void enemyFlatSpeed(float amount)
    {
        if (type == EnemyType.FlyingKamikaze)
        {
            flyingKam.enemyStats.flatSpeed(amount);
        }
        else if (type == EnemyType.Brawler)
        {
            golem.enemyStats.flatSpeed(amount);
        }
        else if (type == EnemyType.IrradiatedEnemy)
        {
            IREnemy.enemyStats.flatSpeed(amount);
        }
    }

    public void enemyMultiplyDefense(float multiplier)
    {
        if (type == EnemyType.FlyingKamikaze)
        {
            flyingKam.enemyStats.multiplyDefense(multiplier);
        }
        else if (type == EnemyType.Brawler)
        {
            golem.enemyStats.multiplyDefense(multiplier);
        }
        else if (type == EnemyType.IrradiatedEnemy)
        {
            IREnemy.enemyStats.multiplyDefense(multiplier);
        }
    }

    //In the future we may have methods for adjusting class-specific stats
    public void multiplyGolemDamage(float multiplier)
    {
        golem.setchargeDmg(multiplier);
        golem.setBoulderDmg(multiplier);
        golem.setGroundPoundDmg(multiplier);
    }

    public void modifyKamikazeIQ(int amount)
    {
        flyingKam.flatIQ(amount);
    }
}
