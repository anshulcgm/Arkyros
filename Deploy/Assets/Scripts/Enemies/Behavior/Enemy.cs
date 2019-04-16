using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//James Joko
public class Enemy: IClass {

    public static List<Enemy> enemyList = new List<Enemy>();

    private EnemyType type;

    public StatSystem enemyStats;

    public GameObject referenceObject;

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
}
public enum EnemyType
{
    FlyingKamikaze,
    Brawler,
    IrradiatedEnemy
}
