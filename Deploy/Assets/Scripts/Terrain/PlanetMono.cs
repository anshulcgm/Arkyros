using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlanetMono : MonoBehaviour, IMono
{
    private Planet planet;

    public GameObject building;

    public void Create(int seed)
    {
        System.Random r = new System.Random(seed);
        planet = new Planet(r, 1, 1.1f, 0.01f, 0.03f, 71, 71);
        //make the planet
        Mesh me = planet.GeneratePlanet();

        gameObject.GetComponent<MeshFilter>().mesh = me;
        gameObject.GetComponent<MeshCollider>().sharedMesh = me;

        for (int i = 0; i < r.Next(5, 20); i++)
        {
            List<Vector3> points = new List<Vector3>();
            List<int[]> cons = new List<int[]>();
            FloatingIslandGenerator.MakeFloatingIsland(0.5f, 0.1f, 4, 20, 0.2f, 0.25f, 0f, r, out points, out cons);
            List<Triangle>[,] trianglesHash;
            List<Triangle> triangles = new ObjectUpdate().GetTrianglesFromConnections(points, cons, out trianglesHash);
            Mesh m = MeshBuilder3D.GetMeshFrom(points, triangles, trianglesHash);

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

        ObjectUpdate rockSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Rock", 50000, transform.localScale.magnitude, ObjectPlacementDirection.RANDOM, true);
        ObjectHandler.Update(rockSpawnUpdate, gameObject);

        ObjectUpdate bigRockSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Rock_Big", 2500, transform.localScale.magnitude, ObjectPlacementDirection.RANDOM, true);
        ObjectHandler.Update(bigRockSpawnUpdate, gameObject);

        ObjectUpdate tallRockSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "Rock_Tall", 250, transform.localScale.magnitude, ObjectPlacementDirection.UP, true);
        ObjectHandler.Update(tallRockSpawnUpdate, gameObject);

        ObjectUpdate dustSpawnUpdate = planet.MakeObjectsOnSurface(transform.position, "fire_rock", 5000, transform.localScale.magnitude, ObjectPlacementDirection.UP, true);
        ObjectHandler.Update(dustSpawnUpdate, gameObject);
        //planet.RenderDebugLines();

        for(int i = 0; i < 10; i++)
        {
            Debug.Log("iusaghpugihgoiha0[gh09qhg0hq09hg09qh09gqheg[oihg09[h09q[hg0[9qrh0]9ghq09hg09hg09hqr09g");
            List<Rectangle> rects = Tessellations.GetTesselation();
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

