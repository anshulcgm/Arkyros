using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectHandler
{
    public static Server server;
    private class CacheObject
    {         
        public static float maxTimeHandlingCache = 5.0f;
        public static float maxCacheRenderDist = 1000.0f;

        public string name;
        public List<Vector3> positions = new List<Vector3>();
        public List<Quaternion> orientations = new List<Quaternion>();
        public List<GameObject> objs = new List<GameObject>();
        public int numInCache = 1000;
        public GameObject resource = null;
        public CacheObject(string name, List<Vector3> positions, List<Quaternion> orientations, GameObject resource)
        {
            this.name = name;
            this.positions.AddRange(positions);
            this.orientations.AddRange(orientations);
            this.resource = resource;
        }
    }
    static List<CacheObject> cachedObjects = new List<CacheObject>();

    static int currCachedObjIndex = 0;
    static int currCache = 0;
    static int n = 0;
    public static void HandleCachedObjects(Camera cam)
    {
        DateTime start = DateTime.Now;
        if(cachedObjects.Count == 0)
        {
            return;
        }
        
        for(int i = currCachedObjIndex; i < cachedObjects[currCache].positions.Count; i++)
        {
            if(n == cachedObjects[currCache].numInCache)
            {
                break;
            }
            if(Vector3.SqrMagnitude(cachedObjects[currCache].positions[i] - cam.transform.position) < CacheObject.maxCacheRenderDist * CacheObject.maxCacheRenderDist)
            {
                cachedObjects[currCache].objs[n].transform.position = cachedObjects[currCache].positions[i];
                cachedObjects[currCache].objs[n].transform.rotation = cachedObjects[currCache].orientations[i];
                n++;
            }
            if ((DateTime.Now - start).TotalMilliseconds > CacheObject.maxTimeHandlingCache)
            {
                currCachedObjIndex = i + 1;
                return;
            }
        }
        currCachedObjIndex = 0;        
        n = 0;
        currCache++;
        if (currCache >= cachedObjects.Count)
        {
            currCache = 0;
        }
    }
    
    /* Parameters: 
     * o: list of changes to make to the gameObject, as well as new gameObjects to instantiate and behaviors to attatch to the new gameObjects.
     * gameObject: the gameObject to change
     * 
     * Description:
     * changes the gameObject as specified by 'o', instantiates new objects as specified by 'o'.
     */
    public static void Update(ObjectUpdate o, GameObject gameObject)
    {
        //handling all the instantiation requests
        foreach(InstantiationRequest i in o.instatiationRequests)
        {
            GameObject resource = (GameObject)Resources.Load(i.resourcePath);
            if (i.isCache)
            {
                bool existsInCache = false;
                foreach(CacheObject c in cachedObjects)
                {
                    if (c.name.Equals(i.resourcePath))
                    {
                        c.positions.Add(i.position);
                        c.orientations.Add(i.orientation);
                        existsInCache = true;
                        break;
                    }
                }
                if (!existsInCache)
                {
                    Debug.Log("making new cache " + i.resourcePath);
                    CacheObject newCache = new CacheObject(i.resourcePath, new List<Vector3> { i.position }, new List<Quaternion> { i.orientation }, resource);
                    for (int i1 = 0; i1 < newCache.numInCache; i1++)
                    {
                        newCache.objs.Add(UnityEngine.Object.Instantiate(resource, new Vector3(100000,100000,100000), Quaternion.Euler(Vector3.zero)));
                    }
                    cachedObjects.Add(newCache);
                }
                continue;
            }

            foreach (IClass c in i.behaviorsToAdd)
            {
                if (c == null)
                {
                    throw new Exception("gameObjectID: " + gameObject.GetInstanceID() + " gameObject Name: " + gameObject.name + " Error: the class of type " + c.GetType().Name + " is null!");
                }
                IMono comp = (IMono)resource.AddComponent(c.MonoScript);
                if (comp == null)
                {
                    throw new Exception("gameObjectID: " + gameObject.GetInstanceID() + " gameObject Name: " + gameObject.name + " Error: Monobehavior " + c.MonoScript.Name + " for class " + c.GetType().Name + " does not exist, is not a Monobehavior, or does not implement the IMono interface");
                }
                comp.SetMainClass(c);
            }
            //instantiating the new gameobject with the behaviors added above
            GameObject gameObj = GameObject.Instantiate(resource, i.position, i.orientation) as GameObject;
            if(gameObj == null)
            {
                Debug.Log("gameObject is NULL!!!");
            }
            server.Create(gameObj, i.resourcePath);
        }

        //setting all of the values here

        //set position and orientation
        if(o.position != null)
        {
            gameObject.transform.position = o.position.GetVector();
        }
        if(o.rotation != null)
        {
            gameObject.transform.rotation = o.rotation.GetQuaternion();
        }

        //if you're trying to set the velocity or force of the object, but there is no Rigidbody, throw an exception
        Rigidbody r = gameObject.GetComponent<Rigidbody>();
        if(o.velocity != null || o.force != null && r == null)
        {
            throw new Exception("gameObjectID: " + gameObject.GetInstanceID() + " gameObject Name: " + gameObject.name + " Error: there is no rigidbody component for this gameObject");
        }

        //set velocity and force
        if (o.velocity != null)
        {
            r.velocity = o.velocity.GetVector();
        }
        if (o.force != null)
        {
            r.AddForce(o.force.GetVector());
        }

        ///@TODO: when faces are implemented, they'll need another if statement here.
        //setting mesh
        if (o.meshPoints != null)
        {
            List<Vector3> meshPointsUnwrapped = new List<Vector3>();
            foreach (Vector3Wrapper v in o.meshPoints)
            {
                meshPointsUnwrapped.Add(v.GetVector());
            }
            if (o.triangles != null)
            {
                Mesh newMesh = MeshBuilder3D.GetMeshFrom(meshPointsUnwrapped, o.triangles);
                gameObject.GetComponent<MeshFilter>().mesh = newMesh;
                gameObject.GetComponent<MeshCollider>().sharedMesh = newMesh;
            }
        }
    }

    /* Parameters: 
     * a: the animator variable to change, and the value to change it to
     * gameObject: the gameObject to change
     * 
     * Description:
     * helper function that changes the animator variable specified by 'a' to the value specified by 'a', for the gameObject.
     */
    ///@TODO: fix animator variables setting, currently you cannot set animation states with the ObjectHandler.
    private static void SetAnimatorVariable(AnimatorVarSetRequest a, GameObject gameObject)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        
    }
}