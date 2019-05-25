using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorStarship : MonoBehaviour {
	//flyinglanding fields
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
	
	//endgame fields
	private Renderer rend;
   	private bool endgameCheck;
    private GameObject endgamePrefab;
	private bool EndgameAlreadyHappened;

    	public float timer;

    	public Material starshipMaterial;
	
	//charged ray fields
	public float rayTimer;
	private float oRayTimer;
	

	void Start() {
		//flyinglanding start
		rb = GetComponent<Rigidbody>();

		oPos = transform.position;

		checkL = false;
		checkF = true;

		oTimeL = lTime;
		oTimeF = fTime;
		
		//endgame start
		endgameCheck = true;
        endgamePrefab = transform.GetChild(0).gameObject;
        rend = GetComponent<Renderer>();
        rend.material = starshipMaterial;
		
		//charged ray start
		oRayTimer = rayTimer;

		//make low health
		GetComponent<StatManager>().changeHealth(-96);
	}
	void Update() {
		
		if(GetComponent<StatManager>().ship.enemyStats.getHealth() < 0.05f * GetComponent<StatManager>().ship.enemyStats.getMaxHealth() ) {
			endgame();
			Debug.Log("endgame is called");
			
		}
		
		if (Random.Range(1.0f, 10.0f) <= 4.0f) {
			rayTimer -= Time.deltaTime;
			if (rayTimer > 0) {
				chargedRay();
			} else {
				rayTimer = oRayTimer;
			}
		}
		
		flyingLanding();
	}
	
	void flyingLanding() {
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
	void chargedRay() {
		//toma late
	}
	void endgame() {
		
		if (timer > 0)
        	{
	            timer -= Time.deltaTime;
        	    rend.material.color = Color.Lerp(starshipMaterial.color, Color.red, Time.time/timer);
				Debug.Log("timer > 0");
        	}
        	else
        	{
            		if (endgameCheck)
            		{
						Debug.Log("endgame happened");
                		endgamePrefab.SetActive(true);
                		endgameCheck = false;
            		}
        	}
	}
	void spawn() {
		//anshul late
	}
}