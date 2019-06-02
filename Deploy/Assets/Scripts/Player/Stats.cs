using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//IMPORTANT MESSAGE: All entities will have a three Stats, baseStats, multiplierStats, flatStats.
//Base stats will only go up as one levels up
//The stat multipliers and flat are for changing stats in case one gets debuffed
//A 30% damage debuff would be enemy.multiplierStats.attack = 0.7
//Flat adjuster is for arbitrary flat value buffs, for example player.flatState.speed - 5.
//stats are calculated within each entity with something like baseStats * multipler + flat or maybe (base + flat) * multiplier


//For the following stat arrays, [0] is the base, [1] is the multiplier, and [2] is the flat
public enum statModifier { Base, Multiplier, Flat };
public enum stat { HealthRegen, /*ManaRegen*/ Speed, Attack, AttackSpeed, Defense };
public enum buff { Invisible, Gravityless, Unstoppable };

public class Stats : MonoBehaviour
{
    SoundManager soundManager;
    public int regenTimer = 0;
    //go with this for now, we may need maxHealth as a separate variable

    public float maxHealth;
    public float health;
    //public float maxMana;
    //public float mana;
    public float damageMultiplier;

    //Might be easier to use two enums and have a 2D array



    public float[,] allStats = new float[5, 3]; //first column is base stats, second multiplier, third flat. Each 
    public int[] buffs = new int[3];            //number changes with number of positive buffs



    /*
    public int[] statusAilments = new int[x] where x is the number of ailments
    //it's a int array because this also tracks duration
    
    public enum statusAilments {a, b, c, d, e, ....

    update function counts down, see below

    */

    public float getFinal(int statType) //getFinal((int)stats.HealthRegen)
    {
        return allStats[statType, (int)statModifier.Base] * allStats[statType, (int)statModifier.Multiplier] + allStats[statType, (int)statModifier.Flat];
        // Stat[0] * Stat[1] + Stat[2] same thing
    }

    public void Start()
    {
        //file reading, assign the numbers to the variables
        soundManager = GetComponent<SoundManager>();
    }

    public void Update()
    {
        //Regen should be every half second
        //Health Regen
        regenTimer++;
        if (regenTimer == 30) //half second
        {
            heal(getFinal((int)stat.HealthRegen));
            regenTimer = 0;
        }


        if (health == 0)
        {
            //die
        }

        if(buffs[(int)buff.Invisible] > 0)
        {
            //go transparent, invisible
        }








        for(int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i] > 0)
            {
                buffs[i]--;
            }
            
        }

        //need one for negative buffs too

    }
    /////////////// 
    //INTERACTIONS

    public void takeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); //might be moved to update
        //soundManager.playOneShot("TakeDamage");
    }

    public void heal(float healAmount)
    {
        health += healAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }




    //postive and negative are be separated
    public void addBuff(int buff, int duration)
    {
        buffs[buff] += duration;
    }

    public void setBuffDuration(int buff, int duration)
    {
        buffs[buff] = duration;
    }

    public void addStatus(int status, int duration)
    {
        //statusAilment[status] += duration;
    }

    public void dealDamage(GameObject target, float damage)
    {
        Debug.Log("REEEE1");
        if (target.tag == "Enemy")
        {
            
            target.GetComponent<StatManager>().changeHealth(-damage);
            Debug.Log("REEEE2");
            //not sure if this works exactly to detect if its been killed
            if (target == null)
            {
                //trigger onKill() passives
            }
        }

    }

}
