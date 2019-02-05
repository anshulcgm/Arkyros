using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public GameObject[] enemies;

    private void Start()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(enemy, spawnPosition, spawnRotation);
        }

    }
}
