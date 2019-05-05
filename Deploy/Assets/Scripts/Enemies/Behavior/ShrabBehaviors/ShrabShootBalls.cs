using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrabShootBalls : MonoBehaviour
{
	public GameObject Projectile;
	public float timer;
	public float variableTimer;
    // Start is called before the first frame update
    void Start()
    {
        variableTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
		if (timer <= 0) {
			GameObject laser = (GameObject)Instantiate(Projectile, gameObject.transform.position, gameObject.transform.rotation);
			timer = variableTimer;
		}
    }
}
