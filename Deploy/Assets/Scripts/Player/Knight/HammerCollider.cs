using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCollider : MonoBehaviour
{
    public int currentDamage;
    public bool active;
    public Stats stats;
    DateTime start;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active && (DateTime.Now - start).TotalSeconds > 1)//turns off collider after one second
        {
            active = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && active)
        {
            stats.dealDamage(other.gameObject, currentDamage);
        }
    }

    public void setActive(int damage)
    {
        currentDamage = damage;
        active = true;
        start = DateTime.Now;
    }
}
