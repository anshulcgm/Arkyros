using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachChildrenObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(transform.GetChild(0).gameObject);
        //transform.GetChild(0).GetChild(0).DetachChildren();
        //transform.GetChild(0).DetachChildren();
        //transform.DetachChildren();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 5f);
    }
}
  