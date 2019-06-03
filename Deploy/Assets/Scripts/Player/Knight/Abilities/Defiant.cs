using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defiant : Passives
{
    //ADD passive ability's tier - check from ability log

    public bool used;
    public bool divided;

    Stats stats;
    DateTime start;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void FixedUpdate()
    {
        if (stats.health < 20 && !used)
        {
            start = DateTime.Now;
            // Stats.addBuff((int)buff.Unstoppable, 720);
            stats.allStats[(int)stat.Defense, (int)statModifier.Multiplier] *= 1.2f;
            used = true;
            divided = false;
        }
        if ((DateTime.Now - start).TotalSeconds < 12 && !divided)
        {
            stats.allStats[(int)stat.Defense, (int)statModifier.Multiplier] /= 1.2f;
            divided = true;
        }
        if (stats.health == 0)
        {
            used = false;
        }
    }

}