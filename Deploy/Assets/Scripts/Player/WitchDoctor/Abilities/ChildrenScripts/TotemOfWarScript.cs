using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemOfWarScript : MonoBehaviour
{
    DateTime start;
    public GameObject[] array;
    public List<GameObject> affectedPlayers = new List<GameObject>();
    int lifetime = 8;
    int radius = 20;
    float multiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.Now;
        array = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject player in array)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= radius) //if the player is within 20 units of the totem
            {
                if (!affectedPlayers.Contains(player))//if its not already in the List, add it in and give it the buff
                {
                    //this ensures that everything is only buffed once
                    player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= multiplier;
                    affectedPlayers.Add(player);
                }
            }
        }

        foreach (GameObject player in affectedPlayers)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > radius)//remove if too far away and debuff
            {
                player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] /= multiplier;
                affectedPlayers.Remove(player);
            }
        }


        if ((DateTime.Now - start).TotalSeconds > lifetime)//8 second life time
        {
            foreach (GameObject player in affectedPlayers)
            {
                player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] /= multiplier;
                affectedPlayers.Remove(player);
            }
            Destroy(this.gameObject);
        }
    }

    public void augment()
    {
        lifetime += 12;
        radius += 10;
        multiplier = 2f;

        foreach (GameObject player in affectedPlayers)
        {
            player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] *= 4f;
            player.GetComponent<Stats>().allStats[(int)stat.Attack, (int)statModifier.Multiplier] /= 3f;
        }
    }
}
