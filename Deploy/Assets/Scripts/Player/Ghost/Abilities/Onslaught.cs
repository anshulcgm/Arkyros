using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onslaught : MonoBehaviour
{
    Stats stats;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    void Update()
    {

    }

    public new void onKill()
    {
        // REDUCE OWN COOLDOWNS BY 1/3 OR SOME BULLSHIT IDK MAN
    }

    public new void damageTaken(float damage)
    {

    }
}