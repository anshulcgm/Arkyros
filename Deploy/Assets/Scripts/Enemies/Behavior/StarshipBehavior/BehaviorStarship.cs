using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorStarship : MonoBehaviour
{
    //flyinglanding fields
    public float speed;
    private GameObject planet;
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

    public float endgameTimer;
    private float oEndgameTimer;

    public GameObject nuke1;
    public GameObject nuke2;

    private Renderer nuke1Render;
    private Renderer nuke2Render;

    private Color initialColor;

    //charged ray fields
    private float rayTimer;
    public GameObject ChargedRay;
    public float maxLength;

    //button, shortened to butt - Toma
    private bool butt;

    private Transform instantiationPoint;

    //Enemy prefabs for spawning
    public GameObject kamikazePrefab;
    public GameObject shlyPrefab;
    public GameObject shrabPrefab;
    public GameObject golemPrefab;

    private float spawnTimer = 1.0f;

    private bool landSpawingCheck = false;

    public GameObject chargedRayParticlePrefab;

    void Start()
    {
        //flyinglanding start
        rb = GetComponent<Rigidbody>();
        //endgameRb = GetComponent<Rigidbody>(); 
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
        ChargedRay = this.gameObject.transform.GetChild(5).gameObject;
        ChargedRay.transform.position = this.transform.position;
        butt = false;
        rayTimer = Random.Range(1.0f, 5.0f);

        endgameCheck = true;
        endgamePrefab = transform.GetChild(4).gameObject;

        nuke1Render = nuke1.GetComponent<Renderer>();
        nuke2Render = nuke2.GetComponent<Renderer>();

        oEndgameTimer = endgameTimer;

        initialColor = nuke1Render.material.color;

        instantiationPoint = transform.GetChild(6);

        planet = GameObject.FindGameObjectWithTag("planet");
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
        /*
        rayTimer -= Time.deltaTime;
        if (rayTimer <= 0)
        {
            //when timer hits zero, switch the butt so it will oscillate between on and off
            butt = !butt;
            rayTimer = Random.Range(1.0f, 5.0f);
            chargedRay(butt);
        }



        flyingLanding();
        */
        flyingLanding();
        /*
        if(GetComponent<StatManager>().ship.enemyStats.getHealth() <= 0.1f * GetComponent<StatManager>().ship.enemyStats.getMaxHealth())
        {
            endgame();
        }
       */
    }

    void flyingLanding()
    {
        RaycastHit hit;

        if (checkF && !checkL)
        {
            lTime -= Time.deltaTime;
            if(lTime >= 0.5f * oTimeL)
            {
                flySpawning();
            }
            if (lTime < 0)
            {
                checkF = false;
                lTime = oTimeL;

            }
        }
        if (!checkF && !checkL && Physics.Raycast(transform.position, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
        {


            rb.velocity = (planet.transform.position - transform.position).normalized * speed;
            //endgameRb.velocity = (planet.transform.position - endgamePrefab.transform.position).normalized * speed;

            if (Vector3.Distance(transform.position, hit.point + (transform.position - planet.transform.position).normalized * height) < 0.5f)
            {
                checkL = true;
                rb.velocity = Vector3.zero;
                //endgameRb.velocity = Vector3.zero;
            }
        }
        if (!checkF && checkL)
        {
            fTime -= Time.deltaTime;
            if (fTime < 0)
            {
                chargedRay(true);
                Instantiate(chargedRayParticlePrefab, transform.position, Quaternion.Euler(new Vector3(transform.localRotation.x -90f, transform.localRotation.y, transform.localRotation.z)));
                landSpawning();
                fTime = oTimeF;
                checkF = true;

            }
        }
        if (checkF && checkL)
        {
            rb.velocity = (oPos - transform.position).normalized * speed;
            //endgameRb.velocity = (oPos - endgamePrefab.transform.position).normalized * speed;
            if (Vector3.Distance(transform.position, oPos) < 0.5f)
            {
                checkL = false;
                rb.velocity = Vector3.zero;
                //endgameRb.velocity = Vector3.zero;
            }
        }

    }
    void chargedRay(bool button)
    {

        RaycastHit hit;
       LineRenderer LR = ChargedRay.GetComponent<LineRenderer>();
        //LR.useWorldSpace = true;
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

        //Color of ship = new color(this.getcolor.red + 1, this.getcolor blue, fdbhjlfdsafjhl)
        if (endgameTimer > 0)
        {
            endgameTimer -= Time.deltaTime;
            //nuke1Render.materials[0].color = Color.Lerp(initialColor, Color.red, Time.time / timer);
            Color toSet = new Color(1/oEndgameTimer * Time.time, 0, 0);
            nuke1Render.material.SetColor("_Emission", toSet);
            nuke2Render.material.SetColor("_Emission", toSet);
            //Debug.Log("Setting emission color as " + toSet);
        }
        else
        {
            if (endgameCheck)
            {
               Rigidbody endgameRb =  endgamePrefab.AddComponent<Rigidbody>();
               endgameRb.useGravity = true; 
               endgameCheck = false;
            }
        }
    }
    public void flySpawning()
    {
        
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            /*
            int numKamikaze = (int)Random.Range(1.0f, 5.0f);
            int numShlies = (int)Random.Range(1.0f, 5.0f);

            for (int i = 1; i <= numKamikaze; i++)
            {
                RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.FlyingKamikaze, kamikazePrefab, 2.0f, instantiationPoint.position, 1.0f);
            }
            for (int i = 1; i <= numShlies; i++)
            {
                RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.Shly, shlyPrefab, 0.5f, instantiationPoint.position, 1.0f);
            }
            */
            //System.Random rand = new System.Random();
            int randomNum = (int)(Random.Range(0.0f, 1.0f) + 0.5f);
            if(randomNum == 0)
            {
                Instantiate(shlyPrefab, instantiationPoint.position, Quaternion.identity);
            }
            else if(randomNum == 1)
            {
                Instantiate(kamikazePrefab, instantiationPoint.position, Quaternion.identity);
            }
            spawnTimer = 1.0f;
        }
        
    }
    public void landSpawning()
    {
        Debug.Log("In landSpawning function");
        int numGolems = (int)(Random.Range(1, 3) + 0.5f);
        int numShrabs = 1;
        for(int i = 1; i <= numGolems; i++)
        {
            RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.Brawler, golemPrefab, 2.0f, instantiationPoint.position, 1.0f);
        }
        for(int i = 1; i <= numShrabs; i++)
        {
            RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.Shrab, shrabPrefab, 4.0f, instantiationPoint.position, 1.0f);
        }
        landSpawingCheck = false;
    }
}