using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defiant : Passive
{
    //ADD passive ability's tier - check from ability log
    public int tier = 0;
    public bool isActive;

    public bool used;

    Stats stats;

    public override void On()
    {
        isActive = true;
    }
    public override void Off()
    {
        isActive = false;
        //REVERSE EFFECTS IF NEEDED
    }

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void FixedUpdate()
    {
        if (isActive && stats.health < 20 && !used)
        {
            // Stats.addBuff((int)buff.Unstoppable, 720);
            //Stats.Defense.IncreaseDefense(20);
            used = true;
        }
        if(stats.health == 0)
        {
            used = false;
        }



    }
    
}
