using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargedRayParticleBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject starship;
    void Start()
    {
        starship = GameObject.Find("Starship");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = starship.transform.position;
        Destroy(gameObject, 1.5f);
    }
}
