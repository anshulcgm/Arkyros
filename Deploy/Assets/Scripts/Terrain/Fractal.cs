using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Fractal
{
    private FractalShape baseShape;
    private GameObject fractalChild;
    private float normalDist;
    private float proportionBaseSize;
    private int maxDepth;

    public Fractal(FractalShape baseShape, float normalDist, float proportionBaseSize, int maxDepth)
    {
        this.baseShape = baseShape;
        this.normalDist = normalDist;
        this.proportionBaseSize = proportionBaseSize;
        this.maxDepth = maxDepth;
        fractalChild = (GameObject)Resources.Load("empty");
    }

    private struct Tuple
    {
        public int depth;
        public FractalShape f;
    }

    public void Generate()
    {
        List<Tuple> fractals = new List<Tuple>();
        fractals.Add(new Tuple { depth = 0, f = baseShape });
        while (fractals.Count > 0)
        {
            //create the mesh in the world
            GameObject child = GameObject.Instantiate(fractalChild, Vector3.zero, Quaternion.identity);
            List<Vector3> normals = null;
            Mesh mesh = null; //MeshBuilder3D.GetMeshFrom(fractals[0].f.points, fractals[0].f.triangles, fractals[0].f.trianglesHash, out normals);
            child.GetComponent<MeshFilter>().mesh = mesh;
            child.GetComponent<MeshCollider>().sharedMesh = mesh;

            if(fractals[0].depth < maxDepth)
            {
                //go though generation faces, create new shapes.
                foreach (int[] face in fractals[0].f.generationFaces)
                {
                    List<FractalShape> newFractals = GetTransitionShapes(face, fractals[0].f, normalDist, proportionBaseSize, normals);
                    List<Tuple> newTuples = new List<Tuple>();
                    foreach (FractalShape f in newFractals)
                    {
                        newTuples.Add(new Tuple { depth = fractals[0].depth + 1, f = f });
                    }
                    fractals.AddRange(newTuples);
                }
            }
            fractals.RemoveAt(0);   
        }
    }

    public Vector3 GetFaceNormal(int[] face, FractalShape baseShape, List<Vector3> normals)
    {
        int triangleIndex = -1;
        for (int i = 0; i < face.Length; i++)
        {
            for (int i1 = 0; i1 < face.Length; i1++)
            {
                for (int i2 = 0; i2 < face.Length; i2++)
                {
                    if (i == i1 || i == i2 || i1 == i2)
                    {
                        continue;
                    }
                    int[] triangle = new int[] { face[i], face[i1], face[i2] };
                    for (int i3 = 0; i3 < baseShape.triangles.Count; i3++)
                    {
                        if (baseShape.triangles[i3].points.SequenceEqual(triangle))
                        {
                            triangleIndex = i3;
                        }
                    }
                }
                if (triangleIndex != -1)
                {
                    break;
                }
            }
            if (triangleIndex != -1)
            {
                break;
            }
        }
        return normals[triangleIndex];
    }

    //creates a 'transition' shape between baseShapes to maintain similar baseShape proportions.
    public List<FractalShape> GetTransitionShapes(int[] generationFaceIndexes, FractalShape baseShape, float normalDist, float proportionBaseSize, List<Vector3> normals)
    {
        List<Vector3> generationFace = new List<Vector3>();
        for(int i = 0; i < generationFaceIndexes.Length; i++)
        {
            generationFace.Add(baseShape.points[generationFaceIndexes[i]]);
        }
        Vector3 normal = GetFaceNormal(generationFaceIndexes, baseShape, normals);
        Debug.DrawLine(Vector3.zero, normal.normalized * 10, Color.cyan, 10000);
        Plane transitionPlane = new Plane(normal, generationFace[0] + normal * normalDist);

        Vector3 averageGenerationPoint = Vector3.zero;
        for(int i = 0; i < generationFace.Count; i++) { averageGenerationPoint += generationFace[i] + normal * normalDist; }
        averageGenerationPoint /= generationFace.Count;
        Vector2 averageGenerationPoint2D = transitionPlane.GetMappedPoint(averageGenerationPoint);

        Plane basePlane = new Plane(baseShape.points[baseShape.baseFace[0]], baseShape.points[baseShape.baseFace[1]], baseShape.points[baseShape.baseFace[2]]);
        Vector3 averageBase = Vector3.zero;
        for (int i = 0; i < baseShape.baseFace.Length; i++) { averageGenerationPoint += baseShape.points[baseShape.baseFace[i]] + normal * normalDist; }
        averageBase /= generationFace.Count;
        Vector2 averageBase2D = basePlane.GetMappedPoint(averageBase);

        List<Vector2> basePointsMapped = new List<Vector2>();
        for(int i = 0; i < baseShape.baseFace.Length; i++)
        {
            basePointsMapped.Add((basePlane.GetMappedPoint(baseShape.points[baseShape.baseFace[i]]) - averageBase2D) * proportionBaseSize);
        }

        List<Vector3> transitionFace = new List<Vector3>();
        for (int i = 0; i < basePointsMapped.Count; i++)
        {
            transitionFace.Add(basePointsMapped[i].x * transitionPlane.xDir + basePointsMapped[i].y * transitionPlane.yDir + averageGenerationPoint);
        }

        List<Vector3> transitionShapePoints = new List<Vector3>();
        transitionShapePoints.AddRange(generationFace);
       
        for (int i = 0; i < generationFace.Count; i++)
        {
            float leastDist = float.MaxValue;
            int leastDistIndex = -1;
            for (int i1 = 0; i1 < transitionFace.Count; i1++)
            {
                float sqrDist = Vector2.SqrMagnitude(transitionPlane.GetMappedPoint(transitionShapePoints[i]) - transitionPlane.GetMappedPoint(basePointsMapped[i]));
                if (sqrDist <= leastDist)
                {
                    leastDist = sqrDist;
                    leastDistIndex = i1;
                }
            }
            Debug.DrawLine(generationFace[i], transitionFace[leastDistIndex], Color.white, 10000);
            transitionShapePoints.Add(transitionFace[leastDistIndex]);
            transitionFace.RemoveAt(leastDistIndex);
        }

        List<int[]> transitionShapeTriangles = new List<int[]>();
        for(int i = 0; i < generationFace.Count; i++)
        {
            //transitionShapeTriangles.Add(new int[] { i, i + generationFace.Count, (i + generationFace.Count - 1) % transitionShapePoints.Count });
            transitionShapeTriangles.Add(new int[] { i, (i + 1) % generationFace.Count, (i + generationFace.Count + 1) % transitionShapePoints.Count });

            if(i + 2 < generationFace.Count)
            {
                transitionShapeTriangles.Add(new int[] { 0, i + 1, i + 2 });
                transitionShapeTriangles.Add(new int[] { generationFace.Count, i + generationFace.Count + 1, i + generationFace.Count + 2 });
            }            
        }
        
        FractalShape transitionShape = new FractalShape(transitionShapePoints, transitionShapeTriangles, new int[0], new List<int[]>());

        List<Vector2> baseShapeMapped = new List<Vector2>();
        List<float> distFromBasePlane = new List<float>();
        for(int i = 0; i < baseShape.points.Count; i++)
        {
            distFromBasePlane.Add(basePlane.GetDistToPlane(baseShape.points[i]) * proportionBaseSize);
            baseShapeMapped.Add((basePlane.GetMappedPoint(baseShape.points[i]) - averageBase2D) * proportionBaseSize);
        }

        List<Vector3> newBaseShapePoints = new List<Vector3>();
        for(int i = 0; i < baseShape.points.Count; i++)
        {
            newBaseShapePoints.Add(baseShapeMapped[i].x * transitionPlane.xDir + baseShapeMapped[i].y * transitionPlane.yDir + distFromBasePlane[i] * transitionPlane.normal.normalized + averageGenerationPoint);
        }
        FractalShape newBaseShape = new FractalShape(newBaseShapePoints, baseShape.triangles, baseShape.trianglesHash, baseShape.baseFace, baseShape.generationFaces);

        return new List<FractalShape> { transitionShape, newBaseShape };
    }
}

public class FractalShape
{
    public List<Vector3> points { get; internal set; }
    public List<Triangle> triangles { get; internal set; }
    public List<Triangle>[,] trianglesHash { get; internal set; }
    public int[] baseFace { get; internal set; }
    public List<int[]> generationFaces { get; internal set; }

    public FractalShape(List<Vector3> points, List<int[]> cons, int[] baseFace, List<int[]> generationFaces)
    {
        this.points = points;


        List<Triangle>[,] triHashTemp = null;
        triangles = null;//new ObjectUpdate().GetTrianglesFromConnections(points, cons, out triHashTemp);
        Debug.Log(triangles.Count + "numTri");
        trianglesHash = triHashTemp;

        this.baseFace = baseFace;
        this.generationFaces = generationFaces;
    }

    public FractalShape(List<Vector3> points, List<Triangle> triangles, List<Triangle>[,] trianglesHash, int[] baseFace, List<int[]> generationFaces)
    {
        this.points = points;
        this.triangles = triangles;
        this.trianglesHash = trianglesHash;
        this.baseFace = baseFace;
        this.generationFaces = generationFaces;
    }
}
