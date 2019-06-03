using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adrenaline : Passives
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
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 1.2f;
            stats.allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 1.2f;
            used = true;
            divided = false;
        }
        if ((DateTime.Now - start).TotalSeconds < 12 && !divided)
        {
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] /= 1.2f;
            stats.allStats[(int)stat.Attack, (int)statModifier.Multiplier] /= 1.2f;
            divided = true;
        }
        if (stats.health == 0)
        {
            used = false;
        }
    }

}