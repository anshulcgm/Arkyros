using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starship : Enemy
{
    private float iRHealthSpawnStart;
    private float bombDamage;
    private float bombRadius;
    private float rayDamage;
    private float rayWidth;
    private float turretDamage;


    //Constructor
    public Starship(float maxhp, int ms, float defense, GameObject referenceObject, float iRHealthSpawnStart, float bombDamage, float bombRadius, float rayDamage, float rayWidth, float turretDamage) : base(maxhp, ms, defense, referenceObject)
    {
        this.iRHealthSpawnStart = iRHealthSpawnStart;
        this.bombDamage = bombDamage;
        this.bombRadius = bombRadius;
        this.rayDamage = rayDamage;
        this.rayWidth = rayWidth;
        this.turretDamage = turretDamage;
    }

    public void RadiationCreation()
    {
        if (this.enemyStats.getHealth() <= iRHealthSpawnStart)
        {
            //Spawn Irradiated
        }
    }

}
