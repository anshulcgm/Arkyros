 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Rigidbody r;
    public float innerRadius;
    public float outerRadius;
    public GameObject[] playerList;

    public GameObject starship;

    public GameObject partcleExplosion;

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

    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            Instantiate(partcleExplosion, this.transform.position, new Quaternion(0, 0, 0, 0));
            //Find Everything in Inner Kill Range
            foreach (GameObject p in playerList)
            {
                if (Vector3.Distance(p.transform.position, transform.position) < innerRadius)
                {
                    Destroy(p);
                }
                else if(Vector3.Distance(p.transform.position, transform.position) < outerRadius)
                {
                    //Deal damage
                }
            }
            Destroy(this.gameObject);
            Destroy(starship);
        }
    }
}
