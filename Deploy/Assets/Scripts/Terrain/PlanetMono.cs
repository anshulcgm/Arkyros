using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlanetMono : MonoBehaviour, IMono
{
    private Planet planet;

    public GameObject building;

    public WalkingMap walkingMap;

    public Material[] biomeMats;

    public void Create(int seed)
    {
        DateTime start = DateTime.Now;
        System.Random r = new System.Random(seed);
        int minRad = 4000;
        int maxRad = 4000;
        planet = new Planet(r, minRad, maxRad, 10f, 30f, 91, 91);

        List<Vector3> pts = null;
        List<int>[] map = null;
        List<int>[][] trianglesHash;
        List<Triangle> triangles;

        //make the planet
        int[] mesh = planet.GeneratePlanet(start, out map, out pts, out trianglesHash);
        triangles = planet.triangles;
      
        int numTrianglesUsed = 0;
        bool[] triangleUsed = new bool[triangles.Count];
        List<List<Vector3>> pointsPerBiome = new List<List<Vector3>>();
        List<int[]> biomes = new List<int[]>();
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
                        
            while(trisInBiome.Count < biomeSize && numTrianglesUsed < triangles.Count && trisToEval.Count > 0)
            {                
                List<int> neighbors = new List<int>();
                for(int i = 0; i < 3; i++)
                {
                    foreach (List<int> a in trianglesHash[triangles[trisToEval[0]].points[i]])
                    {
                        neighbors.AddRange(a);
                    }
                }

                neighbors = neighbors.Distinct().Where(x => x != trisToEval[0]).ToList();
                foreach (int a in neighbors)
                {
                    if (!triangleUsed[a] && !trisToEval.Contains(a))
                    {                        
                        trisToEval.Add(a);
                    }
                }
                triangleUsed[trisToEval[0]] = true;
                trisInBiome.Add(trisToEval[0]);
                numTrianglesUsed++;
                pointsMapping[triangles[trisToEval[0]].points[0]] = pointsPerBiome[pointsPerBiome.Count - 1].Count;
                pointsPerBiome[pointsPerBiome.Count - 1].Add(pts[triangles[trisToEval[0]].points[0]]);
                pointsMapping[triangles[trisToEval[0]].points[1]] = pointsPerBiome[pointsPerBiome.Count - 1].Count;
                pointsPerBiome[pointsPerBiome.Count - 1].Add(pts[triangles[trisToEval[0]].points[1]]);
                pointsMapping[triangles[trisToEval[0]].points[2]] = pointsPerBiome[pointsPerBiome.Count - 1].Count;
                pointsPerBiome[pointsPerBiome.Count - 1].Add(pts[triangles[trisToEval[0]].points[2]]);

                trisToEval.RemoveAt(0);
            }           

            int[] biome = new int[trisInBiome.Count * 3];
            for(int i = 0; i < trisInBiome.Count; i++)
            {
                biome[i * 3] = pointsMapping[mesh[trisInBiome[i] * 3]];
                biome[i * 3 + 1] = pointsMapping[mesh[trisInBiome[i] * 3 + 1]];
                biome[i * 3 + 2] = pointsMapping[mesh[trisInBiome[i] * 3 + 2]];
            }
            biomes.Add(biome);
            Debug.Log(numTrianglesUsed);
        }
        int n = 0;
        foreach (int[] biome in biomes)
        {
            GameObject newObj = Instantiate((GameObject)Resources.Load("Empty"), Vector3.zero, Quaternion.identity);
            newObj.GetComponent<MeshFilter>().mesh = new Mesh();
            newObj.GetComponent<MeshFilter>().mesh.subMeshCount = biomes.Count;
            newObj.GetComponent<MeshFilter>().mesh.SetVertices(pointsPerBiome[n]);
            newObj.GetComponent<MeshFilter>().mesh.SetTriangles(biome, 0);
            newObj.GetComponent<MeshRenderer>().material = biomeMats[r.Next(biomeMats.Length)];
            newObj.GetComponent<MeshCollider>().sharedMesh = newObj.GetComponent<MeshFilter>().mesh;
            n++;
        }
        

        /*
        for (int i = 0; i < r.Next(5, 20); i++)
        {
            List<Vector3> points = new List<Vector3>();
            List<int[]> cons = new List<int[]>();
            FloatingIslandGenerator.MakeFloatingIsland(0.5f, 0.1f, 4, 20, 0.2f, 0.25f, 0f, r, out points, out cons);
            triangles = new ObjectUpdate().GetTrianglesFromConnections(points, map, out trianglesHash);
            int[] arr = MeshBuilder3D.GetMeshFrom(points, triangles, map, trianglesHash);

            Vector4 randomQuat = new Vector4((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized;

            GameObject g = Instantiate(Resources.Load("FloatingIsland") as GameObject, Vector3.zero, new Quaternion(randomQuat.x, randomQuat.y, randomQuat.z, randomQuat.w));
            g.transform.position = g.transform.up * 5500;
            gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
            gameObject.GetComponent<MeshFilter>().mesh.SetVertices(points);
            gameObject.GetComponent<MeshFilter>().mesh.SetTriangles(arr, 0);

            gameObject.GetComponent<MeshCollider>().sharedMesh = new Mesh();
            gameObject.GetComponent<MeshCollider>().sharedMesh.SetVertices(points);
            gameObject.GetComponent<MeshCollider>().sharedMesh.SetTriangles(arr, 0);
        }
        
        
        //spawn in the trees
        ObjectUpdate treeSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Savanah_Tree", 10000, transform.localScale.magnitude, ObjectPlacementDirection.UP, true);
        
        ObjectHandler.Update(treeSpawnUpdate, gameObject);

        //spawn in the bushes
        ObjectUpdate glowOreSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Fractal", 0, transform.localScale.magnitude, ObjectPlacementDirection.NORMAL, false);
        ObjectHandler.Update(glowOreSpawnUpdate, gameObject);

        ObjectUpdate rockSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Rock", 50000, transform.localScale.magnitude, ObjectPlacementDirection.RANDOM, true);
        ObjectHandler.Update(rockSpawnUpdate, gameObject);

        ObjectUpdate bigRockSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Rock_Big", 2500, transform.localScale.magnitude, ObjectPlacementDirection.RANDOM, true);
        ObjectHandler.Update(bigRockSpawnUpdate, gameObject);

        ObjectUpdate tallRockSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Rock_Tall", 250, transform.localScale.magnitude, ObjectPlacementDirection.UP, true);
        ObjectHandler.Update(tallRockSpawnUpdate, gameObject);

        ObjectUpdate dustSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "fire_rock", 5000, transform.localScale.magnitude, ObjectPlacementDirection.UP, true);
        ObjectHandler.Update(dustSpawnUpdate, gameObject);
        //planet.RenderDebugLines();
        
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("iusaghpugihgoiha0[gh09qhg0hq09hg09qh09gqheg[oihg09[h09q[hg0[9qrh0]9ghq09hg09hg09hqr09g");
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
                }
            }
        }

        //walkingMap = new WalkingMap(pts, map, maxRad);
        Debug.Log("full planet creation time (sec): " + (DateTime.Now - start).TotalSeconds);
        */
    }

    public void Update()
    {
        //m.CreateMap();
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

