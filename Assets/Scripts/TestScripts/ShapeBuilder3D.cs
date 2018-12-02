using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//only works if all faces are convex and if the material has no actual texture!
public class ShapeBuilder3D : MonoBehaviour
{
    //the array of points
    public Vector3[] points;
    //the edges
    public List<Edge> edges;
    //each face defined by an index in the points array
    public int[][] faces;
    Vector3 avgPt = Vector3.zero;

    //whether this thing creates itself or not
    public bool autoInit = false;
    //ties the points to a transform
    public Transform[] tiedTransforms;


    //if autoInit is true, then automatically make the shape
    public void Start ()
    {
        if(autoInit)
        {
            Initialize(points, edges);
        }
    }
    
    public void Initialize(Vector3[] points, List<Edge> edges)
    {
        tiedTransforms = new Transform[] { };
        //make the edges so that p1 < p2
        for (int i = 0; i < edges.Count; i++)
        {
            Edge e = edges[i];
            if(e.p1 > e.p2){
                int tempP1 = e.p1;
                e.p2 = e.p1;
                e.p1 = tempP1;
            }
        }
        //sort the edges such that (a,b) is less than (c,d) if (a < c or (a = c and b < d)).
        edges.Sort
        (
            delegate (Edge e1, Edge e2)
            {
                if(e1.p1 != e2.p1)
                {
                    return e1.p1.CompareTo(e2.p1);
                }
                else
                {
                    return e1.p2.CompareTo(e2.p2);
                }
            }
        );

        this.edges = edges;
        this.points = points;

        //get the 'average point' such that we have a point inside the mesh to create triangles from.
        foreach (Vector3 v in points)
        {
            avgPt += v;
        }
        avgPt /= points.Length;
        //get the faces
        faces = GetFacesFromEdges(points, edges);
        //set the mesh
        SetMesh(points, faces);
    }

    public void Initialize(Vector3[] points, int[][] triangles)
    {
        //get the 'average point' such that we have a point inside the mesh to create triangles from.
        foreach (Vector3 v in points)
        {
            avgPt += v;
        }
        avgPt /= points.Length;

        List<int> triList = new List<int>();
        foreach(int[] triangle in triangles)
        {
            Vector3 A = points[triangle[0]];
            Vector3 B = points[triangle[1]];
            Vector3 C = points[triangle[2]];
            Vector3 norm = Vector3.Cross((B - A), (C - A)).normalized;
            int mult = -1;
            if (Vector3.Angle(norm, A - avgPt) < 90)
            {
                mult = 1;
            }
            float d = -Vector3.Dot(norm, A);
            float w = Vector3.Dot(norm, -norm * mult);
            //reverse array if counter clockwise
            if(w > 0)
            {
                Array.Reverse(triangle);
            }
            triList.AddRange(triangle);
        }
        SetMeshByTriangles(points, triList.ToArray());
    }

