using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFractals : MonoBehaviour {

	// Use this for initialization
	void Start () {
        List<Vector3> positions = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1),
                                                        new Vector3(0.15f, 3, 0.15f), new Vector3(0.85f, 2, 0), new Vector3(0, 2, 0.85f),
                                                        new Vector3(0.25f, 5, 0.25f) };

        List<int[]> edges = new List<int[]>() { new int[] { 0, 1 }, new int[] { 1, 2 }, new int[] { 2, 0 },
                                                    new int[] { 3, 4 }, new int[] { 4, 5 }, new int[] { 5, 3 },
                                                    new int[] { 0, 3 }, new int[] { 1, 4 }, new int[] { 2, 5 },
                                                    new int[] { 0, 4 }, new int[] { 1, 5 }, new int[] { 2, 3 },
                                                    new int[] { 3, 6 }, new int[] { 4, 6 }, new int[] { 5, 6 }};


                                                    //bottom
        List<int[]> triangles = new List<int[]>() { new int[] { 0, 1, 2 },
                                                    //middle
                                                    new int[] { 0, 4, 1 }, new int[] { 1, 5, 2 }, new int[] { 2, 3, 0 },
                                                    new int[] { 0, 3, 4 }, new int[] { 1, 4, 5 }, new int[] { 2, 5, 3 },
                                                    //top
                                                    new int[] { 3, 4, 6 }, new int[] { 4, 5, 6 }, new int[] { 5, 3, 6 }};

       

        int[] baseFace = new int[] { 0, 1, 2 };

        List<int[]> generationFaces = new List<int[]>() { new int[] { 3, 4, 6 }, new int[] { 4, 5, 6 }, new int[] { 5, 3, 6 } };

        Fractal f = new Fractal(new FractalShape(positions, triangles, baseFace, generationFaces), 0.5f, 0.75f, 1);
        f.Generate();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
