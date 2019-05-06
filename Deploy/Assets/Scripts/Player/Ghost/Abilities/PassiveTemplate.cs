using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveTemplate : Passive
{
    //ADD passive ability's tier - check from ability log
    public int tier = 0; 

    public override void On()
    {
        //code for ability
    }
    public override void Off()
    {
        //reverse ability's effects
    }
}
