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
        ObjectUpdate meshUpdate = planet.GeneratePlanet();
        ObjectHandler.Update(meshUpdate, gameObject);

        for (int i = 0; i < r.Next(5, 20); i++)
        {
            List<Vector3> points = new List<Vector3>();
            List<int[]> cons = new List<int[]>();
            FloatingIslandGenerator.MakeFloatingIsland(0.5f, 0.1f, 4, 20, 0.2f, 0.25f, 0f, r, out points, out cons);
            Mesh m = MeshBuilder3D.GetMeshFrom(points, new ObjectUpdate().GetTrianglesFromConnections(points, cons));
            Vector4 randomQuat = new Vector4((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f).normalized;
            GameObject g = Instantiate(Resources.Load("FloatingIsland") as GameObject, Vector3.zero, new Quaternion(randomQuat.x, randomQuat.y, randomQuat.z, randomQuat.w));
            g.transform.position = g.transform.up * 5500;
            g.GetComponent<MeshFilter>().mesh = m;
            g.GetComponent<MeshCollider>().sharedMesh = m;
        }

        //spawn in the trees
        ObjectUpdate treeSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Savanah_Tree", 10000, transform.localScale.magnitude, ObjectPlacementDirection.UP, true);
        
        ObjectHandler.Update(treeSpawnUpdate, gameObject);

        //spawn in the bushes
        ObjectUpdate glowOreSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Fractal", 0, transform.localScale.magnitude, ObjectPlacementDirection.NORMAL, false);
        ObjectHandler.Update(glowOreSpawnUpdate, gameObject);

        ObjectUpdate rockSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Rock_1", 50000, transform.localScale.magnitude, ObjectPlacementDirection.RANDOM, true);
        ObjectHandler.Update(rockSpawnUpdate, gameObject);

        ObjectUpdate dustSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "fire_rock", 5000, transform.localScale.magnitude, ObjectPlacementDirection.UP, true);
        ObjectHandler.Update(dustSpawnUpdate, gameObject);
        //planet.RenderDebugLines();



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

