﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this class builds meshes given edges and points as input
public class MeshBuilder3D
{

    public static int[] GetMeshFrom(List<Vector3> points, List<Triangle> allTriangles, List<int>[] map, List<int>[][] trianglesHash, out List<Vector3> normals)
    {
        normals = new List<Vector3>();
        BlockingDirection dir;
        DateTime start = DateTime.Now;
        int ind = GetFirstKnownTriangle(points, allTriangles, out dir);

        allTriangles[ind].direction = dir;
        List <Triangle> trianglesWithKnownDirection = new List<Triangle> { allTriangles[ind] };
        start = DateTime.Now;        

        while (trianglesWithKnownDirection.Count > 0)
        {
            Triangle t = trianglesWithKnownDirection[0];
            Plane tPlane = t.plane;
            List<Triangle> neighbors = GetNeighboringTriangles(t, map, allTriangles, trianglesHash);
            for (int i1 = 0; i1 < neighbors.Count; i1++)
            {
                Triangle neighbor = neighbors[i1];
                Plane neighborPlane = neighbor.plane;
                if (neighbor.GetDirInt() == 0)
                {
                    Vector3 centerA = GetCentroid(t, points);
                    Vector3 centerB = GetCentroid(neighbor, points);
                    float distA = tPlane.GetDistToPlane(centerB) * t.GetDirInt();
                    float distB = neighborPlane.GetDistToPlane(centerA);
                    if(Math.Abs(distA) < Mathf.Epsilon || Math.Abs(distB) < Mathf.Epsilon)
                    {
                        continue;
                    }
                    bool a = Math.Sign(distA) == Math.Sign(distB);
                    if (a)
                    {
                        neighbor.direction = BlockingDirection.AWAY_FROM_NORMAL;
                        Vector3 center = GetCentroid(neighbor, points);
                        //Debug.DrawLine(center * 4000, center * 4000 + neighbor.plane.normal.normalized * neighbor.GetDirInt() * 100, Color.red, 100000.0f);
                    }
                    else
                    {
                        neighbor.direction = BlockingDirection.TOWARDS_NORMAL;
                        Vector3 center = GetCentroid(neighbor, points);
                        //Debug.DrawLine(center * 4000, center * 4000 + neighbor.plane.normal.normalized * neighbor.GetDirInt() * 100, Color.cyan, 100000.0f);
                    }
                    trianglesWithKnownDirection.Add(neighbor);
                }
            }
            trianglesWithKnownDirection.RemoveAt(0);
        }
        

        Debug.Log("fill: " + (DateTime.Now - start).TotalSeconds);
        int[] faces = new int[allTriangles.Count * 3];
        for (int i = 0; i < allTriangles.Count; i++)
        {
            Array.Sort(allTriangles[i].points);
            Vector3 a = points[allTriangles[i].points[0]];
            Vector3 b = points[allTriangles[i].points[1]];
            Vector3 c = points[allTriangles[i].points[2]];
            normals.Add(allTriangles[i].plane.normal * allTriangles[i].GetDirInt());
            int[] triangle = new int[3];
            if (!IsClockwise(b - a, c - a, allTriangles[i].plane.normal * allTriangles[i].GetDirInt()))
            {
                triangle[0] = allTriangles[i].points[2];
                triangle[1] = allTriangles[i].points[1];
                triangle[2] = allTriangles[i].points[0];
            }
            else
            {
                triangle[0] = allTriangles[i].points[0];
                triangle[1] = allTriangles[i].points[1];
                triangle[2] = allTriangles[i].points[2];
            }
            faces[i * 3] = triangle[0];
            faces[i * 3 + 1] = triangle[1];
            faces[i * 3 + 2] = triangle[2];
            //Debug.DrawLine(GetCentroid(allTriangles[i], points) * 1000, GetCentroid(allTriangles[i], points) * 1000 + allTriangles[i].normal * allTriangles[i].direction * 1000, Color.red, 1000);
        }
        Debug.Log("running tris: " + (DateTime.Now - start).TotalSeconds);
        return faces;
    }


