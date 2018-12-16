using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {

    public GameObject spawnObj;
    public GameObject light;
    public int numObj;
    public int numLight;
    public float minRad;
    public float maxRad;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < numObj; i++)
        {
            Vector3 posn = Random.onUnitSphere * Random.Range(minRad, maxRad);
            Instantiate(spawnObj, posn + transform.position, Quaternion.identity);
        }
    }   
}
