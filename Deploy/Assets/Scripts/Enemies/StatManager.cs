using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{

    //Monobehavior class that is attached to every enemy gameobject, allows both player and enemy to make changes to all enemy stats
    //fields for Kamikaze
    public string enemyName; 

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

    //fields for Shrab
    public float shrabMaxHp;
    public float shrabDefense;
    public float shrabMovementSpeed;
    public int aggregateNumberofShrabs;
    public float shrabPincerDmg;
    public float shrabEruptionRadius;

    //Fields for Starships
    public float starshipMaxHp;
    public float starshipDefense;
    public float starshipMovementSpeed;
    public float IRHealthSpawnStart;
    public float bombDamage;
    public float bombRadius;
    public float rayDamage;
    public float rayWidth;
    public float turretDamage;
   
    public KamikazeEnemy flyingKam;
    public Golem golem;
    public IrradiatedEnemies IREnemy;
    public ShlyEnemy shly;
    public Shrab shrab;
    public Starship ship;

    private EnemyType type;

    public GameObject brokenKamikaze;
    public GameObject brokenGolem;
    public GameObject brokenShly;
    public GameObject brokenShrab;

    private Enemy defaultEnemy;
    private GameObject defaultBroken;

    public float healthTimer = 5.0f;

    //private Enemy defaultEnemy;
    //Start function sets enemytype for the script so that the right variables are changed
    void Start()
    {
        if(enemyName == "Kamikaze") { 
            flyingKam = new KamikazeEnemy(kamikazeMaxHP, (int)kamikazeMovementSpeed, kamikazeDefense, gameObject, (int)kamikazeIQ);
            Enemy.enemyList.Add(flyingKam);
            type = EnemyType.FlyingKamikaze;
            defaultEnemy = flyingKam;
            defaultBroken = brokenKamikaze;
            Debug.Log("Instantiated flying kamikaze");
        }
        else if(enemyName == "Golem")
        {
            golem = new Golem(golemMaxHp, (int)golemMovementSpeed, golemDefense, gameObject, golemProjectileSpeed, golemChargeSpeed, golemKnockbackDmg, golemGroundPoundDmg, golemProjectileDmg);
            Enemy.enemyList.Add(golem);
            type = EnemyType.Brawler;
            defaultEnemy = golem;
            defaultBroken = brokenGolem;
            Debug.Log("Instantiated golem");
        }
        else if(enemyName == "IRTower")
        {
            IREnemy = new IrradiatedEnemies(IRMaxHp, (int)IRMovementSpeed, IRDefense, gameObject, IRRadiusAffect, IRSpeedBuff, IRMaxHpBuff, IRAttackBuff, IRPlayerAttackDebuff, IRPlayerSpeedDebuff);
            Enemy.enemyList.Add(IREnemy);
            type = EnemyType.IrradiatedEnemy;
            defaultEnemy = IREnemy;
            defaultBroken = null;
            Debug.Log("Instantiated IR enemy");
        }
        else if(enemyName == "Shly")
        {
            shly = new ShlyEnemy(shlyMaxHp, (int)shlyMovementSpeed, shlyDefense, (int)aggregateNumberofShlies, bullChargeSpeed, speedDebuffProportion, this.gameObject, pelletDamage, bullChargeDamage);
            Enemy.enemyList.Add(shly);
            ShlyEnemy.shlyList.Add(shly);
            type = EnemyType.Shly;
            defaultEnemy = shly;
            defaultBroken = brokenShly;
            Debug.Log("Instantiated shly object");
        }
        else if(enemyName == "Shrab")
        {
            shrab = new Shrab(shrabMaxHp, (int)shrabMovementSpeed, shrabDefense, gameObject, shrabPincerDmg, shrabEruptionRadius, aggregateNumberofShrabs);
            Enemy.enemyList.Add(shrab);
            Shrab.shrabList.Add(shrab);
            type = EnemyType.Shrab;
            defaultEnemy = shrab;
            defaultBroken = brokenShrab;
            Debug.Log("Instantiated shrab object");
        }
        else if(enemyName == "Starship")
        {
            ship = new Starship(starshipMaxHp, (int)starshipMovementSpeed, starshipDefense, gameObject, IRHealthSpawnStart, bombDamage, bombRadius, rayDamage, rayWidth, turretDamage);
            Enemy.enemyList.Add(ship);
            type = EnemyType.Starship;
            defaultEnemy = ship;
            defaultBroken = null;
        }
        //Change values in Enemy Behavior scripts to align with these values
    }

    // Update is called once per frame
    void Update()
    {
        /*
        healthTimer -= Time.deltaTime; 
        if(healthTimer <= 0)
        {
            changeHealth(-600);
        }
        */
        if(defaultEnemy.enemyStats.getHealth() <= 0 && (type != EnemyType.IrradiatedEnemy || type != EnemyType.Starship))
        {
            Instantiate(defaultBroken, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
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
        else if(type == EnemyType.Shrab)
        {
            shrab.enemyStats.updateHealth(amount);
        }
        else if(type == EnemyType.Starship)
        {
            ship.enemyStats.updateHealth(amount);
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
        else if(type == EnemyType.Shrab)
        {
            shrab.enemyStats.multiplySpeed(multiplier);
        }
        else if(type == EnemyType.Starship)
        {
            ship.enemyStats.multiplySpeed(multiplier);
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
        else if(type == EnemyType.Shrab)
        {
            shrab.enemyStats.flatSpeed(amount);
        }
        else if(type == EnemyType.Starship)
        {
            ship.enemyStats.flatSpeed(amount);
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
        else if(type == EnemyType.Shrab)
        {
            shrab.enemyStats.multiplyDefense(multiplier);
        }
        else if(type == EnemyType.Starship)
        {
            ship.enemyStats.multiplyDefense(multiplier);
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
        else if(type == EnemyType.Shrab)
        {
            shrab.setchargeDmg(multiplier);
        }
        else if(type == EnemyType.Starship)
        {
            ship.bombDamage *= multiplier;
            ship.turretDamage *= multiplier;
            ship.rayDamage *= multiplier;
        }
    }
    public void modifyKamikazeIQ(int amount)
    {
        flyingKam.flatIQ(amount);
    }
}
