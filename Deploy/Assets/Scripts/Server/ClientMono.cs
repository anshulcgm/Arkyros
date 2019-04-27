using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMonoBehaviour : MonoBehaviour {
    Client client;
    UDP udp;
    public string server_ipaddr; //have the user enter the server_ipaddr for now
	// Use this for initialization
	void Start () {
		udp = new UDP();
        udp.StartUDP(); 
        client = new Client(server_ipaddr, gameObject, udp);
        udp.Send(UDP.GetLocalIPAddress().ToString(), server_ipaddr);
	}
	
	// Update is called once per frame
	void Update () {
        client.SendPositionAndOrientation();
		client.HandleServerOutput();
	}
}