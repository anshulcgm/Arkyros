using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adrenaline : Passive
{
    //ADD passive ability's tier - check from ability log
    public int tier = 0;
    public bool isActive;

    public bool used;

    //Stats stats = getComponent<Stats>();

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
        /*
        if (isActive && Stats.health < 20 && !used)
        {
            //cleanse debuffs
            //stats.addBuff adrenaline buff, 720
            //increase attack by 20
            //increase speed
            used = true;
        }
        if(Stats.health == 0)
        {
            used = false;
        }
        */
    }
}
