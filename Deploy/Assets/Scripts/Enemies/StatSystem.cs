using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The purpose of this class is to consolidate all the base stats into one object that will be instantiated whenever an enemy object is instantiated
public class StatSystem
{
    private float health;
    private float maxHealth;
    private float speed;
    private float baseDmgMultiplier;
    private float defense;

    public StatSystem(float maxHealth, float speed, float defense)
    {
        Debug.Log("Instantiating a stat system");
        this.maxHealth = maxHealth;
        health = this.maxHealth;
        this.speed = speed;
        this.defense = defense; 
    }

    //Mutators
    public void updateHealth(float amount)
    {
        health += amount;
    }

    public void multiplySpeed(float multiplier)
    {
        speed *= multiplier;
    }
    
    public void flatSpeed(float amount)
    {
        speed += amount;
    }

    public void multiplyDefense(float multiplier)
    {
        defense *= multiplier;
    }

    //Accessors
    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getHealth()
    {
        return health; 
    }

    public float getSpeed()
    {
        return speed;
    }

    public float getBdmgMultiplier()
    {
        return baseDmgMultiplier;
    }

    public float getDefense()
    {
        return defense;
    }
}