    /* Parameters: 
     * points: List of Vector3 objects containing points that represent the verticies of the mesh
     * triangles: List of int[]'s that contain 3 indexes per int[]. Each int[] represents one triangle of the mesh. Indexes within each int[] do not need to be sorted in any order
     * 
     * Returns:
     * Mesh object that can be assigned to a GameObject to display the mesh.
     */
    public static int[] GetMeshFrom(List<Vector3> points, List<Triangle> trianglesIndexes, List<int>[] map, List<int>[][] trianglesHash)
    {
        List<Vector3> normals = null;
        return GetMeshFrom(points, trianglesIndexes, map, trianglesHash, out normals);
    }

    #region Helper Functions

    /* Parameters: 
     * a: first directional vector on plane
     * b: second directional vector on plane
     * normal: normal vector of the plane (which way the plane is oriented)
     * 
     * Returns:
     * whether the vectors are going clockwise or not relative to the normal
     */
    static bool IsClockwise(Vector3 a, Vector3 b, Vector3 normal)
    {
        Vector3 crossProd = Vector3.Cross(a, b);
        return Vector3.Dot(crossProd, normal) > 0;
    }

    
    ///@TODO: need to make this function work for concave polygons as well
    /* Parameters: 
     * face: list of indexes of points in an array that represent a polygon/side/face of a mesh
     * 
     * Returns:
     * list of triangles that make up the face
     * NOTE: does not work if the face has concave portions. Only works on convex polygons.
     */
    private static List<int> GetTrianglesFromFace(int[] face)
    {
        int[] triangles = new int[(face.Length - 2) * 3];
        for (int i = 0; i < face.Length - 2; i++)
        {
            triangles[i * 3] = face[0];
            triangles[i * 3 + 1] = face[i + 1];
            triangles[i * 3 + 2] = face[i + 2];
        }
        List<int> triList = new List<int>();
        for (int i = 0; i < triangles.Length; i++)
        {
            triList.Add(triangles[i]);
        }
        return triList;
    }

    /* Parameters: 
     * points: points of the mesh
     * faces: indexes of points that make up the 'faces' or 'sides' of the 3D mesh
     * 
     * Returns:
     * Mesh object that can be attatched to a GameObject to display the 3D mesh/shape
     */
    static Mesh GetMesh(List<Vector3> points, int[] triangles)
    {
        //get mesh and coll
        Mesh mesh = new Mesh();
        mesh.subMeshCount = 1;
        mesh.SetVertices(points);
        mesh.SetTriangles(triangles, 0);
        return mesh;
    }

    private static int GetFirstKnownTriangle(List<Vector3> points, List<Triangle> trianglesList, out BlockingDirection dir)
    {

        List<int[]> edges = new List<int[]>();
        for (int i = 0; i < trianglesList.Count; i++)
        {
            edges.Add(new int[] { trianglesList[i].points[0], trianglesList[i].points[1] });
            edges.Add(new int[] { trianglesList[i].points[0], trianglesList[i].points[2] });
            edges.Add(new int[] { trianglesList[i].points[1], trianglesList[i].points[2] });
        }

        for (int i = 0; i < trianglesList.Count; i++)
        {
            Plane trianglePlane = new Plane(points[trianglesList[i].points[0]], points[trianglesList[i].points[1]], points[trianglesList[i].points[2]]);
            List<Vector2> mappedTriangle = trianglePlane.GetMappedPoints(new List<Vector3>() { points[trianglesList[i].points[0]], points[trianglesList[i].points[1]], points[trianglesList[i].points[2]] });
            BlockingDirection blockedFaces = BlockingDirection.NONE;
            for (int i1 = 0; i1 < points.Count; i1++)
            {
                if (i1 == trianglesList[i].points[0] || i1 == trianglesList[i].points[1] || i1 == trianglesList[i].points[2])
                {
                    //ignore this point if it's one of the triangle's points
                    continue;
                }
                Vector2 mappedPoint = trianglePlane.GetMappedPoint(points[i1]);
                if (!PointInTriangle(mappedPoint, mappedTriangle[0], mappedTriangle[1], mappedTriangle[2]))
                {
                    //ignore this point if it's not behind the triangle
                    continue;
                }
                //rounding accuracy to hundredths place to 
                int pointSign = Math.Sign(Math.Round(trianglePlane.GetDistToPlane(points[i1]), 2));
                if (pointSign == 0)
                {
                    //ignore this point if it's on the same plane
                    continue;
                }

                if (pointSign == 1)
                {
                    if (blockedFaces == BlockingDirection.AWAY_FROM_NORMAL)
                    {
                        blockedFaces = BlockingDirection.BOTH;
                        break;
                    }
                    else
                    {
                        if (drawTriangles > 0)
                        {

                        }
                        blockedFaces = BlockingDirection.TOWARDS_NORMAL;
                    }
                }
                else
                {
                    if (blockedFaces == BlockingDirection.TOWARDS_NORMAL)
                    {
                        blockedFaces = BlockingDirection.BOTH;
                        break;
                    }
                    else
                    {
                        if (drawTriangles > 0)
                        {
                        }
                        blockedFaces = BlockingDirection.AWAY_FROM_NORMAL;
                    }
                }
            }
            Triangle t = new Triangle(trianglesList[i].points, trianglePlane, blockedFaces);
            t.direction = GetBlockingDir(points, trianglesList, edges, i, t.direction, t);
            if(t.GetDirInt() != 0)
            {
                dir = t.direction;
                return i;
            }
        }
        dir = BlockingDirection.NONE;
        return -1;
    }

