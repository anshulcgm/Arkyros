using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDoctorAnimAndSounds : MonoBehaviour
{
    private GameObject camera;
    bool playingJumpAnim = false;
    public AnimationController anim;
    SoundManager soundManager;

    int attackDelay;
    public GameObject fireball;

    Stats stats;
    public GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
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
        if (GetComponent<PlayerScript>().M1Down() && attackDelay == 0)
        {
            model.transform.rotation = camera.transform.rotation;
            GameObject clone = Instantiate(fireball, model.transform.position + model.transform.forward * 5 + transform.up * 6, model.transform.rotation);

            float x = Screen.width / 2f;
            float y = Screen.height / 2f;

            var ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

            clone.GetComponent<Rigidbody>().velocity = ray.direction * 40;


            anim.StartOverlayAnim("BasicAttack", 0.5f, 0.8f);
            soundManager.playOneShot2("FireballCast");
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
            anim.StartOverlayAnim("JumpLaunch", 0.5f, 1f);
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
                anim.PlayLoopingAnim("TrueIdle");
            }
            else
            {
                anim.PlayLoopingAnim("walkForward");
            }
            soundManager.play2("Walk");
        }
        else if (!GetComponent<Movement>().isMoving)
        {
            anim.PlayLoopingAnim("TrueIdle");
            //soundManager.play2("Idle");
        }
    }
}

