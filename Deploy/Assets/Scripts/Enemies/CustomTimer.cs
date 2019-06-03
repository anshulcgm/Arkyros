using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTimer : MonoBehaviour
{
    public GameObject shly;
    public GameObject kamikaze;
    public GameObject starship;

    public GameObject shrab1;
    public GameObject shrab2;
    public GameObject shrab3;
    public GameObject shrab4;

    public GameObject golem;

    public GameObject player;

    public GameObject shrabPrefab;
    public GameObject golemPrefab;
    public GameObject shlyPrefab;
    public GameObject kamikazePrefab; 

    public float shlyTimer;
    public float kamikazeTimer;
    public float shrabTimer;
    public float golemTimer;

    public float oShlyTimer;
    public float oKamikazeTimer;
    public float oShrabTimer;
    public float oGolemTimer;

    /*
    public float shrab1Timer;
    public float shrab2Timer;
    public float shrab3Timer;
    public float shrab4Timer;
    */
    private bool activateShly = true;
    private bool activateKamikaze = true;
    private bool activateStarship = true;
    private bool activateShrab = true; 
    //private bool activateShrab1 = true;
    //private bool activateShrab2 = true;
    //private bool activateShrab3 = true;
    //private bool activateShrab4 = true;

    private bool activateGolem = true; 

    private float oStarshipTimer;

    private Vector3 starshipInstantionPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        //starshipInstantionPoint = new Vector3(879.4f, 4007.4f, 376.5f);
        //oStarshipTimer = starshipTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Assumes that the timers are in the order presented above
        /*
        shlyTimer -= Time.deltaTime;
        if(shlyTimer <= 0 && activateShly)
        {
            shly.SetActive(true);
            activateShly = false;
        }

        kamikazeTimer -= Time.deltaTime;
        if (kamikazeTimer <= 0 && activateKamikaze)
        {
            kamikaze.SetActive(true);
            //kamikaze.GetComponent<PlayerAttack>().enabled = true;
            activateKamikaze = false;
        }

        starshipTimer -= Time.deltaTime;
        if(starshipTimer <= 0 && activateStarship)
        {
            Instantiate(starship, starshipInstantionPoint, Quaternion.identity);
            starshipTimer = oStarshipTimer;
            //starship.GetComponent<StarshipEndgame>().enabled = true;
            //activateStarship = false; 
        }
        shrab1Timer -= Time.deltaTime;
        if (shrab1Timer <= 0 && activateShrab1)
        {
            shrab1.SetActive(true);
            activateShrab1 = false;
        }

        shrab2Timer -= Time.deltaTime;
        if (shrab2Timer <= 0 && activateShrab2)
        {
            shrab2.SetActive(true);
            activateShrab2 = false;
        }

        shrab3Timer -= Time.deltaTime;
        if (shrab3Timer <= 0 && activateShrab3)
        {
            shrab3.SetActive(true);
            activateShrab3 = false; 
        }

        shrab4Timer -= Time.deltaTime;
        if (shrab4Timer <= 0 && activateShrab4)
        {
            shrab4.SetActive(true);
            activateShrab4 = false; 
        }

        golemTimer -= Time.deltaTime;
        if(golemTimer <= 0 && activateGolem)
        {
            golem.SetActive(true);
            activateGolem = false;

        }
        */
        shlyTimer -= Time.deltaTime;
        if(shlyTimer <= 0 && activateShly)
        {
            for(int i = 1; i <= 3; i++)
            {
                RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.Shly, shlyPrefab, 20.0f, player.transform.position + new Vector3(0f, 30f, 0f), 1.0f);
            }
            shlyTimer = oShlyTimer;
        }
        shrabTimer -= Time.deltaTime;
        if(shrabTimer <= 0 && activateShrab)
        {
            for(int i = 1; i <= 10; i++)
            {
                RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.Shrab, shrabPrefab, 20.0f, player.transform.position + new Vector3(0f, 30f, 0f), 1.0f);
            }
            shrabTimer = oShrabTimer; 
        }
        kamikazeTimer -= Time.deltaTime;
        if(kamikazeTimer <= 0 && activateKamikaze)
        {
            for(int i = 1; i <= 3; i++)
            {
                RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.FlyingKamikaze, kamikazePrefab, 20f, player.transform.position + new Vector3(0f, 30f, 0f), 1.0f);
            }
            kamikazeTimer = oKamikazeTimer;

        }
        golemTimer -= Time.deltaTime;
        if (golemTimer <= 0 && activateGolem)
        {
            for(int i = 1; i <= 4; i++)
            {
                RandomEnemySpawn.spawnEnemyWithinRadius(EnemyType.Brawler, golemPrefab, 20f, player.transform.position + new Vector3(0f, 30f, 0f), 1.0f);
            }
            golemTimer = oGolemTimer;
        }
    }
}
