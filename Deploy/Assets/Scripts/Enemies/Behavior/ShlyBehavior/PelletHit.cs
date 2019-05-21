using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class PelletHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Object.Destroy(gameObject, 2.0f);

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
            Debug.Log("Player Hit");
        Destroy(gameObject);
    }
}
