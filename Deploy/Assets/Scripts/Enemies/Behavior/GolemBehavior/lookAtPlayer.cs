using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class lookAtPlayer : MonoBehaviour {

    public GameObject player;
    public float fieldOfView;
    private GameObject planet;
	// Use this for initialization
	void Start () {
        planet = GameObject.Find("Planet");	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Angle between planet and player is " + Vector3.Angle(planet.transform.position, player.transform.position));
        Debug.Log("Angle between planet and enemy is " + Vector3.Angle(planet.transform.position, transform.position));
        float sphereAngle = angleBetweenTwoPointsOnSphere(transform.position, player.transform.position, planet.transform.position);
        Debug.Log("Sphere angle is " + sphereAngle);
        if (sphereAngle <= 20.0f)
        {
            
            Plane2 plane = new Plane2(transform.parent.up, transform.position);
            Vector2 mappedPoint = plane.GetMappedPoint(player.transform.position) - plane.GetMappedPoint(transform.position);
            //Debug.Log("mappedPoint x is " + mappedPoint.x + " and mappedPoint y is " + mappedPoint.y);
            //Debug.Log("Unmapped point is " + (mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir));
            Debug.DrawLine(transform.position, transform.position + mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir, Color.red);
            if (mappedPoint.magnitude < 10f)
            {
                Debug.Log("Magnitiude tiny");
                return;
            }
            float angle = (float)(Math.Atan2((transform.position + mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir).z - transform.position
                .z, (transform.position + mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir).x - transform.position.x) * 180 / Math.PI);
            //Debug.Log("Calculated angle is " + angle + " and Vector3 angle is " + Vector3.Angle(transform.position, player.transform.position));
            //transform.rotation = Quaternion.Euler(0f, 270 - angle, 0f);
            //transform.localRotation = Quaternion.AngleAxis(angle, player.transform.position);
        }
    }
    public bool detectLineOfSight(GameObject target)
    {
        RaycastHit hit;
        Debug.DrawLine(transform.position, target.transform.position);
        if (Vector3.Angle(target.transform.position - transform.position, transform.forward) <= fieldOfView && Physics.Linecast(transform.position,
            target.transform.position, out hit) && hit.collider.gameObject == target)
        {
            
            Debug.Log("Object is in line of sight");
            return true;
        }
       Debug.Log("Object isn't in line of sight");
       return false;
    }
    public float angleBetweenTwoPointsOnSphere(Vector3 pointA, Vector3 pointB, Vector3 sphereCenter)
    {
        return (Vector3.Angle(pointA - sphereCenter, pointB - sphereCenter));
    }
}
