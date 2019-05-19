using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EtherealForm : Passive
{
    //ADD passive ability's tier - check from ability log
    public int tier = 0;
    public bool isActive;

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
            if() //(damage is dealt and target is ghost)
            {
                Random rnd = new Random();
                //int num = rnd.Next(1, 5);
                if(num == 1)
                {
                    //stats - don't deal damage
                }
            }
        }
    }
}
