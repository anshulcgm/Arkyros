using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this class builds meshes given edges and points as input
public class MeshBuilder3D
{

    public static Mesh GetMeshFrom(List<Vector3> points, List<int[]> trianglesIndexes, out List<Vector3> normals)
    {
        normals = new List<Vector3>();
        // gets all the triangles
        List<Triangle> allTriangles = GetKnownTriangles(points, trianglesIndexes);
        List<Triangle> trianglesWithKnownDirection = new List<Triangle>();
        for (int i = 0; i < allTriangles.Count; i++)
        {
            if (allTriangles[i].GetDirInt() != 0)
            {
                trianglesWithKnownDirection.Add(allTriangles[i]);
                /*
                Debug.DrawLine(points[allTriangles[i].points[0]] * 4000, points[allTriangles[i].points[1]] * 4000, Color.red, 100000);
                Debug.DrawLine(points[allTriangles[i].points[1]] * 4000, points[allTriangles[i].points[2]] * 4000, Color.red, 100000);
                Debug.DrawLine(points[allTriangles[i].points[0]] * 4000, points[allTriangles[i].points[2]] * 4000, Color.red, 100000);
                */
                
                
                Vector3 centroid = GetCentroid(allTriangles[i], points);
                Vector3 normal = allTriangles[i].normal * allTriangles[i].GetDirInt();
                Plane triPlane = new Plane(normal, centroid);

                /*
                if(allTriangles[i].GetDirInt() == 1)
                    Debug.DrawLine(centroid * 4000, centroid * 4000 + normal * 100, Color.cyan, 100000);
                else
                    Debug.DrawLine(centroid * 4000, centroid * 4000 + normal * 100, Color.green, 100000);
                    */
            }
        }

        int index = 0;
        while (trianglesWithKnownDirection.Count < allTriangles.Count)
        {
            Triangle t = trianglesWithKnownDirection[index];
            Plane tPlane = new Plane(t.normal * t.GetDirInt(), points[t.points[0]]);
            List<Triangle> neighbors = GetNeighboringTriangles(t, allTriangles);
            for (int i1 = 0; i1 < neighbors.Count; i1++)
            {
                Triangle neighbor = neighbors[i1];
                Plane neighborPlane = new Plane(neighbor.normal, points[neighbor.points[0]]);
                if (neighbor.GetDirInt() == 0)
                {
                    Vector3 centerA = GetCentroid(t, points);
                    Vector3 centerB = GetCentroid(neighbor, points);
                    float distA = tPlane.GetDistToPlane(centerB);
                    float distB = neighborPlane.GetDistToPlane(centerA);

                    bool a = Math.Sign(distA) == Math.Sign(distB);
                    if (a)
                    {
                        neighbor.direction = BlockingDirection.AWAY_FROM_NORMAL;
                        

                        Vector3 center = GetCentroid(neighbor, points);
                    }
                    else
                    {
                        neighbor.direction = BlockingDirection.TOWARDS_NORMAL;
                        

                        Vector3 center = GetCentroid(neighbor, points);
                    }
                    trianglesWithKnownDirection.Add(neighbor);
                }
            }
            if (index < trianglesWithKnownDirection.Count - 1)
            {
                index++;
            }
            else
            {
                throw new Exception("wat");
            }
        }
        

        int[][] faces = new int[allTriangles.Count][];
        for (int i = 0; i < allTriangles.Count; i++)
        {
            Array.Sort(allTriangles[i].points);
            Vector3 a = points[allTriangles[i].points[0]];
            Vector3 b = points[allTriangles[i].points[1]];
            Vector3 c = points[allTriangles[i].points[2]];
            normals.Add(allTriangles[i].normal * allTriangles[i].GetDirInt());
            int[] triangle = new int[3];
            if (!IsClockwise(b - a, c - a, allTriangles[i].normal * allTriangles[i].GetDirInt()))
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

            //Debug.DrawLine(GetCentroid(allTriangles[i], points) * 1000, GetCentroid(allTriangles[i], points) * 1000 + allTriangles[i].normal * allTriangles[i].direction * 1000, Color.red, 1000);
        }
        return GetMesh(points.ToArray(), faces);
    }


    /* Parameters: 
     * points: List of Vector3 objects containing points that represent the verticies of the mesh
     * triangles: List of int[]'s that contain 3 indexes per int[]. Each int[] represents one triangle of the mesh. Indexes within each int[] do not need to be sorted in any order
     * 
     * Returns:
     * Mesh object that can be assigned to a GameObject to display the mesh.
     */
    public static Mesh GetMeshFrom(List<Vector3> points, List<int[]> trianglesIndexes)
    {
        List<Vector3> normals = null;
        return GetMeshFrom(points, trianglesIndexes, out normals);
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
    static Mesh GetMesh(Vector3[] points, int[][] faces)
    {
        //get mesh and coll
        Mesh mesh = new Mesh();
        /*
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
        */
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
            trianglesList.Add(new Triangle (triangles[i], trianglePlane.normal, blockedFaces));
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
    private static List<Triangle> GetNeighboringTriangles(Triangle triangle, List<Triangle> allTriangles)
    {
        List<Triangle> neighbors = new List<Triangle>();
        foreach(Triangle t in allTriangles)
        {
            if(t.points.Contains(triangle.points[0]) && t.points.Contains(triangle.points[1]) && t.points.Contains(triangle.points[2]))
            {
                continue;
            }
            bool addedTriangle = false;
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
                        if (neighbors.Count == 3)
                        {
                            Debug.Log("this is messed up ");
                            return neighbors;
                            //throw new Exception("The number of neighboring triangles per triangle should not exceed 3.");
                        }
                        neighbors.Add(t);
                        addedTriangle = true;
                        
                        continue;
                    }
                }
                if (addedTriangle)
                {
                    continue;
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

    private class Triangle
    {
        public int[] points;
        public Vector3 normal;
        public BlockingDirection direction;

        public Triangle(int[] points, Vector3 normal, BlockingDirection direction)
        {
            this.points = points;
            this.normal = normal;
            this.direction = direction;
        }

        public int GetDirInt()
        {
            if(direction == BlockingDirection.TOWARDS_NORMAL)
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

    private enum BlockingDirection {NONE, TOWARDS_NORMAL, AWAY_FROM_NORMAL, BOTH}
}