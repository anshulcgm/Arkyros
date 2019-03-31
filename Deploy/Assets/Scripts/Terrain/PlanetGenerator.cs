using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator
{
    public static List<int[]> shownCons = new List<int[]>();
    //radius is (minimum) radius of the planet AKA distance from core to surface corners/points
    //variance is maximum radius - minimum, AKA variance is maximum radius added at any 1 point/corner
    //variance seed is just the random seed
    //numPointsLat is number of points around (horizontally) on the sphere
    //numPointsLong is number of layers of points (layers horizontal and vertical in a way too)
    //points is an output bascially
    //cons (connections) is another output basically
    public static void MakePlanet(float radius, float variance, int varianceSeed, int numPointsLat, int numPointsLong, out List<Vector3> points, out List<int[]> cons)
    {
        points = new List<Vector3>();
        cons = new List<int[]>();
        System.Random rand = new System.Random(varianceSeed);

        //making points
        for (double theta = -Math.PI / 2; theta <= Math.PI / 2; theta += Math.PI / (numPointsLong + 1))
        {
            for (double rot = 0; rot <= 2 * Math.PI; rot += 2 * Math.PI / (numPointsLat + 1))
            {
                double rad = radius + rand.NextDouble() * variance;
                float x = (float)(Math.Cos(rot) * Math.Cos(theta) * (rad));
                float y = (float)(Math.Sin(theta) * (rad));
                float z = (float)(Math.Sin(rot) * Math.Cos(theta) * (rad));
                points.Add(new Vector3(x, y, z));
            }
        }
        //remove copies of points
        for (int i = 0; i < numPointsLong; i++)
        {
            points.RemoveAt(0);
        }

        for (int i = 0; i < numPointsLong - 2; i++)
        {
            points.RemoveAt(points.Count - 1);
        }

        //making cons
        //connect 0 up (bottom point)                   --connect range [next point, numPointsLat]
        for (int i = 1; i <= numPointsLat; i++)
        {
            cons.Add(new int[] { 0, i });
        }
        bool first = true;
        //run through all but bottom point and top cirlce
        for (int i = 1; i < (points.Count - numPointsLat - 1); i++)
        {
            if (i % numPointsLat == 0) //if last in rot
            //connect all but top cricle last in rot        --connect +numPointsLat and +1-numPointsLat
            {
                cons.Add(new int[] { i, i + numPointsLat + 1 });
                cons.Add(new int[] { i, i + numPointsLat });
                cons.Add(new int[] { i, i + 1});
                if (first) {
                    shownCons.Add(new int[] { i, i + numPointsLat + 1 });
                    shownCons.Add(new int[] { i, i + 1 });
                    shownCons.Add(new int[] { i, i + numPointsLat });
                    first = false;
                }
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
        List<int>[] map = GetMap(cons);
        int num = rand.Next(2, 8);
        for (int i = 0; i < num; i++)
        {
            int index = rand.Next(0, points.Count);
            points[index] = points[index].normalized * radius * 0.75f;
            points = Smooth(index, 10, 0.5f, map, points);
        }

        num = rand.Next(2, 6);
        for (int i = 0; i < num; i++)
        {
            int index = rand.Next(0, points.Count);
            points[index] = points[index].normalized * radius * 1.4f;
            points = Smooth(index, 10, 0.5f, map, points);
        }
    }

    private struct Tuple
    {
        public int index;
        public int radialDist;
    }

    private static List<Vector3> Smooth(int indexStart, int maxRadius, float smoothFactor, List<int>[] map, List<Vector3> points)
    {
        List<Tuple> indexesToSmooth = new List<Tuple> { new Tuple { index = indexStart, radialDist = 0 } };
        List<int> smoothedIndexes = new List<int>();
        List<int> runIndexes = new List<int>();

        List<Vector3> newPoints = new List<Vector3>();
        for (int i = 0; i < points.Count; i++)
        {
            newPoints.Add(new Vector3(points[i].x, points[i].y, points[i].z));
        }

        while (indexesToSmooth.Count > 0)
        {
            Tuple t = indexesToSmooth[0];
            indexesToSmooth.RemoveAt(0);
            runIndexes.Add(t.index);

            if (t.radialDist > maxRadius)
            {
                continue;
            }

            float smoothRad = newPoints[t.index].magnitude;
            for (int i = 0; i < map[t.index].Count; i++)
            {
                if (!smoothedIndexes.Contains(map[t.index][i]))
                {
                    Vector3 point = newPoints[map[t.index][i]];
                    float pointRad = point.magnitude;
                    float lerpRad = Mathf.Lerp(pointRad, smoothRad, smoothFactor);
                    newPoints[map[t.index][i]] = newPoints[map[t.index][i]].normalized * lerpRad;
                    smoothedIndexes.Add(map[t.index][i]);
                }
                if (!runIndexes.Contains(map[t.index][i]))
                {
                    indexesToSmooth.Add(new Tuple { index = map[t.index][i], radialDist = t.radialDist + 1 });
                }
            }
        }
        return newPoints;
    }

    private static List<int>[] GetMap(List<int[]> cons)
    {
        int max = 0;
        foreach (int[] con in cons)
        {
            if (con[0] > max)
            {
                max = con[0];
            }
            if (con[1] > max)
            {
                max = con[1];
            }
        }

        List<int>[] map = new List<int>[max + 1];
        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new List<int>();
            foreach (int[] con in cons)
            {
                if (con[0] == i)
                {
                    map[i].Add(con[1]);
                }
                if (con[1] == i)
                {
                    map[i].Add(con[0]);
                }
            }
        }
        return map;
    }
}

