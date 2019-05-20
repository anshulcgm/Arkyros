using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour
{
    public Dictionary<string, Passive> passives = new Dictionary<string, Passive>();
    // Start is called before the first frame update
    void Start()
    {
        //load all passives into dictionary
    }
    //return new instance of a certain passive ability
    public Passive get(string name)
    {
        foreach(string s in passives.Keys)
        {
            if (s.Equals(name))
            {
                return passives[s];
            }
        }
        return null;
    }
}
