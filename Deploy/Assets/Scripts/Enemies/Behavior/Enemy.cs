using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
//James Joko
public class Enemy: IClass {

    public static List<Enemy> enemyList = new List<Enemy>();

    private EnemyType type;

    public StatSystem enemyStats;

    public GameObject referenceObject;

    private bool attackIsBoosted = false;
    private bool speedIsBoosted = false;
    private bool maxHPIsBoosted = false;
    private bool reloadIsBoosted = false;
    private int boostTimerDelay = 1000;
    private Timer attackBoostedTimer;
    private Timer speedBoostedTimer;
    private Timer maxHPBoostedTimer;
    private Timer reloadBoostedTimer;

    private ObjectUpdate o;
    public Type MonoScript
    {
        get
        {
            return typeof(EnemyMono);
        }
    }

    //Constructor enemy with no weapon
    public Enemy(float maxhp, int ms, float defense, GameObject referenceObject)
    {
        enemyStats = new StatSystem(maxhp, ms, defense);
        this.referenceObject = referenceObject;
        o = new ObjectUpdate();
    }

    //Accessors 
    public EnemyType getType()
    {
        return type;
    }

    public void destroyEnemy()
    {
        if(enemyStats.getHealth() <= 0)
        {
            GameObject.Destroy(referenceObject);
        }
    }

    //Everything above is accessor

    //HP mutator
    public void changePosition(Vector3 newPos)
    {
        referenceObject.transform.position = newPos;
        o.SetPosition(newPos);
        ObjectHandler.Update(o, referenceObject);
    }
    public void changeRotation(Quaternion newRot)
    {
        referenceObject.transform.rotation = newRot;
        o.SetRotation(newRot);
        ObjectHandler.Update(o, referenceObject);
    }

    public bool isBoosted(string bType)
    {
        if (bType == "Attack")
        {
            return attackIsBoosted;
        }
        else if (bType == "Speed")
        {
            return speedIsBoosted;
        }
        else if (bType == "MaxHP")
        {
            return maxHPIsBoosted;
        }
        else if (bType == "Reload")
        {
            return reloadIsBoosted;
        }
        else
        {
            return true;
        }
    }

    //Starts Boost Timers
    public void startBoostTimer(string bType, float boost)
    {
        if (bType == "Attack")
        {
            attackIsBoosted = true;
            attackBoostedTimer = new Timer(boostTimerDelay);
            attackBoostedTimer.Elapsed += (_, __) =>
            {
                removeBoost("Attack", boost);
                attackBoostedTimer.Stop();
                attackBoostedTimer.Dispose();
            };
        }
        else if (bType == "Speed")
        {
            speedIsBoosted = true;
            speedBoostedTimer = new Timer(boostTimerDelay);
            speedBoostedTimer.Elapsed += (_, __) =>
            {
                removeBoost("Speed", boost);
                speedBoostedTimer.Stop();
                speedBoostedTimer.Dispose();
            };
        }
        else if (bType == "MaxHP")
        {
            maxHPIsBoosted = true;
            maxHPBoostedTimer = new Timer(boostTimerDelay);
            maxHPBoostedTimer.Elapsed += (_, __) =>
            {
                removeBoost("MaxHP", boost);
                maxHPBoostedTimer.Stop();
                maxHPBoostedTimer.Dispose();
            };
        }
        else if (bType == "Reload")
        {
            reloadIsBoosted = true;
            reloadBoostedTimer = new Timer(boostTimerDelay);
            reloadBoostedTimer.Elapsed += (_, __) =>
            {
                removeBoost("Reload", boost);
                reloadBoostedTimer.Stop();
                reloadBoostedTimer.Dispose();
            };

        }
    }

    //Resets Boost Timers
    public void RenewBoostTimers(string bType)
    {
        if (bType == "Attack")
        {
            attackBoostedTimer.Stop();
            attackBoostedTimer.Start();
        }
        else if (bType == "Speed")
        {
            speedBoostedTimer.Stop();
            speedBoostedTimer.Start();
        }
        else if (bType == "MaxHP")
        {
            maxHPBoostedTimer.Stop();
            maxHPBoostedTimer.Start();
        }
        else if (bType == "Reload")
        {
            reloadBoostedTimer.Stop();
            reloadBoostedTimer.Start();
        }
    }

    //Removes Boosts 
    private void removeBoost(string bType, float boost)
    {
        if (bType == "Attack")
        {
            attackIsBoosted = false;
            //divide enemy attack by boost to return to original value
        }
        else if (bType == "Speed")
        {
            speedIsBoosted = false;
            //divide enemy speed by boost to return to original value
        }
        else if (bType == "MaxHP")
        {
            maxHPIsBoosted = false;
            //divide enemy maxHP by boost to return to original value
        }
        else if (bType == "Reload")
        {
            reloadIsBoosted = false;
            //divide enemy reload by boost to return to original value
        }
    }
}
public enum EnemyType
{
    FlyingKamikaze,
    Brawler,
    IrradiatedEnemy
}
