using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public int[] selectedAbilities = new int[7];
    public MonoBehaviourArray[] monoBehaviours;
    // Start is called before the first frame update
    public void Initialize()
    {
        for (int i = 0; i < monoBehaviours.Length; i++)
        {
            int ability = selectedAbilities[i];
            for (int i1 = 0; i1 < monoBehaviours[i].Length(); i++)
            {
                if (i1 == ability)
                {
                    monoBehaviours[i][i1].enabled = true;

                }
                else
                {
                    monoBehaviours[i][i1].enabled = false;
                }
            }
        }
    }

    public void changeAbilitySelection(int tier, int ability)
    {
        monoBehaviours[tier][selectedAbilities[tier]].enabled = false;
        selectedAbilities[tier] = ability;
        monoBehaviours[tier][selectedAbilities[tier]].enabled = true;
    }


}





[System.Serializable]
public class MonoBehaviourArray
{
    public MonoBehaviour[] monobehaviours;

    public MonoBehaviour this[int i]
    {
        get
        {
            return monobehaviours[i];
        }
        set
        {
            monobehaviours[i] = value;
        }
    }

    public int Length()
    {
        return monobehaviours.Length;
    }
}


