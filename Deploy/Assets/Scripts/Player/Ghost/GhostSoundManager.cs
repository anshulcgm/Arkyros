using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSoundManager : MonoBehaviour
{

    public AudioSource ghostSound;

    //Tier 1
    public AudioClip Cleave;
    public AudioClip EnhancedCleave;

    public AudioClip CullingStrike;
    public AudioClip EnhancedCullingStrike;

    public AudioClip SpiritBlade;
    public AudioClip EnhancedSpiritBlade;


    //Tier 2

    public AudioClip DFCombined;

    public AudioClip LeapInTheDark;

    public AudioClip ShadowRush;

    //Tier 3

    public AudioClip HSSprout;
    public AudioClip HSHeal;

    public AudioClip EtherealFormDodge;

    //Tier 4

    public AudioClip BansheesWail;

    public AudioClip ShadowsWing;

    public AudioClip SOTDDamage;
    public AudioClip SOTDFloating;

    //Tier 5

    public AudioClip MMThrow;
    public AudioClip MMReturn;

    public AudioClip NPKTeleport;
    public AudioClip NPKVoiceLine;

    public AudioClip SpiritSummons;

    //Tier 6

    public AudioClip NowYouSeeMe;

    public AudioClip Onslaught;

    public AudioClip SoulReaperResurrect;

    //Tier 7

    public AudioClip CloakSlapCharge;
    public AudioClip CloakSlapRelease;

    public AudioClip HoDRise;
    public AudioClip HoDDive;
    //public AudioClip HoDLand;

    public AudioClip P3LSprout;
    public AudioClip P3LHeal;






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
        ghostSound.clip = DFCombined;
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
