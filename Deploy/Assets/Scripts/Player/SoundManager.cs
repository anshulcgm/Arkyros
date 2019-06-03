using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource Sound;
    public AudioSource Sound2;
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

    public void playOneShot2(string soundname)
    {
        Sound2.PlayOneShot(SoundsDictionary[soundname]);
    }
    public void playOneShot(string soundname, float volume)
    {
        Sound.PlayOneShot(SoundsDictionary[soundname], volume);
    }

    public void play(string soundname)
    {
        if (Sound.clip != SoundsDictionary[soundname])
        {
            Sound.volume = 1.0f;
            Sound.clip = SoundsDictionary[soundname];
            Sound.Play();
        }
        
        
    }
    public void play2(string soundname)
    {
        if (Sound2.clip != SoundsDictionary[soundname])
        {
            Sound2.volume = 1.0f;
            Sound2.clip = SoundsDictionary[soundname];
            Sound2.Play();
        }


    }
    //overloaded
    public void play(string soundname, float volume)
    {
        if (Sound.clip != SoundsDictionary[soundname])
        {
            Sound.volume = 0.5f;
            Sound.clip = SoundsDictionary[soundname];
            Sound.Play();
        }
    }

    public void stop()
    {
        Sound.Stop();
        /*
        Sound.clip = SoundsDictionary["Idle"];
        Sound.Play();
        */
    }
    public void stop2()
    {
        Sound2.Stop();
        /*
        Sound.clip = SoundsDictionary["Idle"];
        Sound.Play();
        */
    }
}
