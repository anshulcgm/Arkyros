using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTimer : MonoBehaviour
{
    public GameObject shly;
    public GameObject kamikaze;
    public GameObject starship;


    public float shlyTimer;
    public float kamikazeTimer;
    public float starshipTimer;

    private bool activateShly = true;
    private bool activateKamikaze = true;
    private bool activateStarship = true;

    private float oStarshipTimer;

    private Vector3 starshipInstantionPoint;

    // Start is called before the first frame update
    void Start()
    {
        starshipInstantionPoint = new Vector3(879.4f, 4007.4f, 376.5f);
        oStarshipTimer = starshipTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Assumes that the timers are in the order presented above
        shlyTimer -= Time.deltaTime;
        if(shlyTimer <= 0 && activateShly)
        {
            //shly.SetActive(true);
            activateShly = false;
        }

        kamikazeTimer -= Time.deltaTime;
        if (kamikazeTimer <= 0 && activateKamikaze)
        {
            //kamikaze.SetActive(true);
            kamikaze.GetComponent<PlayerAttack>().enabled = true;
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

    }
}
