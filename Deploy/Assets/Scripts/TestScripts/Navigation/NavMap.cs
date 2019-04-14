using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class NavMap
{
    List<int>[,,] triChunks;

    public NavMap(List<int[]> triangles, List<Vector3> points, float spaceBetweenChunks, float spaceBetweenNodes, Vector3 start, Vector3 bounds)
    {
        triChunks = new List<int>[(int)(bounds.x / spaceBetweenChunks), (int)(bounds.y / spaceBetweenChunks), (int)(bounds.z / spaceBetweenChunks)];
        for(int i = 0; i < triangles.Count; i++)
        {
            for (int i1 = 0; i1 < triangles[i].Length; i1++)
            {
                Vector3 mappedPt = (points[triangles[i][i1]] - start) / spaceBetweenChunks;
                if (triChunks[(int)mappedPt.x, (int)mappedPt.y, (int)mappedPt.z] == null)
                {
                    triChunks[(int)mappedPt.x, (int)mappedPt.y, (int)mappedPt.z] = new List<int>();
                }
                if(!triChunks[(int)mappedPt.x, (int)mappedPt.y, (int)mappedPt.z].Contains(i))
                {
                    triChunks[(int)mappedPt.x, (int)mappedPt.y, (int)mappedPt.z].Add(i);
                }                
            }
        }

        for(int x = 0; x < triChunks.Length; x++)
        {
            for (int y = 0; y < triChunks.Length; y++)
            {
                for (int z = 0; z < triChunks.Length; z++)
                {
                    
                }
            }
        }


    }


}

