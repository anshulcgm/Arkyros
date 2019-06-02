using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlanetMono : MonoBehaviour, IMono
{
    private Planet planet;    

    public void Create(int seed)
    {
        System.Random r = new System.Random(seed);
        planet = new Planet(r, 1, 1.1f, 0.1f, 0.3f, 24, 25);
        //make the planet
        int[] mesh = planet.GeneratePlanet(start, out map, out pts, out trianglesHash);
        triangles = planet.triangles;

        List<Vector3> pointsCopy = new List<Vector3>();
        List<int>[] mapCopy = new List<int>[pts.Count];
        for(int i = 0; i < pts.Count; i++){
            pointsCopy.Add(pts[i]);
            mapCopy[i] = new List<int>();
            foreach(int i1 in map[i]){
                mapCopy[i].Add(i1);
            }
        }
        CollisionHandler.Initialize(pointsCopy, mapCopy);
        
        int numTrianglesUsed = 0;
        bool[] triangleUsed = new bool[triangles.Count];
        List<List<Vector3>> pointsPerBiome = new List<List<Vector3>>();
        List<int[]> biomes = new List<int[]>();
        List<int> biomeTypes = new List<int>();
        List<Triangle>[] biomesOverall = new List<Triangle>[biomeMats.Length];
        int minBiomeSize = 200;
        int maxBiomeSize = 1500;

        while (numTrianglesUsed < triangles.Count)
        {
            pointsPerBiome.Add(new List<Vector3>());
            int[] pointsMapping = new int[pts.Count];

            int biomeSize = r.Next(minBiomeSize, maxBiomeSize);
            int randomStartTri = r.Next(triangles.Count);
            while (triangleUsed[randomStartTri])
            {
                randomStartTri = r.Next(triangles.Count);
            }

            List<int> trisInBiome = new List<int>();
            List<int> trisToEval = new List<int>();
            trisToEval.Add(randomStartTri);

        for (int i = 0; i < r.Next(5, 20); i++)
        {
            List<Vector3> points = new List<Vector3>();
            List<int[]> cons = new List<int[]>();
            FloatingIslandGenerator.MakeFloatingIsland(0.5f, 0.1f, 4, 20, 0.2f, 0.25f, 0f, r, out points, out cons);
            List<int>[] islandMap = ObjectUpdate.GetMap(cons);

            List<int>[][] islandTrianglesHash = null;
            List<Triangle> islandTriangles = new ObjectUpdate().GetTrianglesFromConnections(points, islandMap, out islandTrianglesHash);
            int[] arr = MeshBuilder3D.GetMeshFrom(points, islandTriangles, islandMap, islandTrianglesHash);

            Vector4 randomQuat = new Vector4((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized;

            GameObject g = Instantiate(Resources.Load("FloatingIsland") as GameObject, Vector3.zero, new Quaternion(randomQuat.x, randomQuat.y, randomQuat.z, randomQuat.w));
            g.transform.position = g.transform.up * 5500;
            g.GetComponent<MeshFilter>().mesh = new Mesh();
            g.GetComponent<MeshFilter>().mesh.SetVertices(points);
            g.GetComponent<MeshFilter>().mesh.SetTriangles(arr, 0);

            g.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;

            g.GetComponent<MeshRenderer>().material = biomeMats[r.Next(biomeMats.Length)];
        }*/

        int numCities = r.Next(10, 20);
        for (int i = 0; i < numCities; i++)
        {
            List<Rectangle> rects = Tessellations.GetTesselation(r.Next());
            Vector4 randomQuat = new Vector4((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized;
            Quaternion randomQuaternion = new Quaternion(randomQuat.x, randomQuat.y, randomQuat.z, randomQuat.w);

            Vector3 normal = (randomQuaternion * Vector3.forward).normalized;
            Vector3 center = normal * 6000;

            Plane p = new Plane(normal, center);

            Vector2 mappedCenter = p.GetMappedPoint(center);

            foreach(Rectangle rect in rects)
            {
                Vector2 rectCenter = new Vector2(rect.getX() + rect.getWidth() / 2 - mappedCenter.x, rect.getY() + rect.getHeight() / 2 - mappedCenter.y);
                Vector3 trueCenter = rectCenter.x * p.xDir + rectCenter.y * p.yDir + center;
                RaycastHit hit;
                if (Physics.Raycast(trueCenter, -normal, out hit, Mathf.Infinity))
                {
                    GameObject g = Instantiate(Resources.Load("Building") as GameObject, trueCenter, randomQuaternion);
                    g.transform.localScale = new Vector3(rect.getWidth() * 0.6f, rect.getHeight() * 0.6f, (rect.getWidth() * 2));
                    g.transform.position = hit.point;
                    int rand = r.Next(4);
                    g.transform.localScale = new Vector3(g.transform.localScale.x * (rand == 0 ? buildingScale : 1),  g.transform.localScale.y * (rand == 1 ? buildingScale : 1), g.transform.localScale.z * (rand == 2 ? buildingScale : 1));
                }                
            }
        }

        for(int i = 0; i < biomeMats.Length; i++){
            for(int i1 = 0; i1 < trees[i].Length(); i1++){              
                planet.MakeObjectsOnSurface(planet.Points, biomesOverall[i], ObjectPlacementDirection.UP, "empty", trees[i][i1], numItems[i][i1], 0);
            }
        }

        //planet.MakeObjectsOnSurface(planet.Points, triangles, ObjectPlacementDirection.RANDOM, "Rock_Big", Rock_Big, 2500, 25);
        //planet.MakeObjectsOnSurface(planet.Points, triangles, ObjectPlacementDirection.UP, "Rock_Tall", Rock_Tall, 100, 5);
        //planet.MakeObjectsOnSurface(planet.Points, triangles, ObjectPlacementDirection.UP, "fire_rock", fire_rock, 500, 10);

        Debug.Log("full planet creation time (sec): " + (DateTime.Now - start).TotalSeconds);
        
    }


    public IClass GetMainClass()
    {
        return planet;
    }

    public void SetMainClass(IClass ic)
    {
        planet = (Planet)ic;
    }


}

public static class Splitter
{
    public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
    {
        for (var i = 0; i < (float)array.Length / size; i++)
        {
            yield return array.Skip(i * size).Take(size);
        }
    }
}

    [System.Serializable]
    public class EntityArray
    {
        public EntityGenerator[] array;

        public EntityGenerator this[int i]
        {
            get
            {
                return array[i];
            }
            set
            {
                array[i] = value;
            }
        }

        public int Length(){
            return array.Length;
        }
    }

    [System.Serializable]
    public class IntArray
    {
        public int[] array;

        public int this[int i]
        {
            get
            {
                return array[i];
            }
            set
            {
                array[i] = value;
            }
        }

        public int Length(){
            return array.Length;
        }
    }