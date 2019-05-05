using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsOfTheDamnedProjectile : MonoBehaviour
{
    private Transform target;

    private GameObject[] enemies;
    private Transform[] enemyTransform;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //takes all enemies, puts there transforms into an array
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
        transform.position = Vector3.MoveTowards(transform.position, target.position, .03f);
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
}
