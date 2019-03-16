using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ObjectUpdate
{
    //list of things to instantiate
    public List<InstantiationRequest> instatiationRequests { get; internal set; }

    //list of animator variables to change for this GameObject
    public List<AnimatorVarSetRequest> animatorVarSetRequests { get; internal set; }

    //force, velocity, position of the GameObject to update
    public Vector3Wrapper force { get; internal set; }
    public Vector3Wrapper velocity { get; internal set; }
    public Vector3Wrapper position { get; internal set; }

    //rotation of the GameObject to update
    public QuaternionWrapper rotation { get; internal set; }

    //mesh points and faces to update
    public List<Vector3Wrapper> meshPoints { get; internal set; }

    //setting connections of a mesh
    public List<int[]> triangles { get; internal set; }

    public ObjectUpdate()
    {
        instatiationRequests = new List<InstantiationRequest>();
        animatorVarSetRequests = new List<AnimatorVarSetRequest>();
        force = null;
        velocity = null;
        position = null;
        rotation = null;
        meshPoints = null;
        triangles = null;
    }

    // the following are basically just set methods for the above properties
    #region Basic GameObject Properties
    public void AddForce(Vector3 force)
    {
        if(this.force == null)
        {
            this.force = new Vector3Wrapper(force);
        }
        else
        {
            Vector3 newForce = this.force.GetVector() + force;
            this.force = new Vector3Wrapper(newForce);
        }
    }

    public void SetForce(Vector3 force)
    {
        this.force = new Vector3Wrapper(force);
    }

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = new Vector3Wrapper(velocity);
    }

    public void SetPosition(Vector3 position)
    {
        this.position = new Vector3Wrapper(position);
    }

    public void SetRotation(Quaternion rotation)
    {
        this.rotation = new QuaternionWrapper(rotation);
    }
    #endregion

    
    #region Mesh Setting

    /// TODO: make this O(N^3) operation O(N^2)
    public List<int[]> GetTrianglesFromConnections(List<Vector3> points, List<int[]> connections)
    {
        List<int[]> triangles = new List<int[]>();
        for (int i = 0; i < connections.Count; i++)
        {
            for (int i1 = 0; i1 < connections.Count; i1++)
            {
                if(i == i1) { continue; }
                for(int a = 0; a <= 1; a++)
                {
                    for (int b = 0; b <= 1; b++)
                    {
                        if (connections[i][a] == connections[i1][b])
                        {
                            int indexA = connections[i][(a + 1) % 2];
                            int indexB = connections[i1][(b + 1) % 2];
                            bool hasConnInDir = false;
                            for (int h = 0; h < connections.Count; h++)
                            {
                                if ((connections[h][0] == indexA && connections[h][1] == indexB)||(connections[h][1] == indexA && connections[h][0] == indexB))
                                {
                                    hasConnInDir = true;
                                    break;
                                }
                            }
                            if(hasConnInDir)
                            {
                                int indexC = connections[i][a];
                                int[] triangle = new int[] { indexA, indexB, indexC };
                                Array.Sort(triangle);
                                bool hasEqual = false;
                                foreach (int[] t in triangles)
                                {
                                    if (triangle.SequenceEqual(t))
                                    {
                                        hasEqual = true;
                                    }
                                }
                                if (!hasEqual)
                                {
                                    triangles.Add(triangle);
                                }
                            }                                                        
                        }
                    }
                }
            }
        }
        

        return triangles;
    }

    public void SetMeshByTriangles(List<Vector3> points, List<int[]> triangles)
    {
        meshPoints = new List<Vector3Wrapper>();
        for (int i = 0; i < points.Count; i++)
        {
            meshPoints.Add(new Vector3Wrapper(points[i]));
        }

        this.triangles = new List<int[]>();
        for (int i = 0; i < triangles.Count; i++)
        {
            if (triangles[i].Length != 3)
            {
                throw new Exception("Error: the number of indexes in the triangles array is not 3! Number of indexes: " + triangles[i].Length);
            }
            this.triangles.Add(new int[3]);
            for (int i1 = 0; i1 < 3; i1++)
            {                
                this.triangles[i][i1] = triangles[i][i1];
            }
        }
    }
    #endregion

    #region Requests
    public void AddInstantiationRequest(InstantiationRequest i)
    {
        if(i == null)
        {
            throw new Exception("Instatiation Request cannot be null");
        }
        instatiationRequests.Add(i);
    }

    public void AddAnimatorVarSetRequest(AnimatorVarSetRequest a)
    {
        if (a == null)
        {
            throw new Exception("Animator Variable Set Request cannot be null");
        }
        animatorVarSetRequests.Add(a);
    }
    #endregion
}

#region Request Objects
// this class carries information about new objects to make and the behaviors to add to each object.
public class InstantiationRequest
{
    public string resourcePath;
    public Vector3 position;
    public Quaternion orientation;
    public List<IClass> behaviorsToAdd = new List<IClass>();
    public bool isCache = false;
    public InstantiationRequest(string resourcePath, Vector3 position, Quaternion orientation, List<IClass> behaviorsToAdd)
    {
        this.resourcePath = resourcePath;
        this.orientation = orientation;
        this.position = position;
        if(behaviorsToAdd == null)
        {
            throw new Exception("the List of MonoBehaviors to add cannot be null");
        }
        this.behaviorsToAdd.AddRange(behaviorsToAdd);
    }

    public InstantiationRequest(string resourcePath, Vector3 position, Quaternion orientation, bool isCache)
    {
        this.resourcePath = resourcePath;
        this.orientation = orientation;
        this.position = position;
        this.isCache = isCache;
    }
}

// this class carries information about the animator variable to set, and the value to set it to.
public class AnimatorVarSetRequest
{
    public string varName;
    public object value;
    public AnimatorVarSetRequest(string varName, object value)
    {
        this.varName = varName;
        this.value = value;
    }
}
#endregion

#region Wrapper Objects
public class Vector3Wrapper
{
    Vector3 vector3;
    public Vector3Wrapper(Vector3 vector3)
    {
        this.vector3 = vector3;
    }
    public Vector3 GetVector()
    {
        return new Vector3(vector3.x, vector3.y, vector3.z);
    }

}
public class QuaternionWrapper
{
    Quaternion quaternion;
    public QuaternionWrapper(Quaternion quaternion)
    {
        this.quaternion = quaternion;
    }
    public Quaternion GetQuaternion()
    {
        return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
}
#endregion

public interface IMono
{
    IClass GetMainClass();
    void SetMainClass(IClass ic);
}

public interface IClass
{
    Type MonoScript { get; }
}