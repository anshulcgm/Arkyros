using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarChant : MonoBehaviour
{
    public float cooldown;

    private GameObject camera;

    public AnimationController anim;
    public GameObject model;

    DateTime start;


    Rigidbody rigidbody;
    Stats stats;
    TargetCenterScreen tcs;

    private bool buffActive;
    private bool cast;

    SoundManager soundManager;

    public GameObject[] array;
    public List<GameObject> affectedPlayers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<AnimationController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();
        tcs = GetComponent<TargetCenterScreen>();
        soundManager = GetComponent<SoundManager>();

        array = GameObject.FindGameObjectsWithTag("Player");

        cooldown = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<PlayerScript>().IsPressed("e") && cooldown == 0)      //place key, any key can be pressed.
        {
            cast = false; //ability not yet cast
            start = DateTime.Now;
            anim.StartOverlayAnim("AnimationName", 0.5f, 1f); //this tells the animator to play the right animation, what strength, what duration

            //or

            anim.PlayLoopingAnim("AnimationName"); // mostly only for movement, probably not used in an ability


            //put any setup code here, before the ability is actually cast



        }

        if ((DateTime.Now - start).TotalSeconds < 1 && !cast)
        {
            soundManager.playOneShot("5WarChantLoopable");
            anim.StartOverlayAnim("WarchantBeginning", 0.5f, 1.0f);

            buffActive = true;
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] /= 2f;
            /*
             * All the code for the ability that you want to write
             * transform.forward for the direction the player is 
             * maybe setting colliders
             * instantiating new objects
             * to damage enemy, EnemyGameObject.GetComponent<StatManager>().changeHealth(amount), amount can be positive or negative
             */


            cooldown = 1200;                          //placeholder time, divide by 60 for cooldown in seconds
            cast = true;

        }

        if (buffActive) //keeps track of who is in range
        {
            foreach (GameObject player in array)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= 20) //if the player is within 20 units of the player
                {
                    if (!affectedPlayers.Contains(player))//if its not already in the List, add it in and give it the buff
                    {
                        //this ensures that everything is only buffed once
                        player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 1.25f;
                        affectedPlayers.Add(player);
                    }
                }
            }
            foreach (GameObject player in affectedPlayers)
            {
                if (Vector3.Distance(player.transform.position, transform.position) > 20)//remove if too far away and debuff
                {
                    player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 0.8f;
                    affectedPlayers.Remove(player);
                }
            }
        }

        if((DateTime.Now - start).TotalSeconds > 6 && cast)//Ends all buffs after 6 seconds
        {
            stats.allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 2f;
            foreach (GameObject player in affectedPlayers)//removes everyone from list
            {
                player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 0.8f;
                affectedPlayers.Remove(player);
            }
            buffActive = false;
        }


        if (cooldown > 0) //counts down for the cooldown
        {
            cooldown--;
        }
    }
}
