using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class AccelerateThroughGate : MonoBehaviour
{
    private float speed = 0;
    private bool pushedThrough = false;
    private int ridOfParticle = 0;
    private Vector3 particleLocation;
    // Start is called before the first frame update
    void Start()
    {
        particleLocation = this.transform.GetChild(3).gameObject.transform.position;

        System.Timers.Timer timer = new System.Timers.Timer(250);
        timer.Enabled = true;
        timer.Elapsed += (sender, e) =>
        {
            pushedThrough = true;
            timer.Stop();
            timer.Dispose();

        };
        System.Timers.Timer timerP = new System.Timers.Timer(5000);
        timerP.Enabled = true;
        timerP.Elapsed += (sender, e) =>
        {
            Debug.Log("TimerP has elapsed");
            ridOfParticle = 1;
            timerP.Stop();
            timerP.Dispose();
        };
    }

    // Update is called once per frame
    void Update()
    {



        if (pushedThrough)
        {
            speed = 0f;
        }
        else
        {
            speed += 50f;
        }
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (ridOfParticle == 1)
        {
            ridOfParticle = 2;
            Destroy(this.transform.GetChild(3).gameObject);
            Debug.Log("Enemies entry prefab destroyed");
        }
        else if (ridOfParticle == 0)
        {
            this.transform.GetChild(3).gameObject.transform.position = particleLocation;
        }
    }

}
