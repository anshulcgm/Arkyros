using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Atmosphere : MonoBehaviour {
    public Transform planet;
    public float planetRad = 200;
    float altAtmBlack;
    Camera cam;
    Vector4 backCol;
    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        backCol = cam.backgroundColor;
        altAtmBlack = planetRad * 1.1f;
    }

    
	// Update is called once per frame
	void Update () {
        float alt = Vector3.Distance(planet.position, transform.position);
        float interpolationFractionAlt = ((alt - planetRad) * (alt - planetRad)) / ((altAtmBlack) * (altAtmBlack));
        float interpolationFractionY = 1 - ((transform.position.y + planetRad) / (planetRad * 1.5f));
        float interpolationFraction = interpolationFractionAlt + interpolationFractionY;

        if (interpolationFraction <= 1)
        {
            Vector4 atmCol = Vector4.Lerp(backCol, new Vector4(0, 0, 0, 0), interpolationFraction);
            cam.backgroundColor = atmCol;
        }
	}
}
