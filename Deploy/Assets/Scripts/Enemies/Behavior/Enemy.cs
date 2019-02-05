using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//James Joko
public class Enemy {

    private string name; //Enemy name
    private int hp; //could be double, base hp
    private string faction; //Faction enemy belongs to
    //private Weapon weapon; //Weapon currently equiped, can be null meaning no weapon
    private int ms; //Movement speed
    private int bDmg; // Base damage without a weapon
    private Vector3 position; // Current position in the environment

    //Constructor enemy with no weapon
    public Enemy(string name, int hp, string faction, int ms, int bDmg)
    {
        this.name = name;
        this.hp = hp;
        this.faction = faction;
        //this.weapon = null;
        this.ms = ms;
        this.bDmg = bDmg;
    }


    //Everything below is accessor
    public string getName()
    {
        return name;
    }

    public int getHp()
    {
        return hp;
    }

    public string getFaction()
    {
        return faction;
    }

    //public String getWeapon()
    //{
    //    return weapon.toString();
    //}

    public int getMS()
    {
        return ms;
    }

    public int getBDmg()
    {
        return bDmg;
    }
    //Everything above is accessor

    //HP mutator
    public void incrementHP(int r)
    {
        this.hp += r;
    }

    //get damage enemy deals
    //public int Damage()
    //{
    //    if (this.weapon != null)
    //    {
    //        return bDmg;
    //    }
    //    else
    //    {
    //        return weapon.Damage();
    //    }
    //}

    public void move()
    {
        //Mutate position, needs to account for bounds
    }

    //info
    public string toString()
    {
        return "Enemy Name: " + this.name + "\nCurrent HP: " + this.hp + "\nFaction: " + this.faction 
            + "\nMovement Speed: " + this.ms + "\nBase Damage: " + this.bDmg;

    }
}

