using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


public class Atmosphere : MonoBehaviour {
    public Transform planet;
    public Transform sun;
    public float planetRad = 200;
    public float duskEffectProportion = 0.05f;
    public float maxBloomIntensity;
    public float minBloomIntensity;
    public Color underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
    public float underwaterFogDensity = 0.03f;


    float altAtmBlack;
    Camera cam;
    Vector4 backCol;
    Bloom bloomEffect;
    
    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        backCol = cam.backgroundColor;
        altAtmBlack = planetRad * 1.1f;
        bloomEffect = GetComponent<Bloom>();        
    }


    // Update is called once per frame
    void Update() {
        float alt = Vector3.Distance(planet.position, transform.position);
        float interpolationFractionAlt = ((alt - planetRad) * (alt - planetRad)) / ((altAtmBlack) * (altAtmBlack));

        Vector3 planetToPlayer = transform.position - planet.position;
        Vector3 planetToSun = sun.position - planet.position;
        float angle = Vector3.Angle(planetToPlayer, planetToSun);

        float interpolationFractionSun = 1 - ((180 - angle) / 180);
        float interpolationFraction = Mathf.Min(interpolationFractionAlt + interpolationFractionSun, 1);

        //sun bloom, increase blur intensity when looking towards the sun
        Vector3 camLook = (cam.transform.rotation * Vector3.forward).normalized;
        Debug.DrawLine(cam.transform.position, cam.transform.position + camLook, Color.cyan);
        Vector3 camToSun = (sun.position - cam.transform.position).normalized;
        Debug.DrawLine(cam.transform.position, cam.transform.position + camToSun, Color.red);
        float angleCam = Vector3.Angle(camLook, camToSun);
        float interpolationFractionCam = ((180 - angleCam) / 180);
        bloomEffect.bloomIntensity = (maxBloomIntensity - minBloomIntensity) * interpolationFractionCam + minBloomIntensity;

        RaycastHit hit;
        //handling underwater shading
        if (!Physics.Raycast(transform.position, -planetToPlayer, out hit, Mathf.Infinity, LayerMask.GetMask(new string[] { "ocean" })))
        {
            RenderSettings.fogColor = underwaterColor;
            RenderSettings.fogDensity = underwaterFogDensity;
            RenderSettings.fog = true;
        }
        else
        {
            RenderSettings.fogColor = new Color(0, 0, 0, 0);
            RenderSettings.fogDensity = 0.0f;
            RenderSettings.fog = false;
        }


        //get 
        Vector4 atmCol = Vector4.Lerp(backCol, new Vector4(0, 0, 0, 0), interpolationFraction);
        cam.backgroundColor = atmCol;        
	}

    float GetDirectedAngleRadians(Vector2 vector1, Vector2 vector2)
    {
        float angle = Mathf.Atan2(vector2.y, vector2.x) - Mathf.Atan2(vector1.y, vector1.x);
        if (angle > Mathf.PI) { angle -= 2 * Mathf.PI; }
        else if (angle <= -Mathf.PI) { angle += 2 * Mathf.PI; }
        return angle;
    }
}
