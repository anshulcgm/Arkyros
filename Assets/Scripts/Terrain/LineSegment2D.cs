using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class LineSegment2D
{
    //start and finish points of line segment
    Vector2 start, finish;

    //A, B, and C where the line is represented as Ax + By = C.
    float A, B, C;

    public LineSegment2D(Vector2 start, Vector2 finish)
    {
        this.start = start;
        this.finish = finish;
        A = finish.y - start.y;
        B = finish.x - start.x;
        C = A * start.x + B * start.y;
    }

    /* Parameters: 
     * other: another 2D line segment
     * 
     * Returns:
     * whether the other line segment intersects this one
     */
    public bool Intersects(LineSegment2D other)
    {
        float delta = A * other.B - other.A * B;
        if (delta == 0)
        {
            return false;
        }
        float x = (other.B * C - B * other.C) / delta;
        float y = (A * other.C - other.A * C) / delta;
        return Intersects(new Vector2(x, y));
    }

    /* Parameters: 
     * point: 2D point
     * 
     * Returns:
     * whether this line segment passes through the point
     */
    public bool Intersects(Vector2 point)
    {
        return Mathf.Abs(A * point.x + B * point.y - C) < Mathf.Epsilon && (Vector2.SqrMagnitude(finish - start) - 
                                                                            Vector2.SqrMagnitude(point - start) - 
                                                                            Vector2.SqrMagnitude(point - finish)) < Mathf.Epsilon;
    }
}