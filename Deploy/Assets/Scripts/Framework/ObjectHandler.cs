using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectHandler
{
    public static Server server;
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
            //putting in each of the behaviors
            GameObject resource = (GameObject)Resources.Load(i.resourcePath);
            foreach (IClass c in i.behaviorsToAdd)
            {
                if(c == null)
                {
                    throw new Exception("gameObjectID: " + gameObject.GetInstanceID() + " gameObject Name: " + gameObject.name + " Error: the class of type " + c.GetType().Name + " is null!");
                }
                if (resource.GetComponent(c.MonoScript))
                {
                    UnityEngine.Object.Destroy(resource.GetComponent(c.MonoScript));
                }
                IMono comp = (IMono)resource.AddComponent(c.MonoScript);
                if(comp == null)
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