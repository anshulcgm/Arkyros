using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {
    public GameObject projectile;

    public GameObject leftArmTip;
    public GameObject rightArmTip;

    public float projSpeed;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() { 
	}

    void shootProj()
    {
        GameObject leftProjectile = (GameObject)Instantiate(projectile, leftArmTip.transform.position - leftArmTip.transform.right * 2.5f, transform.rotation);
        leftProjectile.GetComponent<Rigidbody>().velocity = -leftArmTip.transform.right * projSpeed;
        //Debug.Log("Left arm position is " + leftArmTip.transform.position);
        GameObject rightProjectile = (GameObject)Instantiate(projectile, rightArmTip.transform.position - rightArmTip.transform.right * 2.5f, transform.rotation);
        rightProjectile.GetComponent<Rigidbody>().velocity = -rightArmTip.transform.right * projSpeed;
    }
	
}
