using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Anshul Ahluwalia 
public class RandomEnemySpawn: MonoBehaviour {

    private GameObject planet;
    public GameObject flyingKamikaze;
    public GameObject golem;
    public GameObject testPlayer;
    public GameObject testObject;

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
            instantiateGolems();
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
                Enemy e = new Enemy(EnemyType.FlyingKamikaze, 50, 10, enemy);
                Enemy.enemyList.Add(e);
                InstantiationRequest instanRequest = new InstantiationRequest("KamikaziBird", randomInstanPoint, Quaternion.identity, false);
                o.AddInstantiationRequest(instanRequest);

            }
        }
        ObjectHandler.Update(o, this.gameObject);
    }
    public void instantiateGolems()
    {
        golemInstantiationPoints = new List<Vector3>();
        for(int i = 0; i < golemSwarms; i++)
        {

            Vector3 basePoint = GetRandomInstantiationPointOnSphere(); 
            Instantiate(testObject, basePoint, Quaternion.identity);
         
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
    public void InstantiationNearPlayer()
    {
        kamikazeInstantiationPoints = new List<Vector3>();
        for(int i = 0; i < kamikazeSwarms; i++)
        {
            Vector3 instantiationPoint = testPlayer.transform.position + Random.insideUnitSphere * 100f + new Vector3(500f, 200f, 500f);
            kamikazeInstantiationPoints.Add(instantiationPoint);
        }
        foreach(Vector3 point in kamikazeInstantiationPoints)
        {
            for(int i = 0; i < kamikazePerSpawn; i++)
            {
                Vector3 randomPoint = Random.insideUnitSphere * 15.0f + point;
                GameObject enemy = (GameObject)Instantiate(flyingKamikaze, randomPoint, Quaternion.identity);
                enemy.transform.LookAt(testPlayer.transform);
            }
        }
    }
}
