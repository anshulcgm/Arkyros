using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Timers;


public class PelletDropMono : MonoBehaviour
{
    // Start is called before the first frame update

    //Old Pellet Drop Stuff
    /*public float dropRange;
    public GameObject pellet = null;
    private bool canAttack = true;
    private int invokeCountdown = 3;*/


    public GameObject Projectile;
    public float timer;
    public float variableTimer;
    // Start is called before the first frame update
    void Start()
    {
        variableTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject laser = (GameObject)Instantiate(Projectile, gameObject.transform.position, gameObject.transform.rotation);
         }

        //Old Pellet Drop Stuff
        /*
        if (canAttack)
        {

            RaycastHit hit;
            //Debug.DrawRay(transform.position, Vector3.down * dropRange, Color.yellow, 1, true);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, dropRange))
            {
                if (hit.collider.tag == "Player")
                {
                    ReleasePellets();
                    invokeCountdown = 3;
                    canAttack = false;
                    Timer timer = new Timer(2000);
                    timer.Enabled = true;
                    //associates delegate with timer.Elapsed event
                    //creates an anonymous function which takes two required parameters for elapsed events
                    //but doesn't use them 
                    //stuff will run when the timer elapses
                    timer.Elapsed += (sender, e) =>
                    {
                        canAttack = true;
                        timer.Stop();
                        timer.Dispose();
                    };
                }
            }
        }
        //Just used for test scene, moves object
        this.transform.Translate(Vector3.right * Time.deltaTime);*/

    }

    /*Old Pellet Drop Stuff
    private void ReleasePellets()
    {
        InvokeRepeating("ReleasePellet", 0.9f, 0.3f);
    }

    private void ReleasePellet()
    {
        Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z);
        if (invokeCountdown > 0)
        {
            Instantiate(pellet, pos, Quaternion.identity);
        }
        invokeCountdown--;
    }*/
}
