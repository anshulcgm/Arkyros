using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour {
    //private Rigidbody rb;
    //private GameObject Player;
    //private GameObject Enemy;
    //public float ArcHeight;
    private Vector3 initialPosition;
    //public float speed;
    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position;
    //    Enemy = GameObject.Find("Golem");
    //    Player = GameObject.Find("TestPlayer");
    //    Debug.Log("Player Location = " + Player.transform.position);
    //    Debug.Log("Enemy Location = " + Enemy.transform.position);
    //    rb = GetComponent<Rigidbody>();
    //    rb.velocity = (Player.transform.position - Enemy.transform.position).normalized * speed;
    //    Debug.Log("Projectile is instantiated at " + transform.position);
    //    Physics.IgnoreCollision(GetComponent<Collider>(), Enemy.GetComponent<Collider>());
    //    Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Planet").GetComponent<Collider>());
    //    //Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Projectile(Clone)").GetComponent<Collider>());
    //    rb.velocity = (Player.transform.position - transform.position).normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
          if(Vector3.Distance(transform.position, initialPosition) >= 100f)
          {
            Destroy(gameObject);
          }
    //    float xint = Enemy.transform.position.x;
    //    float xfinal = Player.transform.position.x;

    //    float xdis = xfinal - xint;
    //    Vector3 nextPos;
    //    if (xdis == 0)
    //    {
    //        float zint = Enemy.transform.position.z;
    //        float zfinal = Player.transform.position.z;
    //        float zdis = zfinal - zint;

    //        float znext = Mathf.MoveTowards(transform.position.z, zfinal, speed * Time.deltaTime * 0.5f);
    //        float ybase = Mathf.Lerp(Enemy.transform.position.y, Player.transform.position.y, (znext - zint) / zdis);
    //        float xbase = Mathf.Lerp(Enemy.transform.position.x, Player.transform.position.x, (znext - zint) / zdis);
    //        float arc = ArcHeight * (znext - zint) * (znext - zfinal) / (-zdis * zdis);
    //        nextPos = new Vector3(xbase, ybase + arc, znext);

    //    }
    //    else
    //    {
    //        float xnext = Mathf.MoveTowards(this.transform.position.x, xfinal, speed * Time.deltaTime * 0.5f);
    //        float ybase = Mathf.Lerp(Enemy.transform.position.y, Player.transform.position.y, (xnext - xint) / xdis);
    //        float zbase = Mathf.Lerp(Enemy.transform.position.z, Player.transform.position.z, (xnext - xint) / xdis);
    //        float arc = ArcHeight * (xnext - xint) * (xnext - xfinal) / (-xdis * xdis);
    //        nextPos = new Vector3(xnext, ybase + arc, zbase);
    //    }
    //    transform.position = nextPos;

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Projectile destroyed because of collision with " +collision.gameObject.name);
        Destroy(gameObject);
    }
}
