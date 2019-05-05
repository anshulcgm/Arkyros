using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRBehaviorManager : MonoBehaviour
{
    public GameObject shly;
    void Start()
    {
        Dictionary<GameObject, int> toInstantiate = new Dictionary<GameObject, int>()
        {
            {shly, 3 }
        };
        GetComponent<StatManager>().IREnemy.spawnEnemy(EnemyType.Shly, toInstantiate, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
