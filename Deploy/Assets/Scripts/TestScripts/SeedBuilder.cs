using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBuilder : MonoBehaviour
{
    public int numpts;
    public float normalfactorSpikyRangeLow;
    public float normalfactorSpikyRangeHigh;
    public float normalfactorRangeLow;
    public float normalfactorRangeHigh;

    public Vector3 center;
    public float minsize;
    public float maxsize;
    public bool showWireFrame;
    public GameObject fractalChild;
    public float normalfactor;

    private bool isSpiky;
    
    public void Start()
    {
        isSpiky = true;
        //0.5% spiky
        if (isSpiky)
        {
            normalfactor = UnityEngine.Random.Range(normalfactorSpikyRangeLow, normalfactorSpikyRangeHigh);
            minsize = 200;
            maxsize = 300;
            numpts = UnityEngine.Random.Range(3, 5);
        }
        //99.5% plate
        else
        {
            normalfactor = UnityEngine.Random.Range(normalfactorRangeLow, normalfactorRangeHigh);
            minsize = 35;
            maxsize = 45;
        }
        List<int[]> buildFaces = new List<int[]>();
        int endPt = -1;
        Shape3D s = BuildSeed(transform.position, minsize, maxsize, numpts, out buildFaces, out endPt);
        GetComponent<ShapeBuilder3D>().Initialize(s.points.ToArray(), s.triangles);

        if (isSpiky)
        {
            List<Shape3D> children = GenerateFractal(s, buildFaces, endPt);
            foreach(Shape3D shape in children)
            {
                GameObject g = Instantiate(fractalChild, transform.position, Quaternion.identity, transform);
                ShapeBuilder3D sh = g.GetComponent<ShapeBuilder3D>();
                sh.Initialize(shape.points.ToArray(), shape.triangles);
            }
        }
          
    }

    public struct FractalShape
    {
        public int depth;
        public int endpt;
        public Shape3D shape;
        public List<int[]> buildFaces;
    }

    public List<Shape3D> GenerateFractal(Shape3D shape, List<int[]> buildFaces, int endpt)
    {
        FractalShape origin = new FractalShape
        {
            shape = shape,
            buildFaces = buildFaces,
            endpt = endpt,
            depth = 0,
        };
        List<FractalShape> unhandledChildren = new List<FractalShape>{origin};
        List<Shape3D> shapes = new List<Shape3D>();
        while (unhandledChildren.Count > 0)
        {       
            

            List<FractalShape> newChildren = GetChildren(unhandledChildren[0]);
            unhandledChildren.AddRange(newChildren);
            foreach(FractalShape f in newChildren)
            {
                shapes.Add(f.shape);
            }
            
            unhandledChildren.RemoveAt(0);

            while (unhandledChildren.Count > 0 && unhandledChildren[0].depth > 3)
            {
                unhandledChildren.RemoveAt(0);
            }
        }
        return shapes;
    }

    public List<FractalShape> GetChildren(FractalShape origin)
    {
        List<int[]> triangles = origin.buildFaces;
        List<Vector3> points = origin.shape.points;
        int endpt = origin.endpt;

        Vector3 avgPt = Vector3.zero;
        foreach (Vector3 v in origin.shape.points)
        {
            avgPt += v;
        }
        avgPt /= origin.shape.points.Count;

        List<FractalShape> children = new List<FractalShape>();
        foreach (int[] triangle in triangles)
        {
            List<Vector3> childPoints = new List<Vector3>();
            childPoints.Add(points[endpt]);

            int[] triPts = new int[2];
            int ind = 0;
            for (int i = 0; i < triangle.Length; i++)
            {
                if (triangle[i] != endpt)
                {
                    triPts[ind] = triangle[i];
                    ind++;
                }
            }
            childPoints.Add(Vector3.Lerp(points[triPts[0]], points[endpt], UnityEngine.Random.Range(0.7f,0.8f)));
            childPoints.Add(Vector3.Lerp(points[triPts[1]], points[endpt], UnityEngine.Random.Range(0.7f,0.8f)));

            Vector3 A = childPoints[0];
            Vector3 B = childPoints[1];
            Vector3 C = childPoints[2];
            Vector3 norm = Vector3.Cross((B - A), (C - A)).normalized;
            int mult = -1;
            if (Vector3.Angle(norm, A - avgPt) < 90)
            {
                mult = 1;
            }
            float d = -Vector3.Dot(norm, A);
            float w = Vector3.Dot(norm, -norm * mult);
            //reverse array if counter clockwise
            if (w > 0)
            {
                childPoints.Reverse();
            }

            Vector3 sum = Vector3.zero;
            foreach (Vector3 v in childPoints)
            {
                sum += v;
            }
            Vector3 center = sum / 3;


            Plane p = new Plane(childPoints[0], childPoints[1], childPoints[2]);
            Vector3 outPoint = center + p.normal.normalized * (normalfactor/Mathf.Sqrt(origin.depth + 1));
            childPoints.Add(outPoint);

            int[][] childTriangles = new int[][] { new int[] { 0, 1, 2 }, new int[] { 0, 1, 3 }, new int[] { 0, 2, 3 }, new int[] { 1, 2, 3 } };
            List<int[]> buildFaces = new List<int[]>
            {
                childTriangles[1], childTriangles[2], childTriangles[3]
            };

            Shape3D s = new Shape3D(childTriangles, childPoints);
            FractalShape child = new FractalShape
            {
                shape = s,
                buildFaces = buildFaces,
                endpt = 3,
                depth = origin.depth + 1,
            };
            children.Add(child);
        }
        return children;
    }

    public Shape3D BuildSeed(Vector3 center, float minsize, float maxsize, int numpts, out List<int[]> buildFaces, out int endPt)
    {
        List<int[]> triangles = new List<int[]>();
        buildFaces = new List<int[]>();
        Vector3 normal = UnityEngine.Random.onUnitSphere;
        Vector3 polygonCenter = normal.normalized * UnityEngine.Random.Range(minsize, maxsize) + center;
        //get the polygon
        List<Vector3> polygon = GetRandomPolygon(normal, polygonCenter, minsize, maxsize, numpts);
        for(int i = 0; i < polygon.Count; i++)
        {
            triangles.Add(new int[] { i, (i + 1) % polygon.Count, polygon.Count + 1 });
            triangles.Add(new int[] { i, (i + 1) % polygon.Count, polygon.Count});
            buildFaces.Add(new int[] { i, (i + 1) % polygon.Count, polygon.Count});

            if (showWireFrame)
            {
                Debug.DrawLine(polygon[i], polygon[(i + 1) % polygon.Count], Color.red, 1000000);
                Debug.DrawLine(polygon[i], polygonCenter + normal * 10, Color.red, 1000000);
            }
        }
        endPt = polygon.Count;
        //add the outlying point
        polygon.Add(polygonCenter + normal * normalfactor);
        //add the center
        polygon.Add(polygonCenter);        
        return new Shape3D(triangles.ToArray(), polygon);
    }

    

    public List<Vector3> GetRandomPolygon(Vector3 normal, Vector3 polygonCenter, float minsize, float maxsize, int numpts)
    {
        Plane p = new Plane(normal, polygonCenter);

        List<Vector3> points = new List<Vector3>();
        if (showWireFrame)
        {
            Debug.DrawLine(polygonCenter, polygonCenter + p.GetParallelFrom(polygonCenter), Color.green, 100000);
            Debug.DrawLine(polygonCenter, polygonCenter + p.normal, Color.blue, 100000);
        }

        float angle = 0;
        for(int i = 0; i < numpts; i++)
        {            
            Vector3 dir = p.GetRotatedVectorAlongPlane(p.GetParallelFrom(polygonCenter), angle);            
            points.Add(dir * UnityEngine.Random.Range(minsize, maxsize) + polygonCenter);
            if (showWireFrame)
            {
                Debug.DrawLine(polygonCenter, points[i], Color.yellow, 100000);
            }
            angle += Mathf.PI * 2 / numpts;
        }
        return points;
    }    
}


public class Shape3D
{
    public int[][] triangles;
    public List<Vector3> points;
    public Shape3D( int[][] triangles, List<Vector3> points)
    {
        this.triangles = triangles;
        this.points = points;
    }
}