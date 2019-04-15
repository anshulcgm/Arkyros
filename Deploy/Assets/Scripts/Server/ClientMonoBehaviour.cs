using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMonoBehaviour : MonoBehaviour {
    Client client;
    UDP udp;
    string server_ipaddr;
	// Use this for initialization
	void Start () {
		udp = new UDP();
        udp.StartUDP();
        server_ipaddr = ""; //find way to get ip addr
        client = new Client(server_ipaddr, gameObject, udp);
	}
	
	// Update is called once per frame
	void Update () {
		client.HandleServerOutput();
	}
}