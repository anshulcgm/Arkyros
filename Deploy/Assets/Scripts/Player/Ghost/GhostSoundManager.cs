using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSoundManager : MonoBehaviour
{

    public AudioSource ghostSound;

    //Tier 1
    public AudioClip Cleave;

    public AudioClip CullingStrike;

    public AudioClip SpiritBlade;


    //Tier 2

    public AudioClip DFCombined;

    public AudioClip LeapInTheDark;

    public AudioClip ShadowRush;





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void playDFCombined()
    {
        //ghostSound.clip = DFCombined;
        ghostSound.PlayOneShot(DFCombined);
        
    }


    void playLeapInTheDark()
    {
        ghostSound.clip = LeapInTheDark;
        ghostSound.PlayOneShot(LeapInTheDark);
        
    }

    void playShadowRush()
    {

    }

    public void stop()
    {
        ghostSound.Stop();
    }

}
