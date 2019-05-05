using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StriaghtShooter : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject Player;
    private Vector3 current;

    public float speed;
    public float count;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        current = Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = (current).normalized * speed;
        if (count == 0)
        {
            Destroy(this.gameObject);
        }
        count--;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
