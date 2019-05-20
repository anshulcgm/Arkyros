using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defiant : Passive
{
    //ADD passive ability's tier - check from ability log
    public int tier = 0;
    public bool isActive;

    public bool used;

    public override void On()
    {
        isActive = true;
    }
    public override void Off()
    {
        isActive = false;
        //REVERSE EFFECTS IF NEEDED
    }

    void FixedUpdate()
    {
        if (isActive && Stats.Health < 20 && !used)
        {
            // Stats.addBuff((int)buff.Unstoppable, 720);
            //Stats.Defense.IncreaseDefense(20);
            used = true;
        }
        if(Stats.Health == 0)
        {
            used = false;
        }
    }
}
