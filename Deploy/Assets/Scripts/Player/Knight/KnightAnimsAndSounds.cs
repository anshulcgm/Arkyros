using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimsAndSounds : MonoBehaviour
{
    bool playingJumpAnim = false;
    public AnimationController anim;
    SoundManager soundManager;
    Stats stats;
    public HammerCollider hammer;

    int attackDelay;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        soundManager = GetComponent<SoundManager>();

        stats.health = 200;
        //for all four
        stats.allStats[(int)stat.Defense, (int)statModifier.Base] = 5;
        stats.allStats[(int)stat.Attack, (int)statModifier.Base] = 1;
        stats.allStats[(int)stat.HealthRegen, (int)statModifier.Base] = 2;
        stats.allStats[(int)stat.Speed, (int)statModifier.Base] = 45;

        for (int i = 0; i < 4; i++) //4 is number of stats
        {
            stats.allStats[i, (int)statModifier.Multiplier] = 1; //default mults
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Attack
        if (Input.GetMouseButton(0) && attackDelay == 0)
        {

            anim.StartOverlayAnim("Swing_Heavy", 0.5f, 0.8f);
            soundManager.playOneShot("BasicAttack");
            hammer.setActive(600);
            //attackStart = DateTime.Now;
            attackDelay = 60; //can attack 60 frames later

        }
        if (attackDelay > 0)
        {
            attackDelay--;
        }

        if (GetComponent<Movement>().isAirborne && !playingJumpAnim)
        {
            //play jump anim here
            anim.StartOverlayAnim("Jump", 0.5f, 0.5f);
            //soundManager.playOneShot("Jump");
            playingJumpAnim = true;
        }
        if (!GetComponent<Movement>().isAirborne)
        {
            playingJumpAnim = false;
        }

        if (GetComponent<Movement>().isMoving)
        {
            if (!GetComponent<Movement>().isAirborne)
            {
                anim.PlayLoopingAnim("WalkForward");
                soundManager.play2("Walk");
            }
            else
            {
                anim.PlayLoopingAnim("Flight");
                soundManager.play2("Idle");
            }
            //soundManager.play2("Walk");
        }
        else if (!GetComponent<Movement>().isMoving)//Idle
        {
            anim.PlayLoopingAnim("Standard");
            soundManager.play2("Idle");
        }
    }
}
