using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//IMPORTANT MESSAGE: All entities will have a three Stats, baseStats, multiplierStats, flatStats.
//Base stats will only go up as one levels up
//The stat multipliers and flat are for changing stats in case one gets debuffed
//A 30% damage debuff would be enemy.multiplierStats.attack = 0.7
//Flat adjuster is for arbitrary flat value buffs, for example player.flatState.speed - 5.
//stats are calculated within each entity with something like baseStats * multipler + flat or maybe (base + flat) * multiplier
public struct Stats
{
    //go with this for now, we may need maxHealth as a separate variable
    public int health;
    public int healthRegen;

    public int energy;
    public int energyRegen;

    public int speed;

    public int attack;
    public int attackSpeed;

}
