using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipEndgame : MonoBehaviour
{
    private Renderer rend;
    private bool endgameCheck;
    private GameObject endgamePrefab;

    public float timer;
    private float oTimer;

    public GameObject nuke1;
    public GameObject nuke2;

    private Renderer nuke1Render;
    private Renderer nuke2Render;

    private Color initialColor;

    // Start is called before the first frame update
    void Start()
    {
        endgameCheck = true;
        endgamePrefab = transform.GetChild(4).gameObject;

        nuke1Render = nuke1.GetComponent<Renderer>();
        nuke2Render = nuke2.GetComponent<Renderer>();

        oTimer = timer;

        initialColor = nuke1Render.material.color;
        Debug.Log("Material being accessed is " + nuke1Render.material);
        Debug.Log("Initial color is " + initialColor);
    }

    // Update is called once per frame
    void Update()
    {
        //Color of ship = new color(this.getcolor.red + 1, this.getcolor blue, fdbhjlfdsafjhl)
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            //nuke1Render.materials[0].color = Color.Lerp(initialColor, Color.red, Time.time / timer);
            Color toSet = new Color(1 / oTimer * Time.time, 0, 0);
            nuke1Render.material.SetColor("_Emission", toSet);
            nuke2Render.material.SetColor("_Emission", toSet);
            //Debug.Log("Setting emission color as " + toSet);
        }
        else
        {
            if (endgameCheck)
            {
                endgamePrefab.GetComponent<Rigidbody>().useGravity = true;
                endgameCheck = false;
            }
        }
    }
}
