using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class renderSameSize : MonoBehaviour
{
    public float scale;
    public float baseSpeed;
    public Camera player;
    public AutoMoveAndRotate rotator;


    float startSize;
    // Start is called before the first frame update
    void Start()
    {
        startSize = (player.gameObject.transform.position - transform.position).magnitude;
        baseSpeed = rotator.rotateDegreesPerSecond.value.x;
    }

    // Update is called once per frame
    void Update()
    {
        float size = (player.gameObject.transform.position - transform.position).magnitude;
        gameObject.transform.localScale = new Vector3(size, size, size) * scale;
        rotator.rotateDegreesPerSecond.value.x = baseSpeed * size/startSize;
    }
}
