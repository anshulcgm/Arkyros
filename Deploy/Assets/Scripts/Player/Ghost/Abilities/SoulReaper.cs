using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulReaper : Passive
{
    public int tier = 6;
    public bool isActive;

    private int storedLifeForce = 0;

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
            if ()//if (button for attack pressed & enemies health falls)
            {
                storedLifeForce += 1; //life force increases with damage done to enemy
            }
            if()//health reaches 0 && storedLifeForce >= 7
            {
                //increase health by storedLifeForce
                storedLifeForce = 0;
            }
        }
    }
}
