using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMaker
{
    public static void MakePlanet(float radius, float variance, int varianceSeed, int numPointsLat, int numPointsLong, out List<Vector3> points, out List<int[]> cons)
    //radius is (minimum) radius of the planet AKA distance from core to surface corners/points
    //variance is maximum radius - minimum, AKA variance is maximum radius added at any 1 point/corner
    //variance seed is just the random seed
    //numPointsLat is number of points around (horizontally) on the sphere
    //numPointsLong is number of layers of points (layers horizontal and vertical in a way too)
    //points is an output bascially
    //cons (connections) is another output basically
    {
        points = new List<Vector3>();
        cons = new List<int[]>();
        System.Random rand = new System.Random(varianceSeed);

        //making points
        for (double theta = -Math.PI / 2; theta < Math.PI / 2; theta += Math.PI / numPointsLong)
        {
            for (double rot = 0; rot < 2 * Math.PI; rot += 2 * Math.PI / numPointsLat)
            {
                float x = (float)(Math.Cos(rot) * Math.Cos(theta) * (radius + rand.NextDouble() * variance));
                float y = (float)(Math.Sin(theta) * (radius + rand.NextDouble() * variance));
                float z = (float)(Math.Sin(rot) * Math.Cos(theta) * (radius + rand.NextDouble() * variance));
                points.Add(new Vector3(x, y, z));
            }
        }
        //remove copies of points
        for (int i = 0; i < numPointsLong - 1; i++)
        {
            points.RemoveAt(0);
            points.RemoveAt(points.Count - 1);
        }

        //making cons
        //connect 0 up (bottom point)                   --connect range [next point, numPointsLat]
        for (int i = 1; i <= numPointsLat; i++)
        {
            cons.Add(new int[] { 0, i });
        }
        //run through all but bottom point and top cirlce
        for (int i = 1; i < (points.Count - numPointsLat - 1); i++)
        {
            if (i % numPointsLat == numPointsLat - 1) //if last in rot
            //connect all but top cricle last in rot        --connect +numPointsLat and +1-numPointsLat
            {
                cons.Add(new int[] { i, i + 1 - numPointsLat });
                cons.Add(new int[] { i, i + numPointsLat });
                cons.Add(new int[] { i, i + 1 });
            }
            else //connect all but top circle and last in rot    --connect next point and +numPointsLat and +numPointsLat+1
            {
                cons.Add(new int[] { i, i + 1 });
                cons.Add(new int[] { i, i + numPointsLat });
                cons.Add(new int[] { i, i + numPointsLat + 1 });
            }
        }
        //connect top circle and but not last in rot            --connect last and next point
        for (int i = (points.Count - numPointsLat - 1); i < (points.Count - 1) - 1; i++)
        {
            cons.Add(new int[] { i, points.Count - 1 });
            cons.Add(new int[] { i, i + 1 });
        }
        //connect top circle last in rot                --connect last and +1-numPointsLat
        cons.Add(new int[] { points.Count - 2, points.Count - 1 });
        cons.Add(new int[] { points.Count - 2, points.Count - numPointsLat });
    }
}
