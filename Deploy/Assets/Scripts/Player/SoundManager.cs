using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource Sound;
    public AudioClip[] Clips;
    public string[] ClipNames = new string[] { };
    Dictionary<string, AudioClip> SoundsDictionary = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        

        for(int i = 0; i < ClipNames.Length; i++)
        {
            SoundsDictionary.Add(ClipNames[i], Clips[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playOneShot(string soundname)
    {
        Sound.PlayOneShot(SoundsDictionary[soundname]);
    }

    public void play(string soundname)
    {
        Sound.clip = SoundsDictionary[soundname];
        Sound.Play();
    }

    public void stop()
    {
        Sound.Stop();
    }
}
