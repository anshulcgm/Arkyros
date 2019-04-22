using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy {
	private float knockbackSpeed;
	//private float groundPoundSpeed;
	private float boulderSpeed;
    //private float weight;
    private float knockbackDmg;
    private float groundPoundDmg;
    private float projectileDmg; 
	
	public Golem(float hp, int ms, float defense, GameObject referenceObject, float boulderSpeed, float knockbackSpeed, float knockbackDmg, float groundPoundDmg, float projectileDmg) : base(hp,ms,defense, referenceObject) {
        this.boulderSpeed = boulderSpeed;
		this.knockbackSpeed = knockbackSpeed;
        this.knockbackDmg = knockbackDmg;
        this.groundPoundDmg = groundPoundDmg;
        this.projectileDmg = projectileDmg;
	}
	
	public void setBoulderSpeed(float proportion) {
		boulderSpeed *= proportion;
	}
	public void setKnockBackSpeed(float proportion) {
		knockbackSpeed *= proportion;
	}
    public void setKnockbackDmg(float proportion)
    {
        knockbackDmg *= proportion;
    }
    public void setGroundPoundDmg(float proportion)
    {
        groundPoundDmg *= proportion;
    }
    public void setProjDmg(float proportion)
    {
        projectileDmg *= proportion;
    }

    public float getBoulderSpeed()
    {
        return boulderSpeed;
    }

	public float getKnockBackSpeed() {
		return knockbackSpeed;
	}
    
    public float getKnockbackDmg()
    {
        return knockbackDmg;
    }
    public float getGroundPoundDmg()
    {
        return groundPoundDmg;
    }
    public float getProjDmg()
    {
        return projectileDmg;
    }
}
