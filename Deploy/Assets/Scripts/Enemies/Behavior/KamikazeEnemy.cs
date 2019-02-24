using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Osbert Lee
public class KamikazeEnemy : Enemy {

    private int IQ;
    private int stealthField;
    private int abilities;

    public KamikazeEnemy( float hp, int ms, GameObject referenceObject, int IQ, int stealthField) : base( hp, ms, referenceObject)
    {
        this.IQ = IQ;
        this.stealthField = stealthField;

        //if statement that determines the # of abilities
        //examples below
        if (IQ >= 130)
        {
            abilities = 5;
        }
        else if(IQ >= 100) {
            abilities = 3;
        } else {
            abilities = 1; 
            
        }
    }

    public void incIQ()
    {
        //increases IQ
        //has another if statement like in the constructor that sets number of abilities.
    }

    public void incStealthField()
    {
        //increases stealth field each time it dies? so it kind of gets smarter
    }

    public int getAbilities()
    {
		return abilities;
    }
}
