using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Anshul Ahluwalia 
public class RandomEnemySpawn: MonoBehaviour {

    private GameObject planet;

    public GameObject flyingKamikaze;
    public GameObject golem;
    public GameObject IREnemy;
    public GameObject shrab;

    public GameObject shly;
    public GameObject threeShly;
    public GameObject sixShly;

    public GameObject Player;
    

    public int kamikazePerSpawn = 5;
    public int kamikazeSwarms = 1;

    public int golemPerSpawn = 1;
    public int golemSwarms = 1;

    public int numIREnemies;

    public int numShlyBatchesPerIREnemy; //A batch constitutes of 1 of each type of shly
    public float shlySpawnRadius; 

    private Vector3 planetCenter;
    private float planetRadius;

    bool instantiateGolem = true;

    //These instance variables keep track of the various instantiation points for the enemies
    private List<Vector3> kamikazeInstantiationPoints;
    private List<Vector3> golemInstantiationPoints;
    private List<Vector3> IREnemies;

    public void Start()
    {
        //Receives information about the planet
        planet = GameObject.FindGameObjectWithTag("planet");
        planetCenter = planet.transform.position;
        planetRadius = (1.25f * planet.transform.localScale.x);

        //Instantiates enemies
        instantiateGolems(false);
        InstantiateKamikazeNearPlayer(flyingKamikaze, 1, 3); //alternatively use kamikazeInstantiation()
        instantiateIREnemy();
    }
    public Vector3 GetRandomInstantiationPointOnSphere()
    {
        return (Random.onUnitSphere * planetRadius + planetCenter);
    }
    public void kamikazeInstantiation()
    {
        kamikazeInstantiationPoints = new List<Vector3>();

        //THis for loop gets a separate instantiation point for each kamikaze swarm
        for (int i = 0; i < kamikazeSwarms; i++)
        {
            Vector3 instantiationPoint = GetRandomInstantiationPointOnSphere();

            while (kamikazeInstantiationPoints.Contains(instantiationPoint)) //Checks to make sure point isn't already in the list
            {
                instantiationPoint = GetRandomInstantiationPointOnSphere();
            }
            kamikazeInstantiationPoints.Add(instantiationPoint);
        }
        ObjectUpdate o = new ObjectUpdate();

        //This for loop instantiates a group of kamikazes for each instantiation point received above and places them within a certain radius of each other
        foreach (Vector3 point in kamikazeInstantiationPoints)
        {
            for (int i = 0; i < kamikazePerSpawn; i++)
            {
                Vector3 randomInstanPoint = Random.insideUnitSphere * 5.0f + point;  
                GameObject enemy = (GameObject)Instantiate(flyingKamikaze, randomInstanPoint, Quaternion.identity);
                InstantiationRequest instanRequest = new InstantiationRequest("KamikaziBird", randomInstanPoint, Quaternion.identity, false); //Call to server
                o.AddInstantiationRequest(instanRequest);

            }
        }
        ObjectHandler.Update(o, this.gameObject);
    }

    //Instantiates golems either near the player or randomly around the map
    public void instantiateGolems(bool instantiateNearPlayer)
    {
        golemInstantiationPoints = new List<Vector3>();

        
        for(int i = 0; i < golemSwarms; i++)
        {
            //Base point is a random point on the sphere but may not be exactly touching the surface
            Vector3 basePoint = Vector3.zero;
            if (instantiateNearPlayer)
            {
                basePoint = Player.transform.position + Random.insideUnitSphere * 5f + new Vector3(0, 10f, 0); //Random base-point
            }
            else
            {
                basePoint = GetRandomInstantiationPointOnSphere(); //Base-point close to the player
            }
           
            Vector3 raycastDirection = (planetCenter - basePoint).normalized;
            RaycastHit hit;
            //Raycasts from the basepoint to the planet to find a spot on the planet's surface to instantiate the golem on
            if(Physics.Raycast(basePoint, raycastDirection, out hit, Mathf.Infinity))
            {
                Vector3 instantiationPoint = hit.point;
                golemInstantiationPoints.Add(instantiationPoint);
                Debug.Log("Instantiation Point found at " + instantiationPoint);
                Debug.Log("Raycast hit " + hit.collider.gameObject.name);
                
            }
            else
            {
                Debug.Log("Raycast hit not detected");
            }
        }

        foreach(Vector3 point in golemInstantiationPoints)
        {
            for(int i = 0; i < golemPerSpawn; i++)
            {
                Instantiate(golem, point, Quaternion.identity);
            }
        }
    }

