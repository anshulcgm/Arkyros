using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetOverrideUp : MonoBehaviour
{
    public GameObject targetLook;
    public GameObject targetFollow;

    public float deadZone;

    public float rotationDamping;

    public float positionDamping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 velocity = Vector3.zero;
    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion lookRot = Quaternion.LookRotation(targetLook.transform.position - transform.position, targetLook.transform.up);

        Vector3 followPoint = targetFollow.transform.position;
        RaycastHit hit;
        if(Physics.Raycast(targetLook.transform.position, targetFollow.transform.position - targetLook.transform.position, out hit)){
            if(Vector3.Distance(hit.point, targetLook.transform.position) < Vector3.Distance(targetFollow.transform.position, targetLook.transform.position)){
                followPoint = hit.point;
            }
        }

        if(Quaternion.Angle(transform.rotation, lookRot) > deadZone)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationDamping * Time.deltaTime);
            
        }
        transform.rotation = lookRot;

        transform.position = followPoint;
        //transform.position = Vector3.Lerp(transform.position, followPoint, positionDamping * Time.deltaTime);
    }
}
