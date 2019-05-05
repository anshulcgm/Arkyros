using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;


public class PelletDropMono : MonoBehaviour
{
    // Start is called before the first frame update

    public float dropRange;
    public GameObject pellet = null;
    private bool canAttack = true;
    private int invokeCountdown = 3;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        this.transform.Translate(Vector3.right * Time.deltaTime);
    }

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
    }
}
