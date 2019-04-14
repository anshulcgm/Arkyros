using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy {
	private float knockbackSpeed;
	private float groundPoundSpeed;
	private float boulderSpeed;
	private float weight;
	
	public Golem(float hp, int ms, GameObject referenceObject, float weight, float boulderSpeed, float groundPoundSpeed, float knockbackSpeed) : base(hp,ms,referenceObject) {
		this.weight = weight;
		this.boulderSpeed = boulderSpeed;
		this.groundPoundSpeed = groundPoundSpeed;
		this.knockbackSpeed = knockbackSpeed;
	}

	public void setWeight(float proportion) {
		weight *= proportion;
	}
	public void setBoulderSpeed(float proportion) {
		boulderSpeed *= proportion;
	}
	public void setGroundPoundSpeed(float proportion) {
		groundPoundSpeed *= proportion;
	}
	public void setKnockBackSpeed(float proportion) {
		knockbackSpeed *= proportion;
	}

	public float getWeight() {
		return weight;
	}
	public float getBoulderSpeed() {
		return boulderSpeed;
	}
	public float getGroundPoundSpeed() {
		return groundPoundSpeed;
	}
	public float getKnockBackSpeed() {
		return knockbackSpeed;
	}
}
