using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrabBehavior : MonoBehaviour
{
    public float shurikenRange;
    //public List<Shrab> shrabsInRange;
    public float pincerMovementRange;

    public GameObject pincer_R_Top;

    private GameObject player;
    private GameObject planet; 

    public GameObject projectile; 

    private float shrabMovementSpeed = 0; 

    private GameObject leftPlayerLeg;
    private GameObject rightPlayerLeg;

    public float distanceAbovePlanetSurface; 
    private Animator anim;

    public float timer; //How often should the shrab shoot shurikens
    private float variableTimer;

    private float gravity = 10f; 

    private Rigidbody rb;

    public float idleRadius;
    public float attackRadius;

    public float idleTimer;
    private float oIdleTimer;

    private bool searchForNewIdlePoint = true;

    public float maxIdleDistance;

    private Vector3 targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("planet");
        player = GameObject.FindGameObjectWithTag("Player");
        leftPlayerLeg = GameObject.FindGameObjectWithTag("LeftLeg");
        rightPlayerLeg = GameObject.FindGameObjectWithTag("RightLeg");
        anim = GetComponent<Animator>(); 
        this.shurikenRange = 15;
        //this.shrabsInRange = new List<Shrab>();
        this.pincerMovementRange = 10;

        variableTimer = timer;

        rb = GetComponent<Rigidbody>();

        targetPoint = randomPointInRadius();

        oIdleTimer = idleTimer;
    }

    // Update is called once per frame
    void Update()
    {
        shrabMovementSpeed = GetComponent<StatManager>().shrabMovementSpeed;
        float raycastDistancePlayer = Vector3.Distance(transform.position, findRaycastPointOnSphere(player.transform.position));
        //transform.LookAt(player.transform);
        if(raycastDistancePlayer <= attackRadius) { 
            pincerMovement();
        }
        else if(raycastDistancePlayer <= idleRadius && raycastDistancePlayer > attackRadius)
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer <= 0)
            {
                targetPoint = randomPointInRadius();
                searchForNewIdlePoint = false;
                idleTimer = oIdleTimer;
            }
            if (!searchForNewIdlePoint)
            {
                shrabSphereMovement(targetPoint);
            }
            if(Vector3.Distance(transform.position, targetPoint) <= 0.5f)
            {
                rb.velocity = Vector3.zero;
                searchForNewIdlePoint = true;
            }
        }
        RaycastHit hit; 
        if (Physics.Raycast(transform.position + transform.position.normalized * 10.0f, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
        {
            transform.position = hit.point;
        }
       /* if(Vector3.Distance(transform.position, player.transform.position) > 3.0f)
        {
            waterShurikenAttack();
        }
        /*
        foreach(Shrab s in Shrab.shrabList)
        {
            if(Vector3.Distance(s.referenceObject.transform.position, transform.position) < pincerMovementRange)
            {
                shrabsInRange.Add(s);
            }
        }
        int numShrabs = 0;
        foreach(Shrab s in shrabsInRange)
        {
            numShrabs += s.getNumShrabs();
        }
        if(numShrabs >= 12)
        {
            pincerMovement();
        }
        */
    }
    public Vector3 randomPointInRadius()
    {
        //Code to find random point on planet within radius
        Vector3 initialPoint = Random.insideUnitSphere * maxIdleDistance + transform.position + new Vector3(0f, 30f, 0f);
        Vector3 targetPoint = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(initialPoint, -initialPoint.normalized, out hit, Mathf.Infinity))
        {
            targetPoint = hit.point;
        }
        return targetPoint;
    }
    public void shrabSphereMovement(Vector3 target)
    {
        /*
        //This code moves the golem towards the player using plane-logic
        //For shrab, instead of transform.position.normalized, raycast to the center of the planet and use hit.normal 
        Vector3 targetPosition = findRaycastPointOnSphere(target);
        Vector3 planeInstantiationPoint = Vector3.zero;
        RaycastHit planeHit;
        if (Physics.Raycast(transform.position.normalized * 5f + transform.position, (-transform.position).normalized, out planeHit, Mathf.Infinity))
        {
            planeInstantiationPoint = planeHit.normal;
            //if(Vector3.Distance(transform.position, planeHit.point) >= 2.0f)
            //{
            transform.position = planeHit.point + transform.position.normalized * distanceAbovePlanetSurface;
            //}
        }

        Debug.Log("Plane instantiation point is " + planeInstantiationPoint);
        Plane2 plane = new Plane2(planeInstantiationPoint, transform.position);

        Vector2 mappedPoint = plane.GetMappedPoint(target) - plane.GetMappedPoint(transform.position);
        if (mappedPoint.magnitude > 1)
            transform.LookAt(mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir + transform.position, transform.position.normalized);
        /*
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
        {
            Plane2 alignPlane = new Plane2(hit.normal, transform.position);
            //Vector2 mappedPoint2 = alignPlane.GetMappedPoint(player.transform.position) - alignPlane.GetMappedPoint(transform.position);
            //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed);
            if (Vector3.Distance(hit.point, transform.position) >= 1f)
            {
                rb.AddForce(transform.position.normalized * gravity * 2);
                //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed/-2f);
            }
        }
        
        //adding force towards gravity, adding force towards direction faced
        //float step = Time.deltaTime * speed;
        //rb.AddForce((mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir).normalized * shrabMovementSpeed);
        rb.velocity = ((mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir) - transform.position).normalized * shrabMovementSpeed;
        Debug.Log("Velocity is " + rb.velocity + " and magnitude is " + rb.velocity.magnitude);
        //transform.position = Vector3.MoveTowards(transform.position, (mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir), step);
        //rb.velocity = (mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir) * speed;
        //rb.AddForce((mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir) * speed);

        //rb.AddForce(transform.position.normalized * gravity * rb.mass);

        Debug.DrawLine(transform.position, transform.position + mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir, Color.red);
        */
        Plane2 plane = new Plane2(transform.position.normalized, transform.position);

        Vector2 mappedPoint = plane.GetMappedPoint(target) - plane.GetMappedPoint(transform.position);
        Vector3 mappedPoint3D = mappedPoint.x * plane.xDir + mappedPoint.y * plane.yDir;
        if (mappedPoint.magnitude > 1)
            transform.LookAt(mappedPoint3D + transform.position, transform.position.normalized);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.position.normalized * 10.0f, (planet.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
        {
            Plane2 alignPlane = new Plane2(hit.normal, transform.position);
            transform.position = hit.point;
            //Vector2 mappedPoint2 = alignPlane.GetMappedPoint(player.transform.position) - alignPlane.GetMappedPoint(transform.position);
            //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed);
            //if (Vector3.Distance(hit.point, transform.position) >= 1f)
            //{
              //  rb.AddForce(transform.position.normalized * gravity);
                //rb.AddForce((mappedPoint2.x * alignPlane.xDir + mappedPoint2.y * alignPlane.yDir).normalized * speed/-2f);
            //}
        }
        //adding force towards gravity, adding force towards direction faced
        rb.velocity = transform.forward * shrabMovementSpeed;
    }


    public Vector3 findRaycastPointOnSphere(Vector3 point)
    {
        RaycastHit hit;
        Vector3 pointToReturn = Vector3.zero;
        if (Physics.Raycast(point + point.normalized * 5.0f, (-point).normalized, out hit, Mathf.Infinity))
        {
            pointToReturn = hit.point;
        }
        return pointToReturn;
    }

    public void pincerMovement()
    {
        Vector3 leftLegRaycast = findRaycastPointOnSphere(leftPlayerLeg.transform.position);
        Vector3 rightLegRaycast = findRaycastPointOnSphere(rightPlayerLeg.transform.position);
        if (Vector3.Distance(transform.position, leftLegRaycast) < Vector3.Distance(transform.position, rightLegRaycast))
        {
            Debug.Log("going for left leg");
            Debug.Log("Distance between enemy and left leg is " + Vector3.Distance(transform.position, leftLegRaycast));
            if (Vector3.Distance(transform.position, leftLegRaycast) > 4.5f)
            {
                Debug.Log("Moving towards left leg");
                anim.SetBool("Moving", true);
                anim.SetBool("Attack", false);
                shrabSphereMovement(leftLegRaycast);
            }
            else
            {
                Debug.Log("Trying to stop at left leg");
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                anim.SetBool("Moving", false);
                anim.SetBool("Attack", true);
                player.GetComponent<Stats>().takeDamage(1f);
            }
        }
        else
        {
            Debug.Log("going for right leg");
            Debug.Log("Distance between enemy and right leg is " + Vector3.Distance(transform.position, rightLegRaycast));
            if (Vector3.Distance(transform.position, rightLegRaycast) > 4.5f)
            {
                Debug.Log("Moving towards right leg");
                anim.SetBool("Moving", true);
                anim.SetBool("Attack", false);
                shrabSphereMovement(rightLegRaycast);
            }
            else
            {
                Debug.Log("Trying to stop at right leg");
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                anim.SetBool("Moving", false);
                anim.SetBool("Attack", true);
                //Adjust player health here
            }
        }
    }

    public void waterShurikenAttack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject laser = (GameObject)Instantiate(projectile, pincer_R_Top.transform.position, Quaternion.identity);
            timer = variableTimer;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Center")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
}
