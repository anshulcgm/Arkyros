using System;
using System.Collections.Generic;
using UnityEngine;

class Instantiator : IClass
{
    /* parameters: None
     * returns: ObjectUpdate with all the GameObjects to instantiate at the Start.
     */
    ///@TODO: this is a stub that needs to be implemented
    public ObjectUpdate Start()
    {
        ObjectUpdate o = new ObjectUpdate();

        return o;
    }

    // returns the MonoBehavior that implements this class
    public Type MonoScript
    {
        get
        {
            return typeof(InstantiatorMono);
        }
    }
}