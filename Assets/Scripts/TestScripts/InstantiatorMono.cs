using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MonoBehavior implementation of a the Instantiator class
public class InstantiatorMono : MonoBehaviour, IMono {

    // Instantiator class
    Instantiator instantiator = new Instantiator();

    // set and return the Instantiator class
    public IClass GetMainClass()
    {
        return instantiator;
    }
    public void SetMainClass(IClass ic)
    {
        instantiator = (Instantiator)ic;
    }

    // Use this for initialization
    void Start () {
        // this creates alll the prefabs required at the start of the game
        ObjectUpdate o = instantiator.Start();
        ObjectHandler.Update(o, gameObject);
	}
}
