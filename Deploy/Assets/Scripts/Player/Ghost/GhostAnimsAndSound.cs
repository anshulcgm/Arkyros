﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimsAndSound : MonoBehaviour
{
    bool playingJumpAnim = false;
    public AnimationController anim;
    SoundManager soundManager;
    int attackDelay;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Attack
        if (Input.GetMouseButton(0) && attackDelay == 0)
        {
            //Debug.Log("Attack was pressed");
            //setAllTriggersFalse();
            anim.StartOverlayAnim("Swing_Heavy_1", 0.5f, 0.6f);
            soundManager.playOneShot("BasicScytheAttack");
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
            soundManager.playOneShot("Jump");
            playingJumpAnim = true;
        }
        if (!GetComponent<Movement>().isAirborne)
        {
            playingJumpAnim = false;
        }

        if (GetComponent<Movement>().isMoving)
        {
            anim.PlayLoopingAnim("Move_Forward");
            soundManager.play("Walk");
        }
        else if (!GetComponent<Movement>().isMoving)
        {
            anim.PlayLoopingAnim("Idle");
            soundManager.play("Idle");
        }
    }
}