using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoInitPlanet : MonoBehaviour
{
    public int seed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlanetMono>().Create(seed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
