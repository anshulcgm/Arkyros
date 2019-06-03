using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingClientAndServer : MonoBehaviour {
    Client client;
    Server server;
    UDP udp;
    public string server_ipaddr; //to have the user manually enter the server's ip address.. for now
    public bool destroyCube = false;
    GameObject cube = null;
    // Use this for initialization
    void Start () {
        udp = new UDP(1000);
        udp.StartUDP();
        //client = new Client(server_ipaddr, gameObject, udp); //start a new client with the current IP and gameobject
        server = new Server(udp, udp);

        GameObject resource = (GameObject)Resources.Load("cube");
        cube = GameObject.Instantiate(resource, Vector3.zero, Quaternion.identity);
        server.Create(cube, "cube");
    }
	
	// Update is called once per frame
	void Update () {
        server.UpdateGameObjects();
        //client.HandleServerOutput();

        if(cube != null && destroyCube)
        {
            server.Destroy(cube);
            Destroy(cube);
        }
	}
}
