using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIslandGenerator
{
    public static void MakeFloatingIsland(float HorRadius, float VertRadius, int numPointsLong, int numPointsLat, float variance, float islandLowerDepth, float distToPlanetCenter, System.Random r, out List<Vector3> points, out List<int[]> cons)
    {
        points = new List<Vector3>();
        cons = new List<int[]>();
        //making points
        points.Add(new Vector3((float)r.NextDouble() * variance, -islandLowerDepth + distToPlanetCenter - (float)r.NextDouble() * variance, (float)r.NextDouble() * variance));

        List<float> minHorRadiusInDirection = new List<float>();
        for (double rot = 0; rot < Math.PI * 2; rot += Math.PI * 2 / numPointsLat)
        {
            minHorRadiusInDirection.Add(float.MaxValue);
        }

        for (double theta = 0; theta < Math.PI / 2; theta += Math.PI / 2 / numPointsLong)
        {
            int index = 0;
            for (double rot = 0; rot < Math.PI * 2; rot += Math.PI * 2 / numPointsLat)
            {
                float radius = VertRadius + (float)r.NextDouble() * variance;
                float horRadius = HorRadius + (float)r.NextDouble() * variance * 5;
                while (horRadius > minHorRadiusInDirection[index])
                {
                    horRadius = HorRadius + (float)r.NextDouble() * variance * 5;
                }
                minHorRadiusInDirection[index] = horRadius;

                float x = (float)(Math.Cos(rot) * horRadius * Math.Cos(theta));
                float y = (float)(Math.Sin(theta) * radius) + distToPlanetCenter;
                float z = (float)(Math.Sin(rot) * horRadius * Math.Cos(theta));
                points.Add(new Vector3(x, y, z));
                index++;
            }

        }
        points.Add(new Vector3(0, VertRadius + distToPlanetCenter + (float)r.NextDouble() * variance, 0));

        //making cons
        //connect center code to bottom edes
        for (int i = 1; i <= numPointsLat; i++)
        {
            cons.Add(new int[] { 0, i });
        }

        //run through all but bottom point and top circle
        for (int i = 1; i < (points.Count - numPointsLat - 1); i++)
        {
            if (i % numPointsLat == 0) //if last in rot
            //connect all but top cricle last in rot      
            {
                cons.Add(new int[] { i, i - numPointsLat + 1 });
                cons.Add(new int[] { i, i + numPointsLat });
                cons.Add(new int[] { i, i + 1 });
            }
            else //if not last in rot
            //connect all but top circle and last in rot 
            {
                cons.Add(new int[] { i, i + 1 });
                cons.Add(new int[] { i, i + numPointsLat });
                cons.Add(new int[] { i, i + numPointsLat + 1 });
            }
        }
        //connect top circle but not last in rot
        for (int i = (points.Count - numPointsLat - 1); i < (points.Count - 1); i++)
        {
            cons.Add(new int[] { i, points.Count - 1 });
            cons.Add(new int[] { i, i + 1 });
        }
        cons.Add(new int[] { points.Count - 2 - numPointsLat + 1, points.Count - 2 });
    }
}
