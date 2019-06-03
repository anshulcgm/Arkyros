using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimsAndSound : MonoBehaviour
{
    bool playingJumpAnim = false;
    public AnimationController anim;
    SoundManager soundManager;
    public ScytheCollider scythe;
    int attackDelay;
    bool first;
    Stats stats;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
        first = true;
        stats = GetComponent<Stats>();

        stats.health = 150;
        //for all four
        stats.allStats[(int)stat.Defense, (int)statModifier.Base] = 0;
        stats.allStats[(int)stat.Attack, (int)statModifier.Base] = 1;
        stats.allStats[(int)stat.HealthRegen, (int)statModifier.Base] = 1;
        stats.allStats[(int)stat.Speed, (int)statModifier.Base] = 60;


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
            //Debug.Log("Attack was pressed");
            //setAllTriggersFalse();
            if (first)//collides twice
            {
                scythe.setActive(500);
                anim.StartOverlayAnim("Swing_Heavy_1", 0.5f, 0.5f);
                first = false;
            }
            else if (!first)
            {
                scythe.setActive(500);
                anim.StartOverlayAnim("Swing_Heavy_2", 0.5f, 0.3f);
                first = true;
            }
            
            soundManager.playOneShot2("BasicScytheAttack");
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
            //anim.StartOverlayAnim("")
            soundManager.playOneShot2("Jump");
            playingJumpAnim = true;
        }
        if (!GetComponent<Movement>().isAirborne)
        {
            playingJumpAnim = false;
        }

        if (GetComponent<Movement>().isMoving)
        {
            if (GetComponent<Movement>().isAirborne)
            {
                anim.PlayLoopingAnim("Fly_Forward");
            }
            else
            {
                anim.PlayLoopingAnim("Move_Forward");
            }
            soundManager.play2("Walk");
        }
        else if (!GetComponent<Movement>().isMoving)
        {
            anim.PlayLoopingAnim("Idle");
            soundManager.play2("Idle");
        }
    }
}
