using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshBuilder : MonoBehaviour {
    public List<Vector3> points;
    public List<TriangleSerialized> triangles;

	// Use this for initialization
	void Start () {
        List<int[]> trianglesList = new List<int[]>();
        for(int i = 0; i < triangles.Count; i++)
        {
            trianglesList.Add(new int[] { triangles[i].p1, triangles[i].p2, triangles[i].p3 });
        }
        Mesh mesh = MeshBuilder3D.GetMeshFrom(points, trianglesList);
        //set the mesh in the meshFilter component so that we can see it.
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [System.Serializable]
    public class TriangleSerialized
    {
        public int p1;
        public int p2;
        public int p3;
    }
}

