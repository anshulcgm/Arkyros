using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this class builds meshes given edges and points as input
public class MeshBuilder3D
{
    /* Parameters: 
     * points: List of Vector3 objects containing points that represent the verticies of the mesh
     * triangles: List of int[]'s that contain 3 indexes per int[]. Each int[] represents one triangle of the mesh. Indexes within each int[] do not need to be sorted in any order
     * 
     * Returns:
     * Mesh object that can be assigned to a GameObject to display the mesh.
     */
    public static Mesh GetMeshFrom(List<Vector3> points, List<int[]> trianglesIndexes)
    {
        // gets all the triangles
        List<Triangle> allTriangles = GetKnownTriangles(points, trianglesIndexes);
        List<Triangle> trianglesWithKnownDirection = new List<Triangle>();
        for(int i = 0; i < allTriangles.Count; i++)
        {
            if(allTriangles[i].direction != 0)
            {
                trianglesWithKnownDirection.Add(allTriangles[i]);
            }
        }

        int index = 0;
        int iterations = 0;
        while(trianglesWithKnownDirection.Count < allTriangles.Count && iterations < 1000)
        {
            Triangle t = trianglesWithKnownDirection[index];
            List<Triangle> neighbors = GetNeighboringTriangles(t, allTriangles);
            for(int i1 = 0; i1 < neighbors.Count; i1++)
            {
                Triangle neighbor = neighbors[i1];
                if(neighbor.direction == 0)
                {
                    Vector3 centerA = GetCentroid(t, points);
                    Vector3 centerB = GetCentroid(neighbor, points);
                    float angleA = Vector3.Angle(t.direction * t.normal, centerB - centerA);
                    float angleB = Vector3.Angle(neighbor.normal, centerA - centerB);
                    float angleC = Vector3.Angle(-neighbor.normal, centerA - centerB);
                    if(Mathf.Abs(angleA - angleB) < Mathf.Abs(angleA - angleC))
                    {
                        neighbor.direction = 1;
                    }
                    else
                    {
                        neighbor.direction = -1;
                    }
                    trianglesWithKnownDirection.Add(neighbor);
                }
            }
            if (index < trianglesWithKnownDirection.Count - 1)
            {
                index++;
            }            
            iterations++;
        }

        int[][] faces = new int[allTriangles.Count][];
        for(int i = 0; i < allTriangles.Count; i++)
        {
            Vector3 a = points[allTriangles[i].points[0]];
            Vector3 b = points[allTriangles[i].points[1]];
            Vector3 c = points[allTriangles[i].points[2]];
            int[] triangle = new int[3];
            if (!IsClockwise(b - a, c - b, allTriangles[i].normal * allTriangles[i].direction))
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
            faces[i] = triangle;

            Debug.DrawLine(GetCentroid(allTriangles[i], points), GetCentroid(allTriangles[i], points) + allTriangles[i].normal * allTriangles[i].direction, Color.red, 1000);
        }
        return GetMesh(points.ToArray(), faces);
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
        return (normal.normalized - crossProd.normalized).magnitude < Mathf.Epsilon;
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
    static Mesh GetMesh(Vector3[] points, int[][] faces)
    {
        //get mesh and coll
        Mesh mesh = new Mesh();

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
        for (int i = 0; i < faces.Length; i++)
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
        return mesh;
    }

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
        for (int i = 0; i < triangles.Count; i++)
        {
            // get points of triangle from points list.
            Vector3 p0 = points[triangles[i][0]];
            Vector3 p1 = points[triangles[i][1]];
            Vector3 p2 = points[triangles[i][2]];

            // make a plane
            Plane trianglePlane = new Plane(p0, p1, p2);


            BlockingDirection blockedFaces = BlockingDirection.NONE;
            for (int i1 = 0; i1 < points.Count; i1++)
            {
                // if the index matches one of the points of the above triangle, skip the point.
                if (i1 == triangles[i][0] || i1 == triangles[i][1] || i1 == triangles[i][2]) { continue; }

                Vector3 point = points[i1];

                // get signed distance to the plane and the closest point on the plane.
                float distToPlane = trianglePlane.GetDistToPlane(point);
                Vector3 closestPointOnPlane = trianglePlane.GetLineIntersect(point, trianglePlane.normal);

                // get 2D representation of triangle and of the point relative to the plane
                List<Vector3> triangle = new List<Vector3> { p0, p1, p2 };
                Vector3 origin = p0;
                
                List<Vector2> mappedTriangle = trianglePlane.GetMappedPoints(triangle);
                Vector2 mappedPoint = trianglePlane.GetMappedPoint(closestPointOnPlane);

                // if point is inside triangle, but not on the lines of the triangle.
                if (PointInTriangle(mappedPoint, mappedTriangle[0], mappedTriangle[1], mappedTriangle[2]))
                {
                    if (distToPlane < 0)
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
                    else if (distToPlane > 0)
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
            }

            if (blockedFaces == BlockingDirection.NONE)
            {
                Triangle t = new Triangle { points = triangles[i], normal = trianglePlane.normal, direction = 0 };
                trianglesList.Add(t);
            }
            else if (blockedFaces == BlockingDirection.TOWARDS_NORMAL)
            {
                Triangle t = new Triangle { points = triangles[i], normal = trianglePlane.normal, direction = 1};
                trianglesList.Add(t);
            }
            else if (blockedFaces == BlockingDirection.AWAY_FROM_NORMAL)
            {
                Triangle t = new Triangle { points = triangles[i], normal = trianglePlane.normal, direction = -1};
                trianglesList.Add(t);
            }
            else if (blockedFaces == BlockingDirection.BOTH)
            {
                Triangle t = new Triangle { points = triangles[i], normal = trianglePlane.normal, direction = 0};
                trianglesList.Add(t);
            }
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
    private static List<Triangle> GetNeighboringTriangles(Triangle triangle, List<Triangle> allTriangles)
    {
        List<Triangle> neighbors = new List<Triangle>();
        foreach(Triangle t in allTriangles)
        {
            if(t.points.Contains(triangle.points[0]) && t.points.Contains(triangle.points[1]) && t.points.Contains(triangle.points[2]))
            {
                continue;
            }
            for (int i = 0; i < triangle.points.Length; i++)
            {
                for (int i1 = 0; i1 < t.points.Length; i1++)
                {                    
                    int p0 = triangle.points[i];
                    int p1 = t.points[i1];
                    int p2 = triangle.points[(i + 1) % triangle.points.Length];
                    int p3 = t.points[(i1 + 1) % t.points.Length];
                    if ((p0 == p1 && p2 == p3) || (p0 == p2 && p1 == p3))
                    {
                        neighbors.Add(t);
                        if (neighbors.Count > 3)
                        {
                            throw new Exception("The number of neighboring triangles per triangle should not exceed 3.");
                        }
                    }
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
    private static bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
    {
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
    #endregion

    private class Triangle
    {
        public int[] points;
        public Vector3 normal;
        public int direction;
    }
    private enum BlockingDirection {NONE, TOWARDS_NORMAL, AWAY_FROM_NORMAL, BOTH}
}