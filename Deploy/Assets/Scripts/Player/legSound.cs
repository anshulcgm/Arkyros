using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legSound : MonoBehaviour
{
    public AudioSource walk;
    public AudioSource sprint;

    public bool isWalking;
    public bool isSprinting;

    public AudioClip walking;
    public AudioClip sprinting;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            if (!walk.isPlaying)
            {
                sprint.Stop();
                walk.Play();
            }
        }
        else if (isSprinting)
        {
            if (!sprint.isPlaying)
            {
                walk.Stop();
                sprint.Play();
            }
        }
        else if(!isWalking && !isSprinting)
        {
            sprint.Stop();
            walk.Stop();
        }
    }
}