    //quick mesh setting (no triangle calcs, no UV setting, blazingly fast)
    private void SetMeshUpdate(Vector3[] points)
    {
        List<Vector3> pointsList = new List<Vector3>();
        foreach(Vector3 v in points)
        {
            pointsList.Add(v);
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.SetVertices(pointsList);
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    #region start functions
    //gets the faces from edges. Highly intensive operation O(n^3), don't use this for high poly shapes. 
    //Also, don't call this in the update function.
    private int[][] GetFacesFromEdges(Vector3[] points, List<Edge> edges)
    {
        int[][] faces = new int[2 - points.Length + edges.Count][];
        List<Edge>[] edgesFromIndex = new List<Edge>[points.Length];
        for(int i = 0; i < points.Length; i++)
        {
            edgesFromIndex[i] = new List<Edge>();
        }

        foreach(Edge e in edges)
        {
            edgesFromIndex[e.p1].Add(e);
            edgesFromIndex[e.p2].Add(new Edge { p1 = e.p2, p2 = e.p1, });
        }
        int edgeA = 0;
        int edgeB = 0;
        Edge startEdge = edgesFromIndex[0][0];
        

        int numFaces = 0;
        while(numFaces < faces.Length)
        {
            List<int> face = null;
            List<Edge> edgesToRemove = new List<Edge>();
            for (int i1 = 0; i1 < edgesFromIndex[startEdge.p2].Count; i1++)
            {
                Vector3 A = points[startEdge.p1];
                Vector3 B = points[startEdge.p2];
                Vector3 C = points[edgesFromIndex[startEdge.p2][i1].p2];

                if (edgesFromIndex[startEdge.p2][i1].p2 == startEdge.p1)
                {
                    continue;
                }

                edgesToRemove.Add(startEdge);
                Vector3 norm = Vector3.Cross((B - A), (C - A)).normalized;
                int mult = 1;
                if (Vector3.Angle(norm, A - avgPt) < 90)
                {
                    mult = -1;
                }

                float d = -Vector3.Dot(norm, A);
                float w = Vector3.Dot(norm, -norm * mult);
                //if this is clockwise
                if (w < 0)
                {
                    face = new List<int>();
                    face.Add(startEdge.p1);
                    face.Add(startEdge.p2);
                    face.Add(edgesFromIndex[startEdge.p2][i1].p2);
                    Edge currEdge = edgesFromIndex[startEdge.p2][i1];
                    while (currEdge.p2 != startEdge.p1)
                    {
                        Edge newEdge = new Edge { p1 = -1, p2 = -1, };
                        for (int i2 = 0; i2 < edgesFromIndex[currEdge.p2].Count; i2++)
                        {
                            A = points[currEdge.p1];
                            B = points[currEdge.p2];
                            C = points[edgesFromIndex[currEdge.p2][i2].p2];
                            Vector3 normNew = Vector3.Cross((B - A), (C - A)).normalized;
                            mult = 1;
                            if (Vector3.Angle(normNew, A - avgPt) < 90)
                            {
                                mult = -1;
                            }
                            float wNew = Vector3.Dot(normNew, -normNew * mult);

                            if (wNew < 0 && Mathf.Abs(Vector3.Dot(norm, C) + d) < Mathf.Epsilon)
                            {
                                face.Add(edgesFromIndex[currEdge.p2][i2].p2);
                                newEdge = edgesFromIndex[currEdge.p2][i2];
                                break;
                            }
                        }
                        if (newEdge.p1 == -1)
                        {
                            face = null;
                            break;
                        }
                        edgesToRemove.Add(currEdge);
                        currEdge = newEdge;
                    }
                }
                if (face != null)
                {
                    List<int> faceCopy = new List<int>();
                    for (int i3 = 0; i3 < face.Count; i3++)
                    {
                        faceCopy.Add(face[i3]);
                    }
                    bool isFace = true;
                    for (int i = 0; i < numFaces; i++)
                    {
                        List<int> confirmedFace = new List<int>();
                        for (int i3 = 0; i3 < faces[i].Length; i3++)
                        {
                            confirmedFace.Add(faces[i][i3]);
                        }
                        if (confirmedFace.All(faceCopy.Contains) && confirmedFace.Count == faceCopy.Count)
                        {
                            isFace = false;
                            break;
                        }
                    }
                    if (isFace)
                    {
                        faces[numFaces] = faceCopy.ToArray();
                        numFaces++;
                        break;
                    }
                }
            }

            if(edgeB < edgesFromIndex[edgeA].Count - 1)
            {
                edgeB++;
            }
            else
            {
                edgeB = 0;
                edgeA++;
            }
            startEdge = edgesFromIndex[edgeA][edgeB];            
        }

        for (int i = 0; i < faces.Length; i++)
        {
            Array.Reverse(faces[i]);
        }
        return faces;
    }    

    //gets the triangles for a face, provided that it is convex.
    private List<int> GetTrianglesFromFace(int[] face)
    {
        int[] triangles = new int[(face.Length - 2) * 3];
        for (int i = 0; i < face.Length - 2; i++)
        {
            triangles[i * 3] = face[0];
            triangles[i * 3 + 1] = face[i + 1];
            triangles[i * 3 + 2] = face[i + 2];
        }
        List<int> triList = new List<int>();
        for(int i = 0; i < triangles.Length; i++)
        {
            triList.Add(triangles[i]);
        }
        return triList;
    }

    //sets the mesh given the points and faces
    public void SetMesh(Vector3[] points, int[][] faces)
    {
        //get mesh and coll
        Mesh mesh = GetComponent<MeshFilter>().mesh;


        for (int i = 0; i < faces.Length; i++)
        {
            String s = "{";
            for (int i1 = 0; i1 < faces[i].Length; i1++)
            {
                s += faces[i][i1] + ",";
            }
            s += "}";
            Debug.Log(s);
        }

        mesh.vertices = points;
        //get triangles
        List<int> triangles = new List<int>();
        for(int i = 0; i < faces.Length; i++)
        {
            triangles.AddRange(GetTrianglesFromFace(faces[i]));
        }
        //set triangles
        mesh.triangles = triangles.ToArray();

        //set the uvs so that the shader texture is evenly distributed.
        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
        }
        mesh.uv = uvs;


        //save all the changes
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        //set the mesh in the meshFilter component so that we can see it.
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    //sets the mesh given the points and faces
    public void SetMeshByTriangles(Vector3[] points, int[] triangles)
    {
        //get mesh and coll
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.vertices = points;
        //set triangles
        mesh.triangles = triangles;

        //set the uvs so that the shader texture is evenly distributed.
        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
        }
        mesh.uv = uvs;


        //save all the changes
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        //set the mesh in the meshFilter component so that we can see it.
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }


    #endregion
}
public struct Edge
{
    public int p1;
    public int p2;
}



