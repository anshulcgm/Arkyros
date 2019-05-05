using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrab : Enemy
{
    private float chargeSpeed; //Ability: Shrab Charge
    private float chargeDmg; // Ability: Shrab Charge 
    private float eruptionRadius; // Ability: Centipede Eruption
    private float shurikenSpeed; // Ability: Water Shuriken
    private int numShrabs; // Number of shrabs in this chain

    //Shrab Constructor
    public Shrab(float hp, int ms, float defense, GameObject referenceObject, float chargeSpeed, float chargeDmg, float eruptionRadius, float shurikenSpeed, int numShrabs) : base(hp, ms, defense, referenceObject)
    {
        this.chargeSpeed = chargeSpeed;
        this.chargeDmg = chargeDmg;
        this.eruptionRadius = eruptionRadius;
        this.shurikenSpeed = shurikenSpeed;
        this.numShrabs = numShrabs;
	}

    //Start Mutators

    //Change Ability: Shrab Charge - Speed
    public void setchargeSpeed(float proportion)
    {
        chargeSpeed *= proportion;
    }

    //Change Ability: Shrab Charge - Damage
    public void setchargeDmg(float proportion)
    {
        chargeDmg *= proportion;
    }

    //Change Ability: Centipede Eruption - Radius of AOE Knockback
    public void setEruptionRadius(float proportion)
    {
        eruptionRadius *= proportion;
    }

    //Change Ability: Water Shuriken - Speed
    public void setShurikenSpeed(float speed)
    {
        shurikenSpeed *= speed;
    }

    //Change Number of shrabs in this chain
    public void setShrabs(int num)
    {
        numShrabs += num;
    }

    //End Mutators

    //Start Accessors
    
    //Return Ability: Shrab Charge - Speed
    public float getChargeSpeed()
    {
        return chargeSpeed;
    }
    
    //Return Ability: Shrab Charge - Damage
    public float getChargeDmg()
    {
        return chargeDmg;
    }

    //Return Ability: Centipede Eruption - Radius of AOE Knockback
    public float getEruptionRadius()
    {
        return eruptionRadius;
    }

    //Return Ability: Water Shuriken - Speed of Shuriken
    public float getShurikenSpeed()
    {
        return shurikenSpeed;
    }

    //Return how many shrabs are in this chain
    public int getNumShrabs()
    {
        return numShrabs;
    }

    //End Accessors
}
