using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{

    //Monobehavior class that is attached to every enemy gameobject, allows both player and enemy to make changes to all enemy stats
    //fields for Kamikaze
    public float kamikazeMaxHP;
    public float kamikazeDefense;
    public float kamikazeMovementSpeed;
    public float kamikazeIQ;

    //fields for Golem
    public float golemMaxHp;
    public float golemDefense;
    public float golemMovementSpeed;
    public float golemChargeSpeed;
    public float golemProjectileSpeed;
    public float golemKnockbackDmg;
    public float golemGroundPoundDmg;
    public float golemProjectileDmg;

    //fields for IR tower
    public float IRMaxHp;
    public float IRDefense;
    public float IRMovementSpeed;
    public float IRRadiusAffect;
    public float IRSpeedBuff;
    public float IRMaxHpBuff;
    public float IRAttackBuff;
    public float IRPlayerAttackDebuff; 
    public float IRPlayerSpeedDebuff;

    //fields for Shly
    public float shlyMaxHp;
    public float shlyDefense;
    public float shlyMovementSpeed;
    public float aggregateNumberofShlies;
    public float bullChargeSpeed;
    public float speedDebuffProportion;
    public float pelletDamage;
    public float bullChargeDamage;

    private KamikazeEnemy flyingKam;
    private Golem golem;
    public IrradiatedEnemies IREnemy;
    public ShlyEnemy shly; 

    private EnemyType type;
    
    //Start function sets enemytype for the script so that the right variables are changed
    void Start()
    {
        if(gameObject.name == "KamakaziBirdShort(Clone)")
        {
            flyingKam = new KamikazeEnemy(kamikazeMaxHP, (int)kamikazeMovementSpeed, kamikazeDefense, gameObject, (int)kamikazeIQ);
            Enemy.enemyList.Add(flyingKam);
            type = EnemyType.FlyingKamikaze;
            Debug.Log("Instantiated flying kamikaze");
        }
        else if(gameObject.name == "GolemParent(Clone)")
        {
            golem = new Golem(golemMaxHp, (int)golemMovementSpeed, golemDefense, gameObject, golemProjectileSpeed, golemChargeSpeed, golemKnockbackDmg, golemGroundPoundDmg, golemProjectileDmg);
            Enemy.enemyList.Add(golem);
            type = EnemyType.Brawler;
            Debug.Log("Instantiated golem");
        }
        else if(gameObject.name == "IREnemy(Clone)")
        {
            IREnemy = new IrradiatedEnemies(IRMaxHp, (int)IRMovementSpeed, IRDefense, gameObject, IRRadiusAffect, IRSpeedBuff, IRMaxHpBuff, IRAttackBuff, IRPlayerAttackDebuff, IRPlayerSpeedDebuff);
            Enemy.enemyList.Add(IREnemy);
            type = EnemyType.IrradiatedEnemy;
            Debug.Log("Instantiated IR enemy");
        }
        else if(gameObject.name == "ShlyEnemy(Clone)")
        {
            shly = new ShlyEnemy(shlyMaxHp, (int)shlyMovementSpeed, shlyDefense, (int)aggregateNumberofShlies, bullChargeSpeed, speedDebuffProportion, this.gameObject, pelletDamage, bullChargeDamage);
            Enemy.enemyList.Add(shly);
            ShlyEnemy.shlyList.Add(shly);
            type = EnemyType.Shly;
            Debug.Log("Instantiated shly object");
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
        else if(type == EnemyType.Shly)
        {
            shly.enemyStats.updateHealth(amount);
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
        else if(type == EnemyType.Shly)
        {
            shly.enemyStats.multiplySpeed(multiplier);
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
        else if(type == EnemyType.Shly)
        {
            shly.enemyStats.flatSpeed(amount);
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
        else if (type == EnemyType.Shly)
        {
            shly.enemyStats.multiplyDefense(multiplier);
        }
    }

    public void multiplyAttack(float multiplier)
    {
        if(type == EnemyType.Brawler)
        {
            golem.setchargeDmg(multiplier);
            golem.setBoulderDmg(multiplier);
            golem.setGroundPoundDmg(multiplier);
        }
        else if(type == EnemyType.FlyingKamikaze)
        {
            flyingKam.flatIQ((int)(multiplier * 10f));
        }
        else if(type == EnemyType.Shly)
        {
            shly.adjustDamage(multiplier);
        }
    }
    public void modifyKamikazeIQ(int amount)
    {
        flyingKam.flatIQ(amount);
    }
}
