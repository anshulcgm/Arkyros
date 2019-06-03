using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator
{
    public static List<int[]> shownCons = new List<int[]>();
    //radius is (minimum) radius of the planet AKA distance from core to surface corners/points
    //variance is maximum radius - minimum, AKA variance is maximum radius added at any 1 point/corner
    //variance seed is just the rom seed
    //numPointsLat is number of points around (horizontally) on the sphere
    //numPointsLong is number of layers of points (layers horizontal and vertical in a way too)
    //points is an output bascially
    //cons (connections) is another output basically
    public static void MakePlanet(float radius, float variance, System.Random r, int numPointsLat, int numPointsLong, out List<Vector3> points, out List<int[]> cons)
    {
        points = new List<Vector3>();
        cons = new List<int[]>();

        float thetaSlerp = 0;
        double rot = 0;
        //making points
        for (int i = 0; i < numPointsLat - 1; i++)
        {            
            for (int i1 = 0; i1 < numPointsLong; i1++)
            {
                float theta = Mathf.Lerp((float)(-Mathf.PI / 2 + Math.PI / (numPointsLat)), (float)(Mathf.PI / 2 - Math.PI / (numPointsLat)), thetaSlerp);
                double rad = radius + Mathf.PerlinNoise(theta, (float)rot) * variance;
                float x = (float)(Math.Cos(rot) * Math.Cos(theta) * (rad));
                float y = (float)(Math.Sin(theta) * (rad));
                float z = (float)(Math.Sin(rot) * Math.Cos(theta) * (rad));
                points.Add(new Vector3(x, y, z));
                rot += 2 * Math.PI / (numPointsLong);
            }
            thetaSlerp += 1.0f / numPointsLat;
        }


        for(int i = 0; i < points.Count - numPointsLong; i++)
        {
            if((i + 1) % numPointsLong == 0)
            {
                cons.Add(new int[] { i, i + 1 - numPointsLong});
                cons.Add(new int[] { i, i + numPointsLong });
                cons.Add(new int[] { i, i + 1 });
            }
            else
            {
                cons.Add(new int[] { i, i + 1 });
                cons.Add(new int[] { i, i + numPointsLong });
                cons.Add(new int[] { i, i + 1 + numPointsLong});
            }
        }
        int count = points.Count;
        points.Add(new Vector3(0, radius, 0));
        points.Add(new Vector3(0, -radius, 0));

        for(int i = 0; i < numPointsLong; i++)
        {
            cons.Add(new int[] { i, points.Count - 1 });
            cons.Add(new int[] { i + count - numPointsLong, points.Count - 2 });
            if(i != numPointsLong - 1)
                cons.Add(new int[] { i + count - numPointsLong, i + count - numPointsLong + 1 });
        }
        cons.Add(new int[] { count - 1, count - numPointsLong });
        
        List<int>[] map = ObjectUpdate.GetMap(cons);

        
        //valley
        int num = r.Next(10, 15);
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, points.Count);
            points[index] = points[index].normalized * radius * 0.75f;
            points = Smooth(index, 5, 0.6f, map, points);
        }        

        //hill
        num = r.Next(5, 8);
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, points.Count);
            points[index] = points[index].normalized * radius * 1.2f;
            points = Smooth(index, 5, 0.65f, map, points);
        }

        //plateau
        num = r.Next(5, 8);
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, points.Count);
            points[index] = points[index].normalized * radius * 1.2f;
            points = Smooth(index, 2, 1f, map, points);
        }

        //flat
        num = r.Next(1, 3);
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, points.Count);
            points[index] = points[index].normalized * radius * 1.1f;
            points = Smooth(index, 5, 0.95f, map, points);
        }

        //rolling hill
        num = r.Next(5, 8);
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, points.Count);
            points[index] = points[index].normalized * radius * 1f;
            points = Smooth(index, 5, 0.3f, map, points);
        }

        for(int i = 0; i < points.Count; i++)
        {
            points[i] += points[i].normalized * variance * (float)r.NextDouble();
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

    float Smooth(float x)
    {
        return x + (x - (x * x * (3.0f - 2.0f * x)));
    }
}