    private static BlockingDirection GetBlockingDir(List<Vector3> points, List<Triangle> trianglesList, List<int[]> edges, int i, BlockingDirection blockedFaces, Triangle t)
    {
        Plane trianglePlane = new Plane(points[trianglesList[i].points[0]], points[trianglesList[i].points[1]], points[trianglesList[i].points[2]]);
        List<Vector2> mappedTriangle = trianglePlane.GetMappedPoints(new List<Vector3>() { points[trianglesList[i].points[0]], points[trianglesList[i].points[1]], points[trianglesList[i].points[2]] });

        //if this triangle is already blocked in both directions, ignore this triangle.
        if (blockedFaces == BlockingDirection.BOTH)
        {
            return blockedFaces;
        }
        for (int i1 = 0; i1 < edges.Count; i1++)
        {
            // if any of the indexes match, ignore this edge
            bool indexesMatch = false;
            for (int i2 = 0; i2 < 3; i2++)
            {
                for (int i3 = 0; i3 < 2; i3++)
                {
                    if (t.points[i2] == edges[i1][i3])
                    {
                        indexesMatch = true;
                        break;
                    }
                }
                if (indexesMatch) { break; }
            }
            if (indexesMatch)
            {
                continue;
            }

            // if this line doesn't intersect with the triangle, ignore this edge.
            Vector2 mappedPointA = trianglePlane.GetMappedPoint(points[edges[i1][0]]);
            Vector2 mappedPointB = trianglePlane.GetMappedPoint(points[edges[i1][1]]);


            bool lineIntersectsTriangle = false;
            for (int i2 = 0; i2 < 3; i2++)
            {
                Vector2 triangleA = trianglePlane.GetMappedPoint(points[t.points[i2]]);
                Vector2 triangleB = trianglePlane.GetMappedPoint(points[t.points[(i2 + 1) % 3]]);


                if (AreLinesIntersecting(mappedPointA, mappedPointB, triangleA, triangleB, false))
                {
                    lineIntersectsTriangle = true;
                    break;
                }
            }
            if (!lineIntersectsTriangle)
            {
                continue;
            }

            int signA = Math.Sign(Math.Round(trianglePlane.GetDistToPlane(points[edges[i1][0]]), 2));
            int signB = Math.Sign(Math.Round(trianglePlane.GetDistToPlane(points[edges[i1][1]]), 2));

            // if sign A is not the same as sign B, or if sign A or sign B is 0, ignore this edge.
            if (signA != signB || signA == 0 || signB == 0)
            {
                continue;
            }

            if (signA == 1)
            {
                if (blockedFaces == BlockingDirection.AWAY_FROM_NORMAL)
                {
                    blockedFaces = BlockingDirection.BOTH;
                    break;
                }
                else
                {
                    blockedFaces = BlockingDirection.TOWARDS_NORMAL;
                }
            }
            else
            {
                if (blockedFaces == BlockingDirection.TOWARDS_NORMAL)
                {
                    blockedFaces = BlockingDirection.BOTH;
                    break;
                }
                else
                {
                    blockedFaces = BlockingDirection.AWAY_FROM_NORMAL;
                }
            }
        }
        drawTriangles--;
        return blockedFaces;
    }



