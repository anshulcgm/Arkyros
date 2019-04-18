using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public float kamikazeMaxHP;
    public float kamikazeDefense;
    public float kamikazeMovementSpeed;
    public float kamikazeIQ;

    public float golemMaxHp;
    public float golemDefense;
    public float golemMovementSpeed;
    public float golemChargeSpeed;
    public float golemProjectileSpeed;

    public float IRMaxHp;
    public float IRDefense;
    public float IRMovementSpeed;
    public float IRRadiusAffect;
    public float IRSpeedBuff;
    public float IRMaxHpBuff;
    public float IRAttackBuff;

    private KamikazeEnemy flyingKam;
    private Golem golem;
    private IrradiatedEnemies IREnemy;

    private EnemyType type;
    
    void Start()
    {
        if(gameObject.name == "KamikaziBirdShort")
        {
            flyingKam = new KamikazeEnemy(kamikazeMaxHP, (int)kamikazeMovementSpeed, kamikazeDefense, gameObject, (int)kamikazeIQ);
            type = EnemyType.FlyingKamikaze;
        }
        else if(gameObject.name == "GolemParent")
        {
            golem = new Golem(golemMaxHp, (int)golemMovementSpeed, golemDefense, gameObject, golemProjectileSpeed, golemChargeSpeed);
            type = EnemyType.Brawler;
        }
        else if(gameObject.name == "IREnemy")
        {
            IREnemy = new IrradiatedEnemies(IRMaxHp, (int)IRMovementSpeed, IRDefense, gameObject, IRRadiusAffect, IRSpeedBuff, IRMaxHpBuff, IRAttackBuff);
            type = EnemyType.IrradiatedEnemy;
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
}
