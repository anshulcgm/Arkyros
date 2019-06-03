using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMono : MonoBehaviour {
    public bool sendDefault = false;
    public bool readFromFile = false;

    public GameObject planet;

    public GameObject playerPosition;

    public GameObject playerRotation;

    public GameObject cam;

    private Client client;
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

        UnityHandler.player = playerPosition;
        
        client = new Client(server_ipaddr, playerRotation, cam, udp);
        udp.Send(UDP.GetLocalIPAddress().ToString(), server_ipaddr);
	}
	public bool hasBeenCreated = false;
	//call whenever we send the create player data
	public void SendPlayerCreateData(string classPath, int[] abilityIds)
	{
		client.SendClassData(classPath, abilityIds);
	}
	
	// Update is called once per frame
	void Update () {
        if(client == null)
        {
            Debug.Log("uh oh");
        }
        if (sendDefault && planet.GetComponent<PlanetMono>().created)
        {
            SendPlayerCreateData("default", new int[] { 0, 0, 0, 0, 0, 0, 0 });
            sendDefault = false;
        }
        if (hasBeenCreated){			
			client.SendPlayerData();     
		}   
		client.HandleServerOutput();
	}
}