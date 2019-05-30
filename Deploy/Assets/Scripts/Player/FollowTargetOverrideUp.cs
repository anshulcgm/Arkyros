using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetOverrideUp : MonoBehaviour
{
    public GameObject targetLook;
    public GameObject targetFollow;

    public float rotationDamping;

    public float positionDamping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 velocity = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        Quaternion lookRot = Quaternion.LookRotation(targetLook.transform.position - transform.position, targetLook.transform.up);
        Vector3 followPoint = targetFollow.transform.position;
        RaycastHit hit;
        if(Physics.Raycast(targetLook.transform.position, targetFollow.transform.position - targetLook.transform.position, out hit)){
            if(Vector3.Distance(hit.point, targetLook.transform.position) < Vector3.Distance(targetFollow.transform.position, targetLook.transform.position)){
                followPoint = hit.point;
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, rotationDamping * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, followPoint, positionDamping * Time.deltaTime);
    }
}
