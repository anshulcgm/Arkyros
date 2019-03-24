using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster {
    public int numberOfAbilities = 7;
    public bool[] unlockRecorder ;
   

    public AbilityCaster()
    {
        unlockRecorder = new bool[numberOfAbilities];
        for(int i = 0; i < numberOfAbilities; i++)
        {
            unlockRecorder[i] = false;
        }
    }

    public void checkUnlock(int ID)
    {
        if (!unlockRecorder[ID]) //if not already unlocked
        {
            if (!unlockRecorder[getPreReq(ID)])
            {
                Console.WriteLine("Missing prerequisite");
            }
            else
            {
                unlockRecorder[ID] = true;
            }
        }
        else Console.WriteLine("Already unlocked");
    }

    public int getPreReq(int ID) //file REEEEading
    {
        return 0;
    }

}