    static int drawTriangles = -1;
    /* Parameters: 
     * points: points of the mesh
     * faces: indexes of points that make up the triangles of the 3D mesh
     * 
     * Returns:
     * List of Triangles with the direction of their 'normals'/ the direction that the triangles should be rendered in. If a direction can't be determined, then it's left as 0.
     */
    private static List<Triangle> GetKnownTriangles(List<Vector3> points, List<int[]> triangles)
    {
        List<Triangle> trianglesList = new List<Triangle>();
        for(int i = 0; i < triangles.Count; i++)
        {
            Array.Sort(triangles[i]);
        }
        List<int[]> edges = new List<int[]>();
        
        // go through all the points for each triangle, find points that are either in front of or behind those triangles.
        for(int i = 0; i < triangles.Count; i++)
        {
            edges.Add(new int[] { triangles[i][0], triangles[i][1] });
            edges.Add(new int[] { triangles[i][0], triangles[i][2] });
            edges.Add(new int[] { triangles[i][1], triangles[i][2] });

            Plane trianglePlane = new Plane(points[triangles[i][0]], points[triangles[i][1]], points[triangles[i][2]]);
            List<Vector2> mappedTriangle = trianglePlane.GetMappedPoints(new List<Vector3>() { points[triangles[i][0]], points[triangles[i][1]], points[triangles[i][2]] });
            BlockingDirection blockedFaces = BlockingDirection.NONE;
            for (int i1 = 0; i1 < points.Count; i1++)
            {                
                if(i1 == triangles[i][0] || i1 == triangles[i][1] || i1 == triangles[i][2])
                {
                    //ignore this point if it's one of the triangle's points
                    continue;
                }
                Vector2 mappedPoint = trianglePlane.GetMappedPoint(points[i1]);
                if(!PointInTriangle(mappedPoint, mappedTriangle[0], mappedTriangle[1], mappedTriangle[2]))
                {
                    //ignore this point if it's not behind the triangle
                    continue;
                }
                //rounding accuracy to hundredths place to 
                int pointSign = Math.Sign(Math.Round(trianglePlane.GetDistToPlane(points[i1]), 2));
                if(pointSign == 0)
                {
                    //ignore this point if it's on the same plane
                    continue;
                }

                if (pointSign == 1)
                {
                    if (blockedFaces == BlockingDirection.AWAY_FROM_NORMAL)
                    {
                        blockedFaces = BlockingDirection.BOTH;
                        break;
                    }
                    else
                    {
                        if(drawTriangles > 0)
                        {
                           
                        }
                        blockedFaces = BlockingDirection.TOWARDS_NORMAL;
                    }
                }
                else
                {
                    if (blockedFaces == BlockingDirection.TOWARDS_NORMAL)
                    {
                        blockedFaces = BlockingDirection.BOTH;
                        break;
                    }
                    else
                    {
                        if (drawTriangles > 0)
                        {                          
                        }
                        blockedFaces = BlockingDirection.AWAY_FROM_NORMAL;
                    }
                }
            }
            trianglesList.Add(new Triangle (triangles[i], trianglePlane, blockedFaces));
        }

        drawTriangles = 1;
        // go through all the connections for each triangle, find lines that are either in front of or behind those triangles.
        for (int i = 0; i < trianglesList.Count; i++)
        {
            Plane trianglePlane = new Plane(points[triangles[i][0]], points[triangles[i][1]], points[triangles[i][2]]);
            List<Vector2> mappedTriangle = trianglePlane.GetMappedPoints(new List<Vector3>() { points[triangles[i][0]], points[triangles[i][1]], points[triangles[i][2]] });
            BlockingDirection blockedFaces = trianglesList[i].direction;
            
            //if this triangle is already blocked in both directions, ignore this triangle.
            if (blockedFaces == BlockingDirection.BOTH)
            {
                continue;
            }
            for (int i1 = 0; i1 < edges.Count; i1++)
            {
                // if any of the indexes match, ignore this edge
                bool indexesMatch = false;
                for (int i2 = 0; i2 < 3; i2++)
                {
                    for (int i3 = 0; i3 < 2; i3++)
                    {
                        if (trianglesList[i].points[i2] == edges[i1][i3])
                        {
                            indexesMatch = true;
                            break;
                        }
                    }
                    if (indexesMatch) { break; }
                }
                if (indexesMatch)
                {
                    continue;
                }

                // if this line doesn't intersect with the triangle, ignore this edge.
                Vector2 mappedPointA = trianglePlane.GetMappedPoint(points[edges[i1][0]]);
                Vector2 mappedPointB = trianglePlane.GetMappedPoint(points[edges[i1][1]]);
                
                
                bool lineIntersectsTriangle = false;
                for (int i2 = 0; i2 < 3; i2++)
                {
                    Vector2 triangleA = trianglePlane.GetMappedPoint(points[trianglesList[i].points[i2]]);
                    Vector2 triangleB = trianglePlane.GetMappedPoint(points[trianglesList[i].points[(i2 + 1) % 3]]);
                    

                    if (AreLinesIntersecting(mappedPointA, mappedPointB, triangleA, triangleB, false))
                    {
                        lineIntersectsTriangle = true;
                        break;
                    }
                }
                if (!lineIntersectsTriangle)
                {
                    continue;
                }

                int signA = Math.Sign(Math.Round(trianglePlane.GetDistToPlane(points[edges[i1][0]]), 2));
                int signB = Math.Sign(Math.Round(trianglePlane.GetDistToPlane(points[edges[i1][1]]), 2));

                // if sign A is not the same as sign B, or if sign A or sign B is 0, ignore this edge.
                if (signA != signB || signA == 0 || signB == 0)
                {
                    continue;
                }

                if (signA == 1)
                {
                    if (blockedFaces == BlockingDirection.AWAY_FROM_NORMAL)
                    {
                        blockedFaces = BlockingDirection.BOTH;
                        break;
                    }
                    else
                    {
                        blockedFaces = BlockingDirection.TOWARDS_NORMAL;
                    }
                }
                else
                {
                    if (blockedFaces == BlockingDirection.TOWARDS_NORMAL)
                    {
                        blockedFaces = BlockingDirection.BOTH;
                        break;
                    }
                    else
                    {
                        blockedFaces = BlockingDirection.AWAY_FROM_NORMAL;
                    }
                }
            }
            drawTriangles--;
            trianglesList[i].direction = blockedFaces;
        }
        
        return trianglesList;
    }

