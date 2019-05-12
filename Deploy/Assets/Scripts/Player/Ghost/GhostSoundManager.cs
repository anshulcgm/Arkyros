using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSoundManager : MonoBehaviour
{
    //shoulda used a dictionary from the start oops
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

    //Tier 1
    void playCleave()
    {
        ghostSound.clip = Cleave;
        ghostSound.PlayOneShot(ghostSound.clip);
    }

    void playCullingStrike()
    {
        ghostSound.clip = EnhancedCullingStrike;
        ghostSound.PlayOneShot(ghostSound.clip);
    }

    void playEnhancedSpiritBlade()
    {
        ghostSound.clip = EnhancedSpiritBlade;
        ghostSound.PlayOneShot(ghostSound.clip);
    }

    void playEnhancedCleave()
    {
        ghostSound.clip = EnhancedCleave;
        ghostSound.PlayOneShot(ghostSound.clip);
    }

    void playEnhancedCullingStrike()
    {
        ghostSound.clip = CullingStrike;
        ghostSound.PlayOneShot(ghostSound.clip);
    }

    void playSpiritBlade()
    {
        ghostSound.clip = SpiritBlade;
        ghostSound.PlayOneShot(ghostSound.clip);
    }



    //Tier 2
    public void playDFCombined()
    {
        //ghostSound.clip = DFCombined;
        ghostSound.PlayOneShot(DFCombined);

    }


    void playLeapInTheDark()
    {
        //ghostSound.clip = LeapInTheDark;
        ghostSound.PlayOneShot(LeapInTheDark);

    }

    void playShadowRush()
    {
        //ghostSound.clip = ShadowRush;
        ghostSound.PlayOneShot(ShadowRush);
    }

    //Tier 3
    void playHSSprout()
    {
        //ghostSound.clip = HSSprout;
        ghostSound.PlayOneShot(HSSprout);
    }
    void playHSHeal()
    {
        //ghostSound.clip = HSHeal;
        ghostSound.PlayOneShot(HSHeal);
    }

    void playEtherealFormDodge()
    {
        //ghostSound.clip = EtherealFormDodge;
        ghostSound.PlayOneShot(EtherealFormDodge);
    }

    //Tier 4
    void playShadowsWing()
    {
        ghostSound.clip = ShadowsWing;
        ghostSound.Play();
    }

    void playBansheesWail()
    {
        //ghostSound.clip = BansheesWail;
        ghostSound.PlayOneShot(BansheesWail);
    }

    void playSOTDDamage()
    {
        //ghostSound.clip = SOTDDamage;
        ghostSound.PlayOneShot(SOTDDamage);
    }
    void playSOTDFloating()
    {
        ghostSound.clip = SOTDFloating;
        ghostSound.Play();
    }

    //Tier 5
    void playMMThrow()
    {
        //ghostSound.clip = MMThrow;
        ghostSound.PlayOneShot(MMThrow);
    }
    void playMMReturn()
    {
        ghostSound.clip = MMReturn;
        ghostSound.Play();
    }

    void playSpiritSummons()
    {
        //ghostSound.clip = SpiritSummons;
        ghostSound.PlayOneShot(SpiritSummons);
    }

    void playNPKTeleport()
    {
        //ghostSound.clip = NPKTeleport;
        ghostSound.PlayOneShot(NPKTeleport);
    }
    void playNPKVoiceLine()
    {
        //ghostSound.clip = NPKVoiceLine;
        ghostSound.PlayOneShot(NPKVoiceLine);
    }

    //Tier 6

    void playSoulReaperResurrect()
    {
        ghostSound.clip = SoulReaperResurrect;
        ghostSound.PlayOneShot(SoulReaperResurrect);
    }

    void playOnslaught()
    {
        ghostSound.clip = Onslaught;
        ghostSound.PlayOneShot(Onslaught);
    }

    void playNowYouSeeMe()
    {
        //ghostSound.clip = NowYouSeeMe;
        ghostSound.PlayOneShot(NowYouSeeMe);
    }

    //Tier 7
    void playHoDRise()
    {
        //ghostSound.clip = HoDRise;
        ghostSound.PlayOneShot(HoDRise);
    }
    void playHoDDive()
    {
        ghostSound.clip = HoDDive;
        ghostSound.Play();
    }
    void playHoDLand()
    {
        //ghostSound.clip = HoDLand;
        ghostSound.PlayOneShot(ghostSound.clip);
    }

    void playCloakSlapCharge()
    {
        ghostSound.clip = CloakSlapCharge;
        ghostSound.Play();
    }
    void playCloakSlapRelease()
    {
        //ghostSound.clip = CloakSlapRelease;
        ghostSound.PlayOneShot(CloakSlapRelease);
    }

    void playP3LSprout()
    {
        //ghostSound.clip = P3LSprout;
        ghostSound.PlayOneShot(P3LSprout);
    }
    void playP3LHeal()
    {
        //ghostSound.clip = P3LHeal;
        ghostSound.PlayOneShot(P3LHeal);
    }

    public void stop()
    {
        ghostSound.Stop();
    }

}
