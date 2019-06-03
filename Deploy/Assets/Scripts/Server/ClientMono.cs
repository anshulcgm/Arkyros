using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMono : MonoBehaviour {
    public bool sendDefault = false;
    public bool readFromFile = false;

    public GameObject player;

    public GameObject cam;

    Client client;
    UDP udp;
    public string server_ipaddr;
	// Use this for initialization
	void Start () {
        if (readFromFile)
        {
            server_ipaddr = System.IO.File.ReadAllText("ipAddr.txt");
        }		
        udp = new UDP();
        udp.StartUDP();
        client = new Client(server_ipaddr, player, cam, udp);
        udp.Send(UDP.GetLocalIPAddress().ToString(), server_ipaddr);
        if (sendDefault)
        {
            client.SendClassData("default", new int[] { 0, 0, 0, 0, 0, 0, 0 });
        }
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