    /* Parameters: 
     * triangle: the triangle that we're trying to get the neighbors of
     * allTriangles: all the triangles in the mesh
     * 
     * Returns:
     * List of all the triangles that are next to the one passed in
     */
    public static List<Triangle> GetNeighboringTriangles(Triangle triangle, List<int>[] map, List<Triangle> allTriangles, List<int>[][] trianglesHash)
    {
        List<Triangle> neighbors = new List<Triangle>();
        for(int i = 0; i < 3; i++)
        {
            int index1 = triangle.points[i];
            int index2 = triangle.points[(i + 1) % 3];
            int index = -1;
            for(int i1 = 0; i1 < map[index1].Count; i1++)
            {
                if(map[index1][i1] == index2)
                {
                    index = i1;
                    break;
                }
            }
            List<Triangle> possibleNeighbors = trianglesHash[index1][index].Select(x => allTriangles[x]).ToList();
            foreach(Triangle t in possibleNeighbors)
            {
                if (!t.Equals(triangle) && !neighbors.Contains(t) && 
                    !(t.points.Contains(triangle.points[0]) && t.points.Contains(triangle.points[1]) && t.points.Contains(triangle.points[2])))
                {
                    neighbors.Add(t);                        
                }
            }
        }
        return neighbors;
    }

