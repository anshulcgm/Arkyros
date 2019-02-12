using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Toma Itagaki
public class DetectPlayer : MonoBehaviour {

    private Vector3 playerPosition;
    private Vector3 enemyPosition;

    private Vector3 direction;
    private float distance;


    public void findDistance()
    {
        this.distance = Vector3.Distance(playerPosition, enemyPosition);
        this.direction = (enemyPosition - playerPosition).normalized;

        if (distance < 10)
        {
            move(direction);
        }
    }

    public void move(Vector3 direction)
    {
        //move function
    }
}
