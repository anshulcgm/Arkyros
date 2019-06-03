using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EtherealForm : Passives
{
    //ADD passive ability's tier - check from ability log
    Stats stats;

    float chance = 10;
    float rand;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void Update()
    {

    }

    public new void onKill()
    {

    }

    public new void damageTaken(float damage)
    {
        rand = Random.value;
        chance *= rand;

        if (chance < 1)
        {
            stats.heal(damage);
            // Don't know if this prevents death (dipping in and out of death range)
        }        
    }
}
