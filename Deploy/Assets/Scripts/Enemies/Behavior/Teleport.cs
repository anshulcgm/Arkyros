using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Lucas Sanchez


public class Teleport : MonoBehaviour {

    public Vector3 newPosition;

    // Use this for initialization
    void Start () {
        this.transform.position = newPosition;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
