using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy {
	private float knockbackSpeed;
	//private float groundPoundSpeed;
	private float boulderSpeed;
	//private float weight;
	
	public Golem(float hp, int ms, float defense, GameObject referenceObject, float boulderSpeed, float knockbackSpeed) : base(hp,ms,defense, referenceObject) {
        this.boulderSpeed = boulderSpeed;
		this.knockbackSpeed = knockbackSpeed;
	}
	
	public void setBoulderSpeed(float proportion) {
		boulderSpeed *= proportion;
	}
	public void setKnockBackSpeed(float proportion) {
		knockbackSpeed *= proportion;
	}

    public float getBoulderSpeed()
    {
        return boulderSpeed;
    }

	public float getKnockBackSpeed() {
		return knockbackSpeed;
	}
}
