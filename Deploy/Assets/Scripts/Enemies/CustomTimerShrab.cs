using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTimerShrab : MonoBehaviour
{
    // Start is called before the first frame update

    public float shrab1Timer;
    public float shrab2Timer;
    public float shrab3Timer;
    public float shrab4Timer;

    public GameObject shrab1;
    public GameObject shrab2;
    public GameObject shrab3;
    public GameObject shrab4; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shrab1Timer -= Time.deltaTime;
        if(shrab1Timer <= 0)
        {
            shrab1.SetActive(true);
        }

        shrab2Timer -= Time.deltaTime;
        if (shrab2Timer <= 0)
        {
            shrab2.SetActive(true);
        }

        shrab3Timer -= Time.deltaTime;
        if (shrab3Timer <= 0)
        {
            shrab3.SetActive(true);
        }

        shrab4Timer -= Time.deltaTime;
        if (shrab4Timer <= 0)
        {
            shrab4.SetActive(true);
        }
    }
}
