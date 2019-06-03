using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMono : MonoBehaviour {
    Client client;
    UDP udp;
    public string server_ipaddr;
	// Use this for initialization
	void Start () {
		server_ipaddr = System.IO.File.ReadAllText("ipAddr.txt");
		udp = GameObject.FindGameObjectWithTag("UDP").GetComponent<UDPContainer>().udp;
        udp.StartUDP(); 
        client = new Client(server_ipaddr, gameObject, udp);
        udp.Send(UDP.GetLocalIPAddress().ToString(), server_ipaddr);
	}
	bool hasBeenCreated = false;
	//call whenever we send the create player data
	public void SendPlayerCreateData(string classPath, int[] abilityIds)
	{
		client.SendClassData(classPath, abilityIds);
		hasBeenCreated = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(hasBeenCreated){			
			client.SendPlayerData();     
		}   
		client.HandleServerOutput();
	}
}