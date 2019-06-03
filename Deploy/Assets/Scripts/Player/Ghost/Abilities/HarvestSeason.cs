using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestSeason : Passives
{
    //ADD passive ability's tier - check from ability log
    Stats stats;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void Update()
    {

    }
    void onCollisionEnter(Collision col) {
        if (col.gameObject.tag.Equals("HarvestSznTree")) {
            stats.GetComponent<Stats>().health += 3; //get size of tree, don't know how
            Destroy(col.gameObject);  
        }
    }
    public new void onKill()
    {

    }

    public new void damageTaken()
    {

    }
}
