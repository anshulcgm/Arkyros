using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FPCPlanet : MonoBehaviour {
    // uses unity's standard mouselook
    public MouseLook mouseLook = new MouseLook();
    public Transform character;
    public Transform characterCamera;

    // the position of the planet - in transform form, because we want it to update if the planet position changes.
    public Transform planet;

    // Use this for initialization
    void Start()
    {
        mouseLook.Init(character, characterCamera);
    }

    // Update is called once per frame
    void Update()
    {
        mouseLook.LookRotation(character, characterCamera);
        // orient the character such that it's head is always pointing away from the center of the planet.
        transform.rotation = Quaternion.LookRotation((transform.position - planet.position).normalized, Vector3.up);

    }
}
