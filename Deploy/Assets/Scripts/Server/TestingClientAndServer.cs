using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingClientAndServer : MonoBehaviour {
    Client client;
    Server server;
    UDP udp;
    public string server_ipaddr; //to have the user manually enter the server's ip address.. for now
    // Use this for initialization
    void Start () {
        udp = new UDP();
        udp.StartUDP();
        client = new Client(server_ipaddr, gameObject, udp); //start a new client with the current IP and gameobject
        server = new Server(udp);
        ObjectHandler.server = server;

        ObjectUpdate o = new ObjectUpdate();
        for(int i = 0; i < 2; i++)
            o.AddInstantiationRequest(new InstantiationRequest("cube", Vector3.zero, Quaternion.identity));
        ObjectHandler.Update(o, gameObject);
    }

    // Update is called once per frame
    void Update () {
        server.UpdateGameObjects();
        client.HandleServerOutput();
	}
}
