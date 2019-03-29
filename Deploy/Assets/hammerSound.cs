using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammerSound : MonoBehaviour
{
    private AudioSource soundHammer;
    public bool isSwinging;

    public AudioClip swingClip;
    public AudioClip hitClip;

    void Start()
    {
        soundHammer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwinging)
        {
            if (!soundHammer.isPlaying)
            {
                soundHammer.PlayOneShot(swingClip);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        soundHammer.Stop();
        soundHammer.PlayOneShot(hitClip);
    }
}
