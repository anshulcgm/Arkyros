using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    //Abilities instance variables
    private float chargeSpeed; //Ability: Charge
    private float chargeDmg; // Ability: Charge
    private float boulderSpeed; // Ability: Shoot
    private float boulderDmg; // Ability: Shoot
    //private float groundPoundSpeed;
    private float groundPoundDmg; // Ability: Ground Pound
    //private float weight;

    //Constructor
    public Golem(float hp, int ms, float defense, GameObject referenceObject, float boulderSpeed, float boulderDmg, float chargeSpeed, float chargeDmg, float groundPoundDmg) : base(hp, ms, defense, referenceObject)
    {
        this.boulderSpeed = boulderSpeed; // Ability: Shoot
        this.boulderDmg = boulderDmg; // Ability: Shoot
        this.chargeSpeed = chargeSpeed; // Ability: Charge
        this.chargeDmg = chargeDmg; // Ability: Charge
        this.groundPoundDmg = groundPoundDmg; // Ability: Ground Pound
    }

    //Mutators
    //Change Ability: Shoot - Speed
    public void setBoulderSpeed(float proportion)
    {
        boulderSpeed *= proportion;
    }

    //Change Ability: Shoot - Damage
    public void setBoulderDmg(float proportion)
    {
        boulderDmg *= proportion;
    }

    //Change Ability: Charge - Speed
    public void setchargeSpeed(float proportion)
    {
        chargeSpeed *= proportion;
    }

    //Change Ability: Charge - Damage
    public void setchargeDmg(float proportion)
    {
        chargeDmg *= proportion;
    }

    //Change Ability: Ground Pound - Speed
    //public void setGroundPoundSpeed(float proportion)
    //{
    //    groundPoundDmg *= proportion;
    //}

    //Change Ability: Ground Pound - Damage
    public void setGroundPoundDmg(float proportion)
    {
        groundPoundDmg *= proportion;
    }
    //END Mutators


    //Accessors
    //Return Ability: Shoot - Projectile Speed
    public float getBoulderSpeed()
    {
        return boulderSpeed;
    }

    //Return Ability: Shoot - Damage
    public float getBoulderDmg()
    {
        return boulderDmg;
    }

    //Return Ability: Charge - Speed
    public float getChargeSpeed()
    {
        return chargeSpeed;
    }

    //Return Ability: Charge - Damage
    public float getChargeDmg()
    {
        return chargeDmg;
    }

    ////Return Ability: Ground Pound - Speed
    //public float getGroundPoundSpeed()
    //{
    //    reuturn groundPoundSpeed;
    //}

    //Return Ability: Ground Pound - Damage
    public float getGroundPoundDmg()
    {
        return groundPoundDmg;
    }
    //END Accessors
}
