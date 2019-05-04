using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapintheDark : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    private Animator anim;

    DateTime start;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            start = DateTime.Now;
            anim.SetBool("NAME OF ANIMATION", true); //this tells the animator to play the right animation
            Vector3 newpos = new Vector3(gameObject.transform.position.x + 10, gameObject.transform.position.y, gameObject.transform.position.z);
            transform.position = newpos;
            // the attack will be taken care by the animation??
            cooldown = 240;                          //placeholder time, divide by 60 for cooldown in seconds
        }
    }
}
