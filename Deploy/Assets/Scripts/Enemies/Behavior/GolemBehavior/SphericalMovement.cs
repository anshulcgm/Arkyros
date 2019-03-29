using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SphericalMovement : MonoBehaviour {
    public float speed;
    private GameObject planet;
    private Rigidbody rb;
    private GameObject player;

    public float gravity = -10f;
    //private float planetRadius;
	// Use this for initialization
	void Start () {
        planet = GameObject.FindGameObjectWithTag("planet");
        player = GameObject.FindGameObjectWithTag("Player");
        //planetRadius = planet.transform.localScale.x / 2;
        rb = GetComponent<Rigidbody>();
        //Physics.IgnoreCollision(transform.GetComponent<Collider>(), GameObject.Find("Planet").GetComponent<Collider>());
        //rb.velocity = (player.transform.position - transform.position).normalized;
    }
	
	// Update is called once per frame
	void Update () {
        //Attract();
        //moveOnSphere(player.transform.position);
        Plane2 plane = new Plane2(transform.position.normalized, transform.position);

        Vector2 mappedPoint = plane.GetMappedPoint(player.transform.position) - plane.GetMappedPoint(transform.position);
        if (mappedPoint.magnitude > 1)
            transform.LookAt(mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir + transform.position, transform.position.normalized);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity)){
            Plane2 alignPlane = new Plane2(hit.normal, transform.position);
            //Vector2 mappedPoint2 = alignPlane.GetMappedPoint(player.transform.position) - alignPlane.GetMappedPoint(transform.position);
            //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed);
            if(Vector3.Distance(hit.point, transform.position) >= 1f)
            {
                rb.AddForce(transform.position.normalized * gravity * 2);
                //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed/-2f);
            }
        }
        //adding force towards gravity, adding force towards direction faced
        rb.AddForce(transform.forward * speed);
               
        rb.AddForce(transform.position.normalized * gravity);

    }
    public void moveOnSphere(Vector3 endPoint)
    {
        rb.velocity = (player.transform.position - transform.position).normalized;
        transform.position += rb.velocity * Time.deltaTime;
        //RaycastHit hit;
        //if(Physics.Raycast(transform.position, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity, ~LayerMask.GetMask(new string[] { "ocean" }))){
            //transform.position = hit.point;
        //}
        Vector3 gravity = transform.position - planet.transform.position;
        rb.AddForce(-gravity * 10f);
        //Vector3 constrainedPosition = gravity.normalized * planet.transform.localScale.x / 2;

        //transform.position = planet.transform.position + constrainedPosition;

        Vector3 perpVector = Vector3.Cross(gravity, rb.velocity);
        //Debug.DrawLine(transform.position, perpVector, Color.green);
        //Debug.DrawLine(transform.position, gravity.normalized, Color.red);
        Vector3 tangentVector = Vector3.Cross(perpVector, gravity.normalized);

        rb.AddForce(transform.forward * speed);
        //Debug.DrawLine(transform.position, rb.velocity, Color.blue, Time.deltaTime);
    }
    public void Attract()
    {
        //Debug.Log("In attract function");
        Vector3 gravityUp = (transform.position - planet.transform.position).normalized;
        Vector3 localUp = transform.up;
        //Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, transform.position.normalized);
        //Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, float.PositiveInfinity);
        // Debug.Log("Final rotation is " + finalRotation.eulerAngles);
        //transform.rotation = finalRotation;

        Plane2 plane = new Plane2(transform.position.normalized, transform.position);
        Vector2 mappedPoint = plane.GetMappedPoint(player.transform.position) - plane.GetMappedPoint(transform.position);
        transform.LookAt(mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir + transform.position, transform.position.normalized);
        //float angle = (float)(Math.Atan2(mappedPoint.y, mappedPoint.x) * 180 / Math.PI);
        //Debug.Log("Calculated angle is " + angle + " and Vector3 angle is " + Vector3.Angle(transform.position, player.transform.position));
       // transform.localRotation = Quaternion.Euler(0f, angle, 0f);

        //rb.rotation = Quaternion.FromToRotation(localUp, gravityUp) * rb.rotation;



    }
}
