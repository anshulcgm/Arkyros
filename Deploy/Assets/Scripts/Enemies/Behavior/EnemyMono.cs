using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMono : MonoBehaviour, IMono {

    Enemy e;
    public IClass GetMainClass()
    {
        return e;
    }

    public void SetMainClass(IClass ic)
    {
        e = (Enemy)ic;
    }

    void Start () {
        //Functions common to all enemies
        //e.adjustHealth(5f);
	}

	void Update () {
        //Functions common to all enemies
        //e.changeRotation(new Quaternion(0, 0, 0, 1));
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {

        }
        //e.OnCollisionEnter(collision);
    }
}
