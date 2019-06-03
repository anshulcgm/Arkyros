using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfMedication : Passives
{
    //ADD passive ability's tier - check from ability log
    Stats stats;
    DateTime start;
    public bool flag;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void Update()
    {
        if((DateTime.Now - start).TotalSeconds > 4 && !flag)
        {
            stats.allStats[(int)stats.health, (int)statModifier.Flat] += (.12f * (int)stats.maxHealth);
            flag = true;
        }
    }

    public new void onKill()
    {

    }

    public new void damageTaken()
    {
        start = DateTime.Now;
        flag = false;
    }
}
