using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeShlyHealth : MonoBehaviour
{
    private GameObject shly1;
    private GameObject shly2;
    private GameObject shly3;

    private float currentHealth;
    private float maxHealth = 0f; 
    // Start is called before the first frame update
    void Start()
    {
        shly1 = transform.GetChild(0).GetChild(0).gameObject;
        shly2 = transform.GetChild(0).GetChild(1).gameObject;
        shly3 = transform.GetChild(0).GetChild(2).gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(maxHealth == 0)
        {
            maxHealth = GetComponent<StatManager>().shly.enemyStats.getMaxHealth();
        }
        currentHealth = GetComponent<StatManager>().shly.enemyStats.getHealth();

        if(currentHealth <= 2.0f/3.0f * maxHealth)
        {
            if(shly1 != null)
            {
                GetComponent<StatManager>().shly.changeAggregateNum(-1);
                Destroy(shly1);
            }
            
        }
        if(currentHealth <= 1.0f/3.0f * maxHealth)
        {
            if(shly2 != null)
            {
                GetComponent<StatManager>().shly.changeAggregateNum(-1);
                Destroy(shly2);
            }
        }
    }
}
