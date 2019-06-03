using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPContainer : MonoBehaviour
{
    public UDP udp;
    public void Awake(){
        udp = new UDP(1000);
    }
}