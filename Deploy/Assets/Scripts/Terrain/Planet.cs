using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Planet : IClass
{
    public List<EventSystem> eventSystems = new List<EventSystem>();

    //seed for this planet
    private int seed;
    //random object used for generating stuff
    private System.Random r;

    //vars to store points and connections
    private List<Vector3> points;
    private List<int[]> connections;
    public List<Triangle> triangles;

    private float radius;
    private float variance;


    /* parameters:
     * seed of the planet, min/max radius of planet, min/max variance of the surface, min/max number of points on the planet.
     * returns:
     * new Planet object
     * description: does all the math to make the planet, stores the representation of the planet by altering 'points' and 'connections'.
     */
    public Planet(System.Random r, float minRad, float maxRad, float minVariance, float maxVariance, int latPoints, int longPoints)
    {
        this.r = r;
        //get random values from the seed
        radius = (float)(r.NextDouble() * (maxRad - minRad) + minRad);
        variance = (float)(r.NextDouble() * (maxVariance - minVariance) + minVariance);
        int varianceSeed = r.Next(int.MinValue, int.MaxValue);

        //make the planet, store results in points and connections.
        PlanetGenerator.MakePlanet(radius, variance, varianceSeed, latPoints, longPoints, out points, out connections);
    }

    /* parameters: None
     * returns: ObjectUpdate with information neccesary to actually make the planet in unity.
     */
    public int[] GeneratePlanet(DateTime start, out List<int>[] map, out List<Vector3> pts, out List<int>[][] trianglesHash)
    {
        Debug.Log("planet mesh creation time (sec): " + (DateTime.Now - start).TotalSeconds);// -> 9
        map = ObjectUpdate.GetMap(connections);
        pts = points;
        Debug.Log(pts.Count);
        
        Debug.Log("planet mesh creation time (sec): " + (DateTime.Now - start).TotalSeconds);// -> 16
        triangles = new ObjectUpdate().GetTrianglesFromConnections(points, map, out trianglesHash);
        Debug.Log("planet mesh creation time (sec): " + (DateTime.Now - start).TotalSeconds);// -> 20
        Debug.Log(triangles.Count + " numtri");
        ObjectHandler.cacheObjMap = new List<CacheObjTuple>[points.Count][];
        Debug.Log("planet mesh creation time (sec): " + (DateTime.Now - start).TotalSeconds);//-> 21
        Debug.Log(pts.Count + " pts");
        int[] mesh = MeshBuilder3D.GetMeshFrom(points, triangles, map, trianglesHash);
        Debug.Log("planet mesh creation time (sec): " + (DateTime.Now - start).TotalSeconds);
        Debug.Log(pts.Count + " pts");
        return mesh;
    }

    public ObjectUpdate MakeObjectsOnSurface(Vector3 planetCenter, string pathToObject, int numObjs, float scale, ObjectPlacementDirection placeDir, bool isCache)
    {
        ObjectUpdate o = new ObjectUpdate();
        LayerMask mask = ~LayerMask.GetMask(new string[] { "ocean" });
        for (int i = 0; i < numObjs; i++)
        {
            Vector3 dir = new Vector3((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized;
            Vector3 randomUp = new Vector3((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized;
            RaycastHit hit;
            Physics.Raycast(planetCenter + dir * (2 * radius + 2 * variance) * scale, -dir, out hit, Mathf.Infinity, mask);
            Vector3 forward;
            if (placeDir == ObjectPlacementDirection.UP) { forward = dir; }
            else if(placeDir == ObjectPlacementDirection.NORMAL){ forward = hit.normal; }
            else if(placeDir == ObjectPlacementDirection.RANDOM) { forward = new Vector3((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized; }
            else { throw new Exception("ObjectPlacementDirection parameter is invalid"); }            
            o.AddInstantiationRequest(new InstantiationRequest(pathToObject, hit.point, Quaternion.LookRotation(forward, randomUp), triangles[hit.triangleIndex].points));
        }
        return o;
    }

    public void RenderDebugLines()
    {
        List<int[]> edgesToRender = PlanetGenerator.shownCons;
        foreach(int[] edge in edgesToRender)
        {
            Debug.DrawLine(points[edge[0]], points[edge[1]], Color.red, 1000000);
        }
    }


    public Type MonoScript
    {
        get 
        {
            return typeof(PlanetMono);
        }
    }
}

public enum ObjectPlacementDirection { UP, NORMAL, RANDOM };