    public void instantiateIREnemy()
    {
        IREnemies = new List<Vector3>();
        for(int i = 0; i < numIREnemies; i++)
        {
            Vector3 basePoint = GetRandomInstantiationPointOnSphere();

            Vector3 raycastDirection = (planetCenter - basePoint).normalized;
            RaycastHit hit; 

            if(Physics.Raycast(basePoint, raycastDirection, out hit, Mathf.Infinity))
            {
                Vector3 instantiationPoint = hit.point;
                IREnemies.Add(instantiationPoint);
            }
        }

        foreach(Vector3 point in IREnemies)
        {
            Instantiate(IREnemy, point, Quaternion.identity);
        }
    }

    //Method is not in use right now, but could be used in the future
    public GameObject findRaycastPointOnSphere(Vector3 startingPosition)
    {
        RaycastHit hit;
        GameObject attachedObject;
        if(Physics.Raycast(startingPosition, (planetCenter - startingPosition).normalized, out hit, Mathf.Infinity))
        {
            attachedObject = hit.collider.gameObject;
            return attachedObject;
        }
        return null;
        
    }

    //Code is very similar to the method above, but it just instantiates the kamikazes close to the player for testing purposes
    public void InstantiateKamikazeNearPlayer(GameObject toInstantiate, int swarms, int numPerSwarm)
    {
        List<Vector3> InstantiationPoints = new List<Vector3>();
        for(int i = 0; i < swarms; i++)
        {
            Vector3 instantiationPoint = Player.transform.position + Random.insideUnitSphere * 500f;
            InstantiationPoints.Add(instantiationPoint);
        }
        foreach(Vector3 point in InstantiationPoints)
        {
            for(int i = 0; i < numPerSwarm; i++)
            {
                Vector3 randomPoint = Random.insideUnitSphere * 5.0f + point;
                Instantiate(toInstantiate, randomPoint, Quaternion.identity);
            
            }
        }
    }

    public void instantiateShlies()
    {
         foreach(Vector3 point in IREnemies)
         {
            for(int i = 0; i < numShlyBatchesPerIREnemy; i++)
            {
                spawnEnemyWithinRadius(EnemyType.Shly, shly, shlySpawnRadius, point, 1f);
                spawnEnemyWithinRadius(EnemyType.Shly, threeShly, shlySpawnRadius, point, 1f);
                spawnEnemyWithinRadius(EnemyType.Shly, sixShly, shlySpawnRadius, point, 1f);
            }
         }
    }

    //This method is for IRenemies so that they can spawn squishy enemies to defend themselves, the squishy enemies will likely not be at full health
    public static void spawnEnemyWithinRadius(EnemyType type,GameObject enemy, float radius, Vector3 spawnPos, float maxHPProportion)
    {
        //Add code to adjust shrab and shly health 
          Debug.Log("In RandomEnemySpawn function");
        if(type == EnemyType.FlyingKamikaze || type == EnemyType.Shly)
        {
            
           Vector3 instantiationPoint = Random.insideUnitSphere * radius + spawnPos + new Vector3(0, 30f, 0);
           ObjectUpdate o = new ObjectUpdate();
           GameObject bird = Instantiate(enemy, instantiationPoint, Quaternion.identity);
            if(type == EnemyType.FlyingKamikaze)
            {
                bird.GetComponent<StatManager>().kamikazeMaxHP *= maxHPProportion;
            }
            
            InstantiationRequest instanRequest = new InstantiationRequest("KamikaziBird", instantiationPoint, Quaternion.identity, false);
            o.AddInstantiationRequest(instanRequest);
        }
        else if(type == EnemyType.Brawler || type == EnemyType.Shrab)
        {
            Vector3 basePoint = Random.insideUnitSphere * radius + spawnPos;
            Vector3 instanPoint = Vector3.zero;
            Vector3 raycastDirection = (- basePoint).normalized;
            RaycastHit hit;
            if (Physics.Raycast(basePoint, raycastDirection, out hit, Mathf.Infinity))
            {
                instanPoint = hit.point;
            }
            else
            {
                Debug.Log("Raycast hit not detected");
            }
            if(type == EnemyType.Brawler)
            {
                GameObject golem = Instantiate(enemy, instanPoint, Quaternion.identity);
                golem.GetComponent<StatManager>().golemMaxHp *= maxHPProportion;
            }
        }
        
    }
}
