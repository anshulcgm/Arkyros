using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateWhenFarFromPlayer : MonoBehaviour
{
    public float distanceToDeactivation;
    GameObject player;
    Vector3 avg = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        r = GetComponent<MeshRenderer>();
        c = GetComponent<MeshCollider>();
        List<Vector3> verts = new List<Vector3>();
        c.sharedMesh.GetVertices(verts);
        foreach(Vector3 vert in verts)
        {
            avg += vert;
        }
        avg /= verts.Count;
    }
    MeshRenderer r;
    MeshCollider c;
    // Update is called once per frame
    void Update()
    {
        if(Vector3.SqrMagnitude(avg - player.transform.position) > distanceToDeactivation * distanceToDeactivation)
        {
            if (r.enabled)
            {
                r.enabled = false;
                c.enabled = false;
            }

        }
        else
        {
            if (!r.enabled)
            {
                r.enabled = true;
                c.enabled = true;
            }
        }
    }
}
