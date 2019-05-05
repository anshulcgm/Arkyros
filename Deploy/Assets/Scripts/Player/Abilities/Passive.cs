using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : MonoBehaviour
{
    public int tier = 0;
    abstract public void On();
    abstract public void Off();
}
