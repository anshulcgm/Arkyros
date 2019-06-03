using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veteran: Passives
{
    //ADD passive ability's tier - check from ability log
    Stats stats;

    int stacks = 0;
    bool stacksChanged = false;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void Update()
    {
        if (stacksChanged)
        {

        }
    }

    public new void onKill()
    {

    }

    public new void damageTaken(float damage)
    {
        if (stacks < 20)
        {
            stacks++;
            stacksChanged = true;

        }

        

        // stacks provide 2% damage reduction each

    }

    void addStack()
    {
        stats.allStats[(int)stat.Defense, (int)statModifier.Multiplier] *= 1.02f;
    }
}