    private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    /* Parameters: 
     * points: points of the mesh
     * faces: indexes of points that make up the 'faces' or 'sides' of the 3D mesh
     * 
     * Returns:
     * Mesh object that can be attatched to a GameObject to display the 3D mesh/shape
     */
    public static bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
    {
        if(pt.Equals(v1) || pt.Equals(v2) || pt.Equals(v3))
        {
            return false;
        }
        float d1, d2, d3;
        bool has_neg, has_pos;

        d1 = Sign(pt, v1, v2);
        d2 = Sign(pt, v2, v3);
        d3 = Sign(pt, v3, v1);

        has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }

    /* Parameters: 
     * points: points of the mesh
     * faces: indexes of points that make up the 'faces' or 'sides' of the 3D mesh
     * 
     * Returns:
     * Mesh object that can be attatched to a GameObject to display the 3D mesh/shape
     */
    private static bool PointOnTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
    {
        Vector2[] triangle = new Vector2[] { v1, v2, v3 };
        for (int i = 0; i < triangle.Length; i++)
        {
            LineSegment2D line = new LineSegment2D(triangle[i], triangle[(i + 1) % triangle.Length]);
            if (line.Intersects(pt))
            {
                return true;
            }
        }
        return false;
    }

    /* Parameters: 
     * t: triangle that we're trying to get the Centroid of.
     * points: list of all the points of the mesh.
     * 
     * Returns:
     * Centroid of triangle
     * 
     * NOTE: information on what a centroid is https://en.wikipedia.org/wiki/Centroid
     */
    private static Vector3 GetCentroid(Triangle t, List<Vector3> points)
    {
        float x = (points[t.points[0]].x + points[t.points[1]].x + points[t.points[2]].x)/3;
        float y = (points[t.points[0]].y + points[t.points[1]].y + points[t.points[2]].y)/3;
        float z = (points[t.points[0]].z + points[t.points[1]].z + points[t.points[2]].z)/3;
        return new Vector3(x, y, z);
    }


    private static Vector3 GetCentroid(Vector3 a, Vector3 b, Vector3 c)
    {
        float x = (a.x + b.x + c.x) / 3;
        float y = (a.y + b.y + c.y) / 3;
        float z = (a.z + b.z + c.z) / 3;
        return new Vector3(x, y, z);
    }
    #endregion

    

    public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
    {
        //To avoid floating point precision issues we can add a small value
        float epsilon = 0.00001f;

        bool isIntersecting = false;

        float denominator = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);

        //Make sure the denominator is > 0, if not the lines are parallel
        if (denominator != 0f)
        {
            float u_a = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;
            float u_b = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;

            //Are the line segments intersecting if the end points are the same
            if (shouldIncludeEndPoints)
            {
                //Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
                if (u_a >= 0f + epsilon && u_a <= 1f - epsilon && u_b >= 0f + epsilon && u_b <= 1f - epsilon)
                {
                    isIntersecting = true;
                }
            }
            else
            {
                //Is intersecting if u_a and u_b are between 0 and 1
                if (u_a > 0f + epsilon && u_a < 1f - epsilon && u_b > 0f + epsilon && u_b < 1f - epsilon)
                {
                    isIntersecting = true;
                }
            }
        }

        return isIntersecting;
    }

    public static float AngleBetweenPlanes(Vector3 n1, Vector3 n2)
    {
        return Mathf.Acos((n1.x * n2.x + n1.y * n2.y + n1.z * n2.z) / (n1.magnitude * n2.magnitude)) * Mathf.Rad2Deg;
    }

    
}
public enum BlockingDirection { NONE, TOWARDS_NORMAL, AWAY_FROM_NORMAL, BOTH }

public class Triangle
{
    public int[] points;
    public Plane plane;
    public BlockingDirection direction;

    public Triangle(int[] points, Plane plane, BlockingDirection direction)
    {
        this.points = points;
        this.plane = plane;
        this.direction = direction;
    }

    public int GetDirInt()
    {
        if (direction == BlockingDirection.TOWARDS_NORMAL)
        {
            return -1;
        }
        else if (direction == BlockingDirection.AWAY_FROM_NORMAL)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}