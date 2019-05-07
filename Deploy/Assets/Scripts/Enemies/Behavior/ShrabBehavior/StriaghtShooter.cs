using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StriaghtShooter : MonoBehaviour
{
<<<<<<< HEAD
	private Rigidbody rb;
	private GameObject Player;
	private Vector3 current;
	private float oTime;

	public float speed;
	public float timer;
	
=======
    private Rigidbody rb;
    private GameObject Player;
    private Vector3 current;

    public float speed;
    public float count;
>>>>>>> ded1e62142f722801a456760047918a4f153c236

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
<<<<<<< HEAD
		rb = GetComponent<Rigidbody>();
		current = Player.transform.position - transform.position;
		oTime = timer;
=======
        rb = GetComponent<Rigidbody>();
        current = Player.transform.position - transform.position;
>>>>>>> ded1e62142f722801a456760047918a4f153c236
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
		rb.velocity = (current).normalized * speed;

		timer -= Time.deltaTime;

		if (timer < 0) {
			Destroy(this.gameObject);
		}
=======
        rb.velocity = (current).normalized * speed;
        if (count == 0)
        {
            Destroy(this.gameObject);
        }
        count--;
>>>>>>> ded1e62142f722801a456760047918a4f153c236
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
