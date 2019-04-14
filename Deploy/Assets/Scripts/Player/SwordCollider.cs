using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    private int attackDelay = 0;

    void Update()
    {
        
        if (Input.GetMouseButton(0) && attackDelay == 0)
        {
            //run the animation, will be moved to another script
            attackDelay = 60; //can attack 60 frames later
        }

        attackDelay--;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); //other.takeDamage();
        }
    }
}
