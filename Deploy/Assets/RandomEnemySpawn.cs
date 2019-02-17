using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Anshul Ahluwalia 
public class RandomEnemySpawn : MonoBehaviour {

    private GameObject planet;
    public GameObject enemyToInstanstiate;
    public int enemiesPerSpawn = 5;
    public int numberOfSwarms = 3;
    private Vector3 planetCenter;
    private float planetRadius;

    private List<Vector3> enemyInstantiationPoints; 

    [RuntimeInitializeOnLoadMethod]
    private void Start()
    {
        planet = GameObject.FindGameObjectWithTag("planet");
        enemyInstantiationPoints = new List<Vector3>();
        //Debug.Log("Planet transform is at " + planet.transform.position);
        planetCenter = planet.transform.position;
        planetRadius = (planet.transform.localScale.x) / 2; //Can be any variable as all axes will have same local transform
        for(int i = 0; i < numberOfSwarms; i++)
        {
            Vector3 instantiationPoint = GetRandomInstantiationPointOnSphere();
            while (enemyInstantiationPoints.Contains(instantiationPoint)) //Checks to make sure point isn't already in the list
            {
                instantiationPoint = GetRandomInstantiationPointOnSphere();
            }
            enemyInstantiationPoints.Add(instantiationPoint);
        }

        foreach(Vector3 point in enemyInstantiationPoints)
        {
            Quaternion randomRotation = Random.rotation;
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                Vector3 randomInstanPoint = Random.insideUnitSphere * 4.5f + point; //Instantiates enemies within 4.5 meter radius of original instantiation Point 
                GameObject enemy = (GameObject)Instantiate(enemyToInstanstiate, randomInstanPoint, randomRotation);
            }
        }


    }

    public Vector3 GetRandomInstantiationPointOnSphere()
    {
        return (Random.onUnitSphere * planetRadius + planetCenter);
    }
}
