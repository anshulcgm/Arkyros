using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Rigidbody r;
    public float innerRadius;
    public float outerRadius;
    public GameObject[] playerList;


    // Start is called before the first frame update
    void Start()
    {
        this.innerRadius = 25;
        this.outerRadius = 75;
        this.playerList = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Find Everything in Inner Kill Range
        foreach (GameObject p in playerList)
        {
            if (Vector3.Distance(p.transform.position, transform.position) < innerRadius)
            {
                //Kill player object -> p.kill();
            }
        }
        //Find everything in Outer Kill Range
        foreach (GameObject p in playerList)
        {
            if (Vector3.Distance(p.transform.position, transform.position) < outerRadius)
            {
                //Deal massive damage to player object -> p.stats.changeHealth();
            }
        }
    }
}
