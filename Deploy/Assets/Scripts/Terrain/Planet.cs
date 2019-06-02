using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Planet : IClass
{
    public EventSystemMono eventSystem;

    //seed for this planet
    private int seed;
    //random object used for generating stuff
    private System.Random r;

    //vars to store points and connections
    private readonly List<Vector3> points;
    private List<int[]> connections;

    private List<int>[] map;
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
        map = ObjectUpdate.GetMap(connections);
        pts = Points;
        Debug.Log(pts.Count);
        
        triangles = new ObjectUpdate().GetTrianglesFromConnections(Points, map, out trianglesHash);
        EventSystemMono.cacheObjMap = new List<int>[Points.Count];
        List<Vector3> pointsCopy = new List<Vector3>();
        foreach(Vector3 v in Points){
            pointsCopy.Add(v);
        }
        EventSystemMono.SetPoints(pointsCopy);
        EventSystemMono.map = map;
        int[] mesh = MeshBuilder3D.GetMeshFrom(Points, triangles, map, trianglesHash);
        return mesh;
    }

    public void MakeObjectsOnSurface(List<Vector3> points, List<Triangle> triangles, ObjectPlacementDirection placeDir, string pathToObject, EntityGenerator e, int numObjs, int numInCache)
    {        
        List<Vector3> posns = new List<Vector3>();
        List<Quaternion> rots = new List<Quaternion>();
        List<int> closestPoints = new List<int>();
        for(int i = 0; i < numObjs; i++)
        {
            int triangle = r.Next(triangles.Count);
            Vector3 point = (Vector3.Lerp(points[triangles[triangle].points[0]], points[triangles[triangle].points[1]], (float)r.NextDouble()) + 
            
                            Vector3.Lerp(points[triangles[triangle].points[0]], points[triangles[triangle].points[2]], (float)r.NextDouble())) / 2;

            int closestPoint = -1;
            float leastDist = float.MaxValue;
            for(int i1 = 0; i1 < 3; i1++)
            {
                float dist = Vector3.SqrMagnitude(points[triangles[triangle].points[i1]] - point);
                if(dist < leastDist){
                    leastDist = dist;
                    closestPoint = triangles[triangle].points[i1];
                }
            }

            Vector3 forward;
            if (placeDir == ObjectPlacementDirection.UP) { forward = point.normalized; }
            else if(placeDir == ObjectPlacementDirection.NORMAL){ forward = triangles[triangle].plane.normal * triangles[triangle].GetDirInt(); }
            else if(placeDir == ObjectPlacementDirection.RANDOM) { forward = new Vector3((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized; }
            else { throw new Exception("ObjectPlacementDirection parameter is invalid"); }
            Vector3 randomUp = new Vector3((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized;
            rots.Add(Quaternion.LookRotation(forward, randomUp));
            posns.Add(point);
            closestPoints.Add(closestPoint);                  
        }
        Vector3 scale = ((GameObject)Resources.Load(pathToObject)).transform.localScale;
        for(int i = 0; i < posns.Count; i++){
            e.CreateEntity(posns[i], rots[i], scale, closestPoints[i]);
        }
    }
    public void RenderDebugLines()
    {
        List<int[]> edgesToRender = PlanetGenerator.shownCons;
        foreach(int[] edge in edgesToRender)
        {
            Debug.DrawLine(Points[edge[0]], Points[edge[1]], Color.red, 1000000);
        }
    }


    public Type MonoScript
    {
        get 
        {
            return typeof(PlanetMono);
        }
    }

    public List<Vector3> Points
    {
        get
        {
            return points;
        }
    }

    public List<Vector3> Points1
    {
        get
        {
            return points;
        }
    }
}

public enum ObjectPlacementDirection { UP, NORMAL, RANDOM };

