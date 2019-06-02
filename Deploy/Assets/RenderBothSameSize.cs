using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class RenderBothSameSize : MonoBehaviour
{    
    public float scaleSun;
    public float scaleMoon;
    public float baseSpeed;
    public Camera player;
    public AutoMoveAndRotate rotator;
    public GameObject sun;
    public GameObject moon;

    float startSize;
    // Start is called before the first frame update
    void Start()
    {
        startSize = (GetClosest().transform.position - player.transform.position).magnitude;
        baseSpeed = rotator.rotateDegreesPerSecond.value.x;
    }

    // Update is called once per frame
    void Update()
    {
        float size = (GetClosest().transform.position - player.transform.position).magnitude;
    
        sun.transform.localScale = new Vector3(size, size, size) * (GetClosest().Equals(sun) ? scaleSun : scaleMoon);
        moon.transform.localScale = new Vector3(size, size, size) * (GetClosest().Equals(sun) ? scaleSun : scaleMoon);

        rotator.rotateDegreesPerSecond.value.x = baseSpeed * size/startSize;
    }

    private GameObject GetClosest(){
        GameObject closest =  (Vector3.SqrMagnitude(sun.transform.position - player.transform.position) < Vector3.SqrMagnitude(moon.transform.position - player.transform.position)) ? sun : moon;
        return closest;
    }
}
