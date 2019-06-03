using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBannerFunctionality : MonoBehaviour
{
    DateTime start;
    public GameObject[] array;
    public List<GameObject> affectedPlayers = new List<GameObject>();
    bool alive;

    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
        array = GameObject.FindGameObjectsWithTag("Player");
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject player in array)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 200) //if the player is within 20 units of the banner
            {
                if (!affectedPlayers.Contains(player))//if its not already in the List, add it in and give it the buff
                {
                    //this ensures that everything is only buffed once
                    player.GetComponent<Stats>().allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 1.25f;
                    player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 1.25f;
                    affectedPlayers.Add(player);
                }
            }
        }

        foreach (GameObject player in affectedPlayers)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 200)//remove if too far away and debuff
            {
                player.GetComponent<Stats>().allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 0.8f;
                player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 0.8f;
                affectedPlayers.Remove(player);
            }
        }


        if ((DateTime.Now - start).TotalSeconds > 8 && alive)//8 second life time
        {
            alive = false;
            Debug.Log("Die");
            foreach (GameObject player in affectedPlayers)
            {
                player.GetComponent<Stats>().allStats[(int)stat.Speed, (int)statModifier.Multiplier] *= 0.8f;
                player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 0.8f;
                affectedPlayers.Remove(player);
            }
            
            Destroy(this.gameObject);
        }
    }
}
