using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralMovement : MonoBehaviour
{
	private Rigidbody rb;
	//original time
	private float oTime;
	public float speed;

	//max distance kamikaze can go
	public float maxDis;
	//how long it takes before looking for new position
	public float timer;
	//final position
	private Vector3 final;
	

	// Start is called before the first frame update
    void Start()
    {
		
		rb = GetComponent<Rigidbody>();
        oTime = timer;
		//set final position
		final = Random.insideUnitSphere * maxDis + transform.position;
		
    }

    // Update is called once per frame
    void Update()
    {
		//within the beginning of Update(), start moving towards final
		rb.velocity = (final - transform.position).normalized * speed;
		
		//rotation
		Quaternion rotation = Quaternion.LookRotation(final - transform.position);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);

		//once destination reached, or "reached", wait for x seconds and find new final
		if (Vector3.Distance(transform.position, final) < 0.1f) {

			timer -= Time.deltaTime;
			if (timer >= 0) {
				rb.velocity = Vector3.zero;
			} else {
				final = Random.insideUnitSphere * maxDis + transform.position;
				timer = oTime;
			}
			
		}
    }
}
