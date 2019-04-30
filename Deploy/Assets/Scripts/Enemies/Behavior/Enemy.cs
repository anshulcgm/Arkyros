using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
//James Joko
public class Enemy : IClass
{

    public static List<Enemy> enemyList = new List<Enemy>(); //Enemy classification list

    private EnemyType type; //This enemy classification

    public StatSystem enemyStats; //Stat System

    public GameObject referenceObject; //this.object

    //booleans for boosters
    private bool attackIsBoosted = false;
    private bool speedIsBoosted = false;
    private bool maxHPIsBoosted = false;
    private bool reloadIsBoosted = false;
    private int boostTimerDelay = 1000;
    //END booleans for boosters

    //Duration of boosters (if any)
    private Timer attackBoostedTimer;
    private Timer speedBoostedTimer;
    private Timer maxHPBoostedTimer;
    private Timer reloadBoostedTimer;
    //END Duration of boosters (if any)

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

    //If health drops to zero or lower, kill enemy
    public void destroyEnemy()
    {
        if (enemyStats.getHealth() <= 0)
        {
            GameObject.Destroy(referenceObject);
        }
    }

    //Accessors 
    //Get Enemy Classification
    public EnemyType getType()
    {
        return type;
    }

    // Return if a certain booster is being applied to this enemy
    public bool isBoosted(string boosterType)
    {
        if (boosterType == "Attack")
        {
            return attackIsBoosted;
        }
        else if (boosterType == "Speed")
        {
            return speedIsBoosted;
        }
        else if (boosterType == "MaxHP")
        {
            return maxHPIsBoosted;
        }
        else if (boosterType == "Reload")
        {
            return reloadIsBoosted;
        }
        else
        {
            return true;
        }
    }
    //END Accessors

    //Mutate where this enemy is standing
    public void changePosition(Vector3 newPos)
    {
        referenceObject.transform.position = newPos;
        o.SetPosition(newPos);
        ObjectHandler.Update(o, referenceObject);
    }

    //Mutate direction this enemy is facing
    public void changeRotation(Quaternion newRot)
    {
        referenceObject.transform.rotation = newRot;
        o.SetRotation(newRot);
        ObjectHandler.Update(o, referenceObject);
    }

    //Starts Boost Timers
    public void startBoostTimer(string bType, float boost)
    {
        if (bType == "Attack")
        {
            attackIsBoosted = true;
            attackBoostedTimer = new Timer(boostTimerDelay);
            //associates delegate with attackBoostTimer.Elapsed event
            //creates an anonymous function which takes two required parameters for elapsed events
            //but doesn't use them 
            //stuff will run when the timer elapses
            attackBoostedTimer.Elapsed += (sender, e) =>
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
            //associates delegate with speedBoostedTimer.Elapsed event
            //creates an anonymous function which takes two required parameters for elapsed events
            //but doesn't use them 
            //stuff will run when the timer elapses
            speedBoostedTimer.Elapsed += (sender, e) =>
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
            //associates delegate with maxHPBoostedTimer.Elapsed event
            //creates an anonymous function which takes two required parameters for elapsed events
            //but doesn't use them 
            //stuff will run when the timer elapses
            maxHPBoostedTimer.Elapsed += (sender, e) =>
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
            //associates delegate with reloadBoostedTimer.Elapsed event
            //creates an anonymous function which takes two required parameters for elapsed events
            //but doesn't use them 
            //stuff will run when the timer elapses
            reloadBoostedTimer.Elapsed += (sender, e) =>
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
