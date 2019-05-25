using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingLanding : MonoBehaviour
{
	
    public float speed;
	public GameObject planet;
	public float height;
	public float fTime;
	public float lTime;

	private float oTimeL;
	private float oTimeF;
	
	private bool checkL;
	private bool checkF;

	private Vector3 oPos;
	private Rigidbody rb;
	
	
     void Start () {
		rb = GetComponent<Rigidbody>();

		oPos = transform.position;

		checkL = false;
		checkF = true;

		oTimeL = lTime;
		oTimeF = fTime;
		
     }
     
     void Update () {
         
		 RaycastHit hit;

		 if (checkF && !checkL) {
			lTime -= Time.deltaTime;
			if (lTime < 0) {
				checkF = false;
				lTime = oTimeL;
				
			}
		 }
		 if (!checkF && !checkL && Physics.Raycast(transform.position, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity)) {
			

			rb.velocity = (planet.transform.position - transform.position).normalized * speed;
			
			if (Vector3.Distance(transform.position, hit.point + (transform.position - planet.transform.position).normalized*height) < 0.5f) {
				checkL = true;
				rb.velocity = Vector3.zero;
			}
		 }
		 if (!checkF && checkL) {
		 	 fTime -= Time.deltaTime;
			 if (fTime < 0) {
			 	 fTime = oTimeF;
				 checkF = true;
				 
			 }
		 }
		 if (checkF && checkL) {
			rb.velocity = (oPos - transform.position).normalized * speed;

			if(Vector3.Distance(transform.position, oPos) < 0.5f) {
				checkL = false;
				rb.velocity = Vector3.zero;
			}
		 }


     }
}
