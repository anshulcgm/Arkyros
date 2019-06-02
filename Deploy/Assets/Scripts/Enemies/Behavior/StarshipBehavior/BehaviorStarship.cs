using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorStarship : MonoBehaviour
{
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
    private float rayTimer;
    public GameObject ChargedRay;
    public float maxLength;
    //button, shortened to butt - Toma
    private bool butt;

    void Start()
    {
        //flyinglanding start
        rb = GetComponent<Rigidbody>();

        oPos = transform.position;

        checkL = false;
        checkF = true;

        oTimeL = lTime;
        oTimeF = fTime;

        //endgame start
        /*
		endgameCheck = true;
        endgamePrefab = transform.GetChild(0).gameObject;
        rend = GetComponent<Renderer>();
        rend.material = starshipMaterial;
		*/
        //charged ray start
        ChargedRay = this.gameObject.transform.GetChild(0).gameObject;
        ChargedRay.transform.position = this.transform.position;
        butt = false;
        rayTimer = Random.Range(1.0f, 5.0f);


        //make low health
        //GetComponent<StatManager>().changeHealth(-96);

    }
    void Update()
    {
        /*
		if(GetComponent<StatManager>().ship.enemyStats.getHealth() < 0.05f * GetComponent<StatManager>().ship.enemyStats.getMaxHealth() ) {
			endgame();
			//Debug.Log("endgame is called");
			
		}
		*/
        //the timer runs
        rayTimer -= Time.deltaTime;
        if (rayTimer <= 0)
        {
            //when timer hits zero, switch the butt so it will oscillate between on and off
            butt = !butt;
            rayTimer = Random.Range(1.0f, 5.0f);
            chargedRay(butt);
        }



        flyingLanding();
    }

    void flyingLanding()
    {
        RaycastHit hit;

        if (checkF && !checkL)
        {
            lTime -= Time.deltaTime;
            if (lTime < 0)
            {
                checkF = false;
                lTime = oTimeL;

            }
        }
        if (!checkF && !checkL && Physics.Raycast(transform.position, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
        {


            rb.velocity = (planet.transform.position - transform.position).normalized * speed;

            if (Vector3.Distance(transform.position, hit.point + (transform.position - planet.transform.position).normalized * height) < 0.5f)
            {
                checkL = true;
                rb.velocity = Vector3.zero;
            }
        }
        if (!checkF && checkL)
        {
            fTime -= Time.deltaTime;
            if (fTime < 0)
            {
                fTime = oTimeF;
                checkF = true;

            }
        }
        if (checkF && checkL)
        {
            rb.velocity = (oPos - transform.position).normalized * speed;

            if (Vector3.Distance(transform.position, oPos) < 0.5f)
            {
                checkL = false;
                rb.velocity = Vector3.zero;
            }
        }

    }
    void chargedRay(bool button)
    {

        RaycastHit hit;
        LineRenderer LR = ChargedRay.GetComponent<LineRenderer>();
        LR.useWorldSpace = true;
        LR.SetPosition(0, transform.position);
        LR.SetPosition(1, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z));
        //raycast(origin, 0,-1,0...)
        callLaser(button);
        if (button)
        {
            LR.SetPosition(0, transform.position);

            if (Physics.Raycast(transform.position, (LR.GetPosition(1) - transform.position).normalized, out hit, Mathf.Infinity))
            {
                //if (hit.collider)
                //{ Vector3.down * Vector3.Distance(hit.point, transform.position)
                LR.SetPosition(1, hit.point);
                Debug.Log(hit.point);
                //}
            }
            else
            {
                LR.SetPosition(1, transform.position);
            }
        }


    }

    //to turn the laser on and off
    public void callLaser(bool button)
    {
        ChargedRay.gameObject.SetActive(button);
    }

    void endgame()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            rend.material.color = Color.Lerp(starshipMaterial.color, Color.red, Time.time / timer);
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
    void spawn()
    {
        //anshul late
    }
}