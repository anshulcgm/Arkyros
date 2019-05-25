using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixShlyHealth : MonoBehaviour
{
    private GameObject shly1;
    private GameObject shly2;
    private GameObject shly3;
    private GameObject shly4;
    private GameObject shly5;
    private GameObject shly6;

    private float currentHealth;
    private float maxHealth = 0;

   

    public float testTimer;
    private float originalTimer;
    // Start is called before the first frame update
    void Start()
    {

        shly1 = transform.GetChild(0).GetChild(0).gameObject;
        shly2 = transform.GetChild(0).GetChild(1).gameObject;
        shly3 = transform.GetChild(0).GetChild(2).gameObject;
        shly4 = transform.GetChild(0).GetChild(3).gameObject;
        shly5 = transform.GetChild(0).GetChild(4).gameObject;
        shly6 = transform.GetChild(0).GetChild(5).gameObject;

       

        originalTimer = testTimer;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Code here depletes shly health to test disappearing mechanic
        testTimer -= Time.deltaTime;
        if(testTimer < 0)
        {
            GetComponent<StatManager>().changeHealth(-maxHealth / 6);
   `         testTimer = originalTimer;
        }
        */
        if(maxHealth == 0)
        {
            maxHealth = GetComponent<StatManager>().shly.enemyStats.getMaxHealth();
        }
        
        currentHealth = GetComponent<StatManager>().shly.enemyStats.getHealth();
        if (currentHealth <= 5.0f / 6.0f * maxHealth)
        {
            if (shly1 != null)
            {
                GetComponent<StatManager>().shly.changeAggregateNum(-1);
                Destroy(shly1);
            }
        }
        if(currentHealth <= 4.0f/6.0f * maxHealth)
        {
            if(shly2 != null)
            {
                GetComponent<StatManager>().shly.changeAggregateNum(-1);
                Destroy(shly2);
            }
        }
        if (currentHealth <= 3.0f / 6.0f * maxHealth)
        {
            if (shly3 != null)
            {
                GetComponent<StatManager>().shly.changeAggregateNum(-1);
                Destroy(shly3);
            }
        }
        if (currentHealth <= 2.0f / 6.0f * maxHealth)
        {
            if (shly4 != null)
            {
                GetComponent<StatManager>().shly.changeAggregateNum(-1);
                Destroy(shly4);
            }
        }
        if (currentHealth <= 1.0f / 6.0f * maxHealth)
        {
            if (shly5 != null)
            {
                GetComponent<StatManager>().shly.changeAggregateNum(-1);
                Destroy(shly5);
            }
        }
        
       
    }
}
