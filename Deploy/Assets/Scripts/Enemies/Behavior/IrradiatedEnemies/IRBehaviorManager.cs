using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IRBehaviorManager : MonoBehaviour
{
    public GameObject shly;
    public GameObject shrabPrefab;

    private float radiusOfImpact;
    private List<Enemy> enemiesInRadius;

    private float enemySpeedBuffWeight = 0.33f;
    private float enemyAttackBuffWeight = 0.33f;
    private float enemyHPBuffWeight = 0.33f;

    private float playerHealthDebuffWeight = 0.50f;
    private float playerSpeedDebuffWeight = 0.50f;

    private bool buffInProgress;
    private bool spawnEnemies;

    public float buffTimer;
    private float oBuffTimer;

    private EnemyType[] types;
    private Dictionary<GameObject, int> toInstantiate;

    public Material hpBuff;
    public Material speedBuff;
    public Material attackBuff;

    public GameObject IRCrystal;

    private Animator crystalController;

    void Start()
    {
        radiusOfImpact = GetComponent<StatManager>().IREnemy.getRadius();
        oBuffTimer = buffTimer;
        buffInProgress = false;
        spawnEnemies = false;
        types = new EnemyType[]
        {
            EnemyType.Shly,
            EnemyType.Shrab
        };
        toInstantiate = new Dictionary<GameObject, int>()
        {
            {shly, 12},
            {shrabPrefab, 6 }
        };
        //GetComponent<StatManager>().IREnemy.spawnEnemy(EnemyType.Shly, toInstantiate, 5.0f);

        crystalController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        while (!buffInProgress)
        {
            enemiesInRadius = new List<Enemy>();
            foreach (Enemy e in Enemy.enemyList)
            {
                if (Vector3.Distance(e.referenceObject.transform.position, transform.position) <= radiusOfImpact)
                {
                    enemiesInRadius.Add(e);
                }
            }
            Debug.Log(enemiesInRadius.Count + " enemies detected");
            if(enemiesInRadius.Count != 0)
            {
                float currentHealth = 0f;
                float totalHealth = 0.1f;
                foreach (Enemy e in enemiesInRadius)
                {
                    currentHealth += e.enemyStats.getHealth();
                    totalHealth += e.enemyStats.getMaxHealth();
                }
                System.Random rand = new System.Random();
                enemyHPBuffWeight = 1 - currentHealth / totalHealth;
                enemySpeedBuffWeight = 1 - enemyHPBuffWeight / rand.Next(1, 5);
                enemyAttackBuffWeight = 1 - enemyHPBuffWeight - enemySpeedBuffWeight;
                float randomNum = (float)rand.NextDouble();

                if (randomNum <= enemyHPBuffWeight)
                {
                    //HP Buff
                    Debug.Log("Buffing HP");
                    GetComponent<StatManager>().IREnemy.updateEnemiesHP();
                    IRCrystal.GetComponent<MeshRenderer>().material = hpBuff;
                    
                }
                else if (randomNum > enemyHPBuffWeight && randomNum <= enemyHPBuffWeight + enemySpeedBuffWeight)
                {
                    //Speed Buff
                    Debug.Log("Buffing speed");
                    GetComponent<StatManager>().IREnemy.updateEnemiesSpeed();
                    IRCrystal.GetComponent<MeshRenderer>().material = speedBuff;
                }
                else if (randomNum > enemyHPBuffWeight + enemySpeedBuffWeight && randomNum <= enemyHPBuffWeight + enemySpeedBuffWeight + enemyAttackBuffWeight)
                {
                    //Attack Buff
                    Debug.Log("Buffing Attack");
                    GetComponent<StatManager>().IREnemy.updateEnemiesAttack();
                    IRCrystal.GetComponent<MeshRenderer>().material = attackBuff;
                }
                buffInProgress = true;
                crystalController.SetTrigger("RotateCrystal");
            }
        }
        if (buffInProgress)
        {
            buffTimer -= Time.deltaTime;
            if (buffTimer < 0f)
            {
                buffInProgress = false;
                buffTimer = oBuffTimer;
            }
        }

        GetComponent<StatManager>().IREnemy.spawnEnemy(types, toInstantiate, radiusOfImpact);
    }
}
