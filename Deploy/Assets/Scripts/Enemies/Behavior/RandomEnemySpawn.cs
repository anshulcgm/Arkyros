using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Anshul Ahluwalia 
public class RandomEnemySpawn: MonoBehaviour {

    private GameObject planet;
    public GameObject flyingKamikaze;
    public GameObject golem;
    public GameObject Player;
    //public GameObject testObject;

    public int kamikazePerSpawn = 5;
    public int kamikazeSwarms = 1;

    public int golemPerSpawn = 1;
    public int golemSwarms = 1;

    private Vector3 planetCenter;
    private float planetRadius;

    bool instantiateGolem = true;

    private List<Vector3> kamikazeInstantiationPoints;
    private List<Vector3> golemInstantiationPoints;
    public void Start()
    {
       // Debug.Log("In start function of enemy spawn");
        planet = GameObject.FindGameObjectWithTag("planet");
        planetCenter = planet.transform.position;
        //Debug.DrawLine(planetCenter, new Vector3(0, 8000f, 0), Color.red, 20, false);
        planetRadius = (1.25f * planet.transform.localScale.x);
        //kamikazeInstantiation();
        
    }
    private void Update()
    {
        if (instantiateGolem)
        {
            instantiateGolems(true);
            InstantiateKamikazeNearPlayer(flyingKamikaze, 1, 3);
            instantiateGolem = false;
        }   
    }
    public Vector3 GetRandomInstantiationPointOnSphere()
    {
        return (Random.onUnitSphere * planetRadius + planetCenter);
    }
    public void kamikazeInstantiation()
    {
        kamikazeInstantiationPoints = new List<Vector3>();
        //Debug.Log("Planet transform is at " + planet.transform.position);
         //Can be any variable as all axes will have same local transform
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
        foreach (Vector3 point in kamikazeInstantiationPoints)
        {
            for (int i = 0; i < kamikazePerSpawn; i++)
            {
                Vector3 randomInstanPoint = Random.insideUnitSphere * 30.0f + point; //Instantiates enemies within 30 meter radius of original instantiation Point 
                GameObject enemy = (GameObject)Instantiate(flyingKamikaze, randomInstanPoint, Quaternion.identity);
                InstantiationRequest instanRequest = new InstantiationRequest("KamikaziBird", randomInstanPoint, Quaternion.identity, false);
                o.AddInstantiationRequest(instanRequest);

            }
        }
        ObjectHandler.Update(o, this.gameObject);
    }
    public void instantiateGolems(bool instantiateNearPlayer)
    {
        golemInstantiationPoints = new List<Vector3>();
        for(int i = 0; i < golemSwarms; i++)
        {
            Vector3 basePoint = Vector3.zero;
            if (instantiateNearPlayer)
            {
                basePoint = Player.transform.position + Random.insideUnitSphere * 5f + new Vector3(0, 10f, 0);
            }
            else
            {
                basePoint = GetRandomInstantiationPointOnSphere();
            }
            //Instantiate(testObject, basePoint, Quaternion.identity);
         
            //Debug.Log("Base Point is " + basePoint);
            Vector3 raycastDirection = (planetCenter - basePoint).normalized;
            //Debug.Log("Direction to planet is " + raycastDirection);
            ////Debug.DrawLine(basePoint, planetCenter, Color.red, 100f, false);
            //Debug.DrawRay(basePoint, raycastDirection, Color.green, 200, false);
            RaycastHit hit;
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
    public static void spawnEnemyWithinRadius(GameObject enemy, float radius, Vector3 spawnPos, float maxHPProportion)
    {
        if(enemy.name == "KamikaziBirdShort")
        {
           Vector3 instantiationPoint = Random.insideUnitSphere * radius + spawnPos + new Vector3(0, 30f, 0);
           ObjectUpdate o = new ObjectUpdate();
           GameObject bird = Instantiate(enemy, instantiationPoint, Quaternion.identity);
            bird.GetComponent<StatManager>().kamikazeMaxHP *= maxHPProportion;
            InstantiationRequest instanRequest = new InstantiationRequest("KamikaziBird", instantiationPoint, Quaternion.identity, false);
            o.AddInstantiationRequest(instanRequest);
        }
        else if(enemy.name == "GolemParent")
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
            GameObject golem = Instantiate(enemy, instanPoint, Quaternion.identity);
            golem.GetComponent<StatManager>().golemMaxHp *= maxHPProportion;
        }
    }
}
