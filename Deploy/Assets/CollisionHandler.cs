using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject emptyCollider;

    public List<Transform> transforms = new List<Transform>();
    public List<int> closestPoints = new List<int>();

    public List<EntityGenerator> generators = new List<EntityGenerator>();
    public List<List<StaticEntity>[]> staticEntities = new List<List<StaticEntity>[]>();
    public List<List<GameObject[]>> emptyColliders = new List<List<GameObject[]>>();

    private static List<Vector3> points = null;
    private static List<int>[] map = null;

    public static bool initialized = false;


    
    public static int numObjectsCache = 5;

    public static void Initialize(List<Vector3> points, List<int>[] map){
        CollisionHandler.points = points;
        CollisionHandler.map = map;
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < transforms.Count; i++){
            //get the new closest point
            int currentClosestPoint = closestPoints[i];
            int leastDistPoint = currentClosestPoint;
            float leastSqrDist = Vector3.SqrMagnitude(points[currentClosestPoint] - 
                                                        transforms[i].position);
            foreach(int possibleNewClosest in map[currentClosestPoint]){
                float newSqrDist = Vector3.SqrMagnitude(points[possibleNewClosest] - transforms[i].position);
                if(newSqrDist < leastSqrDist){
                    leastDistPoint = possibleNewClosest;
                    leastSqrDist = newSqrDist;
                }
            }
            closestPoints[i] = leastDistPoint;
            List<StaticEntity[]> closestEntitiesOfType = new List<StaticEntity[]>();
            foreach(List<StaticEntity>[] entities in staticEntities){
                StaticEntity[] closestEntities = new StaticEntity[numObjectsCache];
                float closestEntitySqrDist = float.MaxValue;             
                if(entities[closestPoints[i]] != null){
                    entities[closestPoints[i]].Sort((p1, p2) => -Vector3.SqrMagnitude(p1.position - transforms[i].position).CompareTo(Vector3.SqrMagnitude(p2.position - transforms[i].position)));
                    for(int i1 = 0; i1 < numObjectsCache; i1++){
                        if(i1 >= entities[closestPoints[i]].Count){
                            break;
                        }
                        closestEntities[i1] = entities[closestPoints[i]][i1];
                    }
                }
                closestEntitiesOfType.Add(closestEntities);
            }

            for(int i1 = 0; i1 < closestEntitiesOfType.Count; i1++){
                for(int a = 0; a < numObjectsCache; a++){
                    if(closestEntitiesOfType[i1][a] != null){
                        emptyColliders[i1][i][a].transform.position = closestEntitiesOfType[i1][a].position;
                        emptyColliders[i1][i][a].transform.rotation = closestEntitiesOfType[i1][a].rotation;
                        emptyColliders[i1][i][a].transform.localScale = closestEntitiesOfType[i1][a].scale;
                    }
                }
            }
        }
    }

    public void AddTransform(Transform t){
        float leastSqrDist = float.MaxValue;
        int closestPoint = -1;
        for(int i = 0; i < points.Count; i++){
            float sqrDist = Vector3.SqrMagnitude(t.position - points[i]);
            if(sqrDist < leastSqrDist){
                closestPoint = i;
                leastSqrDist = sqrDist;
            }
        }
        transforms.Add(t);
        for(int i = 0; i < emptyColliders.Count; i++){
            GameObject[] colliders = new GameObject[numObjectsCache];
            for(int i1 = 0; i1 < numObjectsCache; i1++){
                GameObject collider = Instantiate(emptyCollider, new Vector3(10000.0f, 10000.0f, 10000.0f), Quaternion.identity);
                collider.GetComponent<MeshCollider>().sharedMesh = generators[i].entityMesh;
                colliders[i1] = collider;
            }
            emptyColliders[i].Add(colliders);
        }
        closestPoints.Add(closestPoint);    
    }

    public void AddObject(EntityGenerator e, int closestPoint, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        int index = generators.IndexOf(e);
        if(index == -1){
            int staticEntityIndex = staticEntities.Count;
            generators.Add(e);
            Debug.Log(staticEntities);
            List<StaticEntity>[] entities = new List<StaticEntity>[points.Count];
            staticEntities.Add(entities);            
            emptyColliders.Add(new List<GameObject[]>());
            staticEntities[staticEntityIndex][closestPoint] = new List<StaticEntity>();
            staticEntities[staticEntityIndex][closestPoint].Add(new StaticEntity {closestPoint = closestPoint, position = position, rotation = rotation, scale = scale});
        }
        else{
            if(staticEntities[index][closestPoint] == null){
                staticEntities[index][closestPoint] = new List<StaticEntity>();
            }
            staticEntities[index][closestPoint].Add(new StaticEntity {closestPoint = closestPoint, position = position, rotation = rotation, scale = scale});
        }
    }
}

public class StaticEntity
{
    public int closestPoint;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}
