using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimsAndSound : MonoBehaviour
{
    bool playingJumpAnim = false;
    public AnimationController anim;
    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
