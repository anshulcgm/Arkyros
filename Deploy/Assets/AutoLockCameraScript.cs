using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLockCameraScript : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position = player.transform.position - direction * 30;
        transform.LookAt(player.transform.position, transform.position.normalized);
    }
}
