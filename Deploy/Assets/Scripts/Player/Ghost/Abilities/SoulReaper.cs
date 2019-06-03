using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulReaper : Passives
{
    Stats stats;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void Update()
    {

    }

    public new void onKill()
    {
        stats.heal(100); // arbitrary amount as of rn
    }

    public new void damageTaken(float damage)
    {

    }
}
