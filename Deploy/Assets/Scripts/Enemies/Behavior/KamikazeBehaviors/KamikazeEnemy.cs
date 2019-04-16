using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Osbert Lee
public class KamikazeEnemy : Enemy {

    private int IQ;
    private int stealthField;
	private float diveSpeed;

    public KamikazeEnemy( float hp, int ms, float defense, GameObject rf, int intel) : base( hp, ms, defense, rf)
    {
        this.IQ = intel;
		this.referenceObject = rf;

        if (IQ >= 130)
        {
            stealthField = 15;
			diveSpeed = 10;
        }
        else if(IQ >= 100) {
			stealthField = 8;
			diveSpeed = 7;
        } else {
			stealthField = 5;
			diveSpeed = 5;
        }
    }


	public void alert(GameObject player) {
		float step = diveSpeed * Time.deltaTime;
		Vector3 playerPos = player.transform.position;
		//flies to the point where the player was when the kamikaze was alerted
		while (Vector3.Distance(referenceObject.transform.position, playerPos) > 0.001f) {
			referenceObject.transform.position = Vector3.MoveTowards(referenceObject.transform.position, playerPos, step);
		}
		//explosion - does damage if you are in a radius of 3
		if (Vector3.Distance(player.transform.position, referenceObject.transform.position) < 3.0f) {
			//player takes damage
		}

	}



	public int getIQ() {
		return IQ;
	}

	public int getStealthField() {
		return stealthField;
	}

	public float getDiveSpeed() {
		return diveSpeed;
	}
}
