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

    public bool CheckIfTriExists(List<int>[][] trianglesStored, List<int>[] map, int[] abc)
    {
        for(int i = 0; i < 3; i++)
        {
            if(trianglesStored[abc[i]] == null)
            {
                continue;
            }
            for(int i1 = 0; i1 < trianglesStored[abc[i]].Length; i1++)
            {
                if (trianglesStored[abc[i]][i1] == null)
                {
                    continue;
                }
                for (int i2 = 0; i2 < trianglesStored[abc[i]][i1].Count; i2++)
                {
                    int a = abc[i];
                    int b = map[a][i1];
                    int c = trianglesStored[abc[i]][i1][i2];
                    if(abc.Contains(a) && abc.Contains(b) && abc.Contains(c))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void AssignTri(int[] abc, List<int>[] map, List<int>[][] trianglesHash, int triangle)
    {
        for(int i = 0; i < 3; i++)
        {
            int a = abc[i];
            if(trianglesHash[a] == null)
            {
                trianglesHash[a] = new List<int>[map[a].Count];
            }
            int indexB = -1;
            int indexC = -1;
            for(int i1 = 0; i1 < map[a].Count; i1++)
            {
                if(map[a][i1] == abc[(i + 1) % 3])
                {
                    indexB = i1;
                }
                if (map[a][i1] == abc[(i + 2) % 3])
                {
                    indexC = i1;
                }
            }

            if (trianglesHash[a][indexB] == null)
            {
                trianglesHash[a][indexB] = new List<int>();
            }
            if (trianglesHash[a][indexC] == null)
            {
                trianglesHash[a][indexC] = new List<int>();
            }
            trianglesHash[a][indexB].Add(triangle);
            trianglesHash[a][indexC].Add(triangle);
        }
    }
    
    public List<Triangle> GetTrianglesFromConnections(List<Vector3> points, List<int>[] map, out List<int>[][] trianglesHash)
    {
        DateTime start = DateTime.Now;
        trianglesHash = new List<int>[points.Count][];
        List<Triangle> triangles = new List<Triangle>();

        for (int i = 0; i < map.Length; i++)
        {
            foreach(int conn1 in map[i])
            {
                foreach (int conn2 in map[i])
                {
                    if(conn1 == conn2)
                    {
                        continue;
                    }
                    if (map[conn1].Contains(conn2) && !CheckIfTriExists(trianglesHash, map, new int[] { i, conn1, conn2 }))
                    {
                        Plane plane = new Plane(points[i], points[conn1], points[conn2]);
                        AssignTri(new int[] { i, conn1, conn2 }, map, trianglesHash, triangles.Count);
                        Triangle t = new Triangle(new int[] { i, conn1, conn2 }, plane, BlockingDirection.NONE);
                        triangles.Add(t);
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

    public static List<int>[] GetMap(List<int[]> cons)
    {
        int max = 0;
        foreach (int[] con in cons)
        {
            if (con[0] > max)
            {
                max = con[0];
            }
            if (con[1] > max)
            {
                max = con[1];
            }
        }

        List<int>[] map = new List<int>[max + 1];
        foreach (int[] con in cons)
        {
            if(map[con[0]] == null)
            {
                map[con[0]] = new List<int>();
            }
            if (map[con[1]] == null)
            {
                map[con[1]] = new List<int>();
            }
            map[con[0]].Add(con[1]);
            map[con[1]].Add(con[0]);
        }
        return map;
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
    public int[] triangle = null;
    public List<IClass> behaviorsToAdd = new List<IClass>();
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

    public InstantiationRequest(string resourcePath, Vector3 position, Quaternion orientation, int[] triangle)
    {
        this.resourcePath = resourcePath;
        this.orientation = orientation;
        this.position = position;
        this.triangle = triangle;
    }

    public InstantiationRequest(string resourcePath, Vector3 position, Quaternion orientation)
    {
        this.resourcePath = resourcePath;
        this.orientation = orientation;
        this.position = position;
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