using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{

    GravityAttractor planet;
    Rigidbody rigidbody;

    void Awake()
    {
        planet = GameObject.FindGameObjectWithTag("planet").GetComponent<GravityAttractor>();
        rigidbody = GetComponent<Rigidbody>();

        // Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Allow this body to be influenced by planet's gravity
        if(rigidbody.gameObject.tag == "Player")
        {
            if (rigidbody.gameObject.GetComponent<Stats>().buffs[(int)buff.Gravityless] == 0) //if you don't have the gravityless buff, gravity applies
            {
                planet.Attract(rigidbody);
            }
        }

        else
        {
            planet.Attract(rigidbody); //all non players will face gravity
        }
        
    }
}