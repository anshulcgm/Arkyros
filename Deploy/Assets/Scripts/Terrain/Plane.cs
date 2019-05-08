using System;
using System.Collections.Generic;
using UnityEngine;

//class that represents a plane in 3D space. Has useful functions for mesh making.
public class Plane
{
    public Vector3 normal;
    public float d;
    // x and y directions on the plane (for conversion of 3D points to 2D)
    public Vector3 xDir = Vector3.zero;
    public Vector3 yDir = Vector3.zero;

    public Plane(Vector3 normal, float d)
    {
        this.normal = normal;
        this.d = d;
        GetDirs();
    }

    public Plane(Vector3 normal, Vector3 point)
    {
        this.normal = normal;
        d = -Vector3.Dot(point, normal);
        GetDirs();
    }

    public Plane(Vector3 e, Vector3 f, Vector3 g)
    {
        Vector3 v1, v2;
        v1 = f - e;
        v2 = g - e;
        normal = Vector3.Cross(v1, v2);
        d = -Vector3.Dot(e, normal);
        GetDirs();
    }

    //Description: gets the x and y directions of the plane and sets the variables. Called in constructor.
    void GetDirs()
    {
        Vector3 point = GetArbitraryPointOnPlane();
        Vector3 U = (point - GetArbitraryPointOnPlaneThatIsNot(point)).normalized;
        xDir = (U - Vector3.Dot(U, normal) * normal).normalized;
        yDir = Vector3.Cross(normal, xDir).normalized;
        xDir = xDir.normalized;
        yDir = yDir.normalized;
    }

    /* Returns:
     * an arbitrary, NOT RANDOM, point on the plane.
     * Description: The point will be the same if you call the function again, given that the plane is the same. Will never return Vector3.zero.
     */
    public Vector3 GetArbitraryPointOnPlane()
    {
        return GetArbitraryPointOnPlaneThatIsNot(Vector3.zero);
    }

    /* Returns:
     * an arbitrary, NOT RANDOM, point on the plane, that is not the Vector3 passed in.
     * Description: The point will be the same if you call the function again with the same parameter given that the plane is the same.
     */
    public Vector3 GetArbitraryPointOnPlaneThatIsNot(Vector3 other)
    {
        float val = 1;
        while (Mathf.Abs(other.x - val) < Mathf.Epsilon || Mathf.Abs(other.y - val) < Mathf.Epsilon || Mathf.Abs(other.z - val) < Mathf.Epsilon)
        {
            val++;
        }

        float x, y, z;
        if (normal.normalized.z != 0)
        {
            x = val;
            y = val;
            z = (d - normal.normalized.x * x - normal.normalized.y * y) / (normal.normalized.z);
        }
        else if (normal.normalized.y != 0)
        {
            x = val;
            z = val;
            y = (d - normal.normalized.x * x - normal.normalized.z * z) / (normal.normalized.y);
        }
        else
        {
            y = val;
            z = val;
            x = (d - normal.normalized.y * y - normal.normalized.z * z) / (normal.normalized.x);
        }
        return new Vector3(x, y, z);
    }

    /* Parameters:
     * linePoint: point on the line intersectng the plane
     * lineDirection: the direction that the line is pointing in
     * 
     * Returns:
     * point where the line intersects the plane.
     */
    public Vector3 GetLineIntersect(Vector3 linePoint, Vector3 lineDirection)
    {
        Vector3 planePoint = GetArbitraryPointOnPlane();
        float t = (Vector3.Dot(normal.normalized, planePoint) - Vector3.Dot(normal.normalized, linePoint)) / Vector3.Dot(normal.normalized, lineDirection.normalized);
        return linePoint + lineDirection.normalized * t;
    }

    /* Parameters:
     * point: point in space
     * 
     * Returns:
     * signed distance to plane (negative means below the plane, positive means above)
     */
    public float GetDistToPlane(Vector3 point)
    {
        return (Vector3.Dot(normal, point) + d)/normal.magnitude;
    }

    /* Parameters:
     * org: origional vector
     * angle: angle to rotate by in radians
     * 
     * Returns:
     * new vector, rotated by angle radians clockwise along the plane
     */
    public Vector3 GetRotatedVectorAlongPlane(Vector3 org, float angle)
    {
        return Quaternion.AngleAxis(angle * Mathf.Rad2Deg, normal) * org;
    }

    /* Parameters:
     * other: the other plane
     * 
     * Returns:
     * angle in radians between planes
     */
    public float AngleBetweenPlanes(Plane other)
    {
        return Mathf.Acos(Vector3.Dot(normal, other.normal) / Mathf.Sqrt(normal.sqrMagnitude * other.normal.sqrMagnitude));
    }

    /* Parameters:
     * unMappedPoints: list of points in space that may or may not be on the plane
     * 
     * Returns:
     * list of 2D points that are on the plane
     */
    public List<Vector2> GetMappedPoints(List<Vector3> unMappedPoints)
    {
        List<Vector2> mappedPoints = new List<Vector2>();
        foreach (Vector3 pt in unMappedPoints)
        {
            mappedPoints.Add(GetMappedPoint(pt));
        }
        return mappedPoints;
    }

    /* Parameters:
     * unMappedPoint: point in space that may or may not be on the plane
     * 
     * Returns:
     * 2D point on the plane
     */
    public Vector2 GetMappedPoint(Vector3 unMappedPoint)
    {
        Vector3 unMappedPointOnPlane = GetDistToPlane(unMappedPoint) * -normal.normalized + unMappedPoint;
        Vector2 posMapped = new Vector2(Vector3.Dot(unMappedPointOnPlane, xDir), Vector3.Dot(unMappedPointOnPlane, yDir));
        return posMapped;
    }

    ///@TODO remove dependencies on this method
    public Vector3 GetParallelFrom(Vector3 v)
    {
        float angle = 0;

        // Generate a uniformly-distributed unit vector in the XY plane.
        Vector3 inPlane = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

        // Rotate the vector into the plane perpendicular to v and return it.
        return Quaternion.LookRotation(v) * inPlane;
    }
}