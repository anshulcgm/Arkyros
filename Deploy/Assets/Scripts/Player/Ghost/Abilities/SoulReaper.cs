﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulReaper : Passive
{
    public int tier = 6;
    public bool isActive;

    private int storedLifeForce = 0;

    GhostSoundManager ghostSoundManager;

    public override void On()
    {
        isActive = true;
    }
    public override void Off()
    {
        isActive = false;
    }
    void FixedUpdate()
    {
        if (isActive)
        {
            if ()//if (damage is dealt and source is ghost)
            {
                storedLifeForce += 1; //life force increases with damage done to enemy
            }
            if()//health reaches 0 && storedLifeForce >= threshold
            {
                ghostSoundManager.playSoulReaperResurrect();
                //increase health by % that scales with storedLifeForce
                storedLifeForce = 0;
            }
        }
    }
}
