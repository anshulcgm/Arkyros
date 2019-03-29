using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//James Joko
public class Enemy: IClass {

    public static List<Enemy> enemyList = new List<Enemy>();

    private EnemyType type;
    private float hp; //could be double, base hp
    private int ms; //Movement speed
    private GameObject referenceObject;

    private ObjectUpdate o;
    public Type MonoScript
    {
        get
        {
            return typeof(EnemyMono);
        }
    }

    //Constructor enemy with no weapon
    public Enemy(EnemyType type, float hp, int ms, GameObject referenceObject)
    {
        this.type = type;
        this.hp = hp;
        this.ms = ms;
        this.referenceObject = referenceObject;
        o = new ObjectUpdate();
    }
    public Enemy(float hp, int ms, GameObject referenceObject)
    {
        this.hp = hp;
        this.ms = ms;
        this.referenceObject = referenceObject;
        o = new ObjectUpdate();
    }

    //Accessors 
    public EnemyType getType()
    {
        return type;
    }

    public float getHp()
    {
        return hp;
    }

    public int getMS()
    {
        return ms;
    }
    //Everything above is accessor

    //HP mutator
    public void changeHealth(float adjustment)
    {
        //For health, have player create empty gameobject in the enemy collider when some sort of damage occurs with a tag such as "AOE"
        //Then in the OnCollisionEnter() in this class, check if the gameObject has certain tag such as "AOE" and adjust health accordingly
        hp += adjustment;
    }
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
