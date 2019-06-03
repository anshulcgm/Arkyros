using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPosition : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform parentStarship;
    private LineRenderer lr;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        parentStarship = transform.parent;
        lr.SetPosition(0, parentStarship.position);
        lr.SetPosition(1, parentStarship.position);
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, parentStarship.transform.position);
    }
}
