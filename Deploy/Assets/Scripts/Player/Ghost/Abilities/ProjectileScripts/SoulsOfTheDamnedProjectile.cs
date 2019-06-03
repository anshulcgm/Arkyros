using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsOfTheDamnedProjectile : MonoBehaviour
{
    private Transform target;
    DateTime start;
    private GameObject[] enemies;
    private Transform[] enemyTransform;
    bool awake;
    public AnimationController anim;

    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AnimationController>();
        awake = false;
        soundManager = GetComponent<SoundManager>();
        start = DateTime.Now;
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //takes all enemies, puts their transforms into an array
        enemyTransform = new Transform[enemies.Length];

        for (int i = 0; i < enemies.Length; i++)
        {
            enemyTransform[i] = enemies[i].transform;
        }

        target = GetClosestEnemy(enemyTransform); //sets target to nearest enemy
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!awake)
        {
            soundManager.playOneShot("SoulsOfDamnedFloating");
            awake = true;
            anim.PlayLoopingAnim("SpiritRun");
        }
        if ((DateTime.Now - start).TotalSeconds > 8)//bullet lifetime of 8 seconds
        {
            Destroy(this.gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, .12f);
    }

    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<StatManager>().changeHealth(-10);
            soundManager.playOneShot("SoulsOfDamnedDamage");
        }
        
        Destroy(this.gameObject);//gets destroyed on contact, even terrain
    }
}
