using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrab : Enemy
{
    private float pincerDmg; // Ability: Shrab Charge 
    private float eruptionRadius; // Ability: Centipede Eruption
    private int numShrabs; // Number of shrabs in this chain

    public static List<Shrab> shrabList = new List<Shrab>();

    //Shrab Constructor
    public Shrab(float hp, int ms, float defense, GameObject referenceObject, float chargeDmg, float eruptionRadius, int numShrabs) : base(hp, ms, defense, referenceObject)
    {
        this.pincerDmg = chargeDmg;
        this.eruptionRadius = eruptionRadius;
        this.numShrabs = numShrabs;
	}

    //Start Mutators

    //Change Ability: Shrab Charge - Speed
 
    //Change Ability: Shrab Charge - Damage
    public void setchargeDmg(float proportion)
    {
        pincerDmg *= proportion;
    }

    //Change Ability: Centipede Eruption - Radius of AOE Knockback
    public void setEruptionRadius(float proportion)
    {
        eruptionRadius *= proportion;
    }

    //Change Ability: Water Shuriken - Speed

    //Change Number of shrabs in this chain
    public void setShrabs(int num)
    {
        numShrabs += num;
    }

    //End Mutators

    //Start Accessors
    
    //Return Ability: Shrab Charge - Speed
    
    //Return Ability: Shrab Charge - Damage
    public float getChargeDmg()
    {
        return pincerDmg;
    }

    //Return Ability: Centipede Eruption - Radius of AOE Knockback
    public float getEruptionRadius()
    {
        return eruptionRadius;
    }


    //Return how many shrabs are in this chain
    public int getNumShrabs()
    {
        return numShrabs;
    }

    //End Accessors
}
