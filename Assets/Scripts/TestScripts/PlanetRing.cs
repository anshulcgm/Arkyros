using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRing : MonoBehaviour {
    public int numPts;
    public float outerRad;
    public float innerRad;
    public float width;
    public bool DontClone = false;
	// Use this for initialization
	void Start () {
        
        List<Vector3> outerRing = new List<Vector3>();
        List<Vector3> innerRingA = new List<Vector3>();
        List<Vector3> innerRingB = new List<Vector3>();

        List<int[]> triangles = new List<int[]>();
		for(int i = 0; i < numPts; i++)
        {
            //outer ring

            //i 
            outerRing.Add(new Vector3(Mathf.Cos(Mathf.PI * 2 * i / numPts) * outerRad, Mathf.Sin(Mathf.PI * 2 * i / numPts) * outerRad, 0));
            //i + numPts
            innerRingA.Add(new Vector3(Mathf.Cos(Mathf.PI * 2 * i / numPts) * innerRad, Mathf.Sin(Mathf.PI * 2 * i / numPts) * innerRad, -width / 2));
            //i + 2 * numPts
            innerRingB.Add(new Vector3(Mathf.Cos(Mathf.PI * 2 * i / numPts) * innerRad, Mathf.Sin(Mathf.PI * 2 * i / numPts) * innerRad, width / 2));

            //connects the top part of the ring via triangle mesh
            triangles.Add(new int[] {i, GetIndex(i + 1, 1), i + numPts});
            triangles.Add(new int[] {i, i + numPts, GetIndex(i + numPts - 1, 2)});

            //connects the bottom part of the ring via triangle mesh
            triangles.Add(new int[] {i, GetIndex(i + 1, 1), i + 2 * numPts});
            triangles.Add(new int[] {i, i + 2 * numPts, GetIndex(i + 2 * numPts - 1, 3)});
            //connects the inside face of the ring via triangle mesh.
            triangles.Add(new int[] {i + numPts, GetIndex(i + numPts + 1, 2), i + 2 * numPts });
            triangles.Add(new int[] {i + 2 * numPts, GetIndex(i + numPts + 1, 2), GetIndex(i + 2 * numPts + 1, 3) });

        }
        outerRing.AddRange(innerRingA);
        outerRing.AddRange(innerRingB);
        Mesh mesh = MeshBuilder3D.GetMeshFrom(outerRing, triangles);
        //set the mesh in the meshFilter component so that we can see it.
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        GetComponent<MeshCollider>().sharedMesh = mesh;
        transform.rotation = Random.rotation;
	}

    //correctly wraps index given unwrapped index and layer.
    public int GetIndex(int unWrappedIndex, int layer)
    {
        int maxVal = layer * numPts - 1;
        int minVal = (layer - 1) * numPts;
        if(unWrappedIndex > maxVal)
        {
            return unWrappedIndex - numPts;
        }
        if(unWrappedIndex < minVal)
        {
            return unWrappedIndex + numPts;
        }
        return unWrappedIndex;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
