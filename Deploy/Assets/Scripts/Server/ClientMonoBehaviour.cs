using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMonoBehaviour : MonoBehaviour {
    Client client;
    UDP udp;
    public string server_ipaddr;
	// Use this for initialization
	void Start () {
		udp = new UDP();
        udp.StartUDP();
        server_ipaddr = ""; //have the user enter the server_ipaddr for now
        client = new Client(server_ipaddr, gameObject, udp);
	}
	
	// Update is called once per frame
	void Update () {
		client.HandleServerOutput();
	}
}