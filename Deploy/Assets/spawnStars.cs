using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class spawnStars : MonoBehaviour
{
    public GameObject star;
    public Camera player;
    public int numStars;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numStars; i++){
            GameObject starObj = Instantiate(star, UnityEngine.Random.onUnitSphere * radius, Quaternion.identity);
            starObj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
