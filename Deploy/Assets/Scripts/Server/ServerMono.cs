using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMono : MonoBehaviour
{
    public static Server server = null;
    public bool waitForClients = true;
    public bool waitForClientCreateMessages = true;
    public bool hasSentTerrainSeed = false;
    public bool createdTerrain = false;

    public bool hasCreatedPlayers = false;

    public static UDP udp;
    public static UDP udpListen;

    // Start is called before the first frame update
    void Start()
    {
        udp = new UDP(15000);
        udpListen = new UDP(15001);
        udp.StartUDP();
        udpListen.StartUDP();
        //start the server
        server = new Server(udp, udpListen);
    }
    int seed = 0;
    // Update is called once per frame
    void Update()
    {
        if(server == null){
            Debug.Log("this is sad");
        }
        //wait for clients to enter the game, save each client.
        if (waitForClients)
        {
            server.GetClients();
            return;
        }
        //if we stop waiting for people to join, send out the terrain seed. Don't send the seed out more than once.
        else if (!hasSentTerrainSeed)
        {
            //get the terrain seed and send it to all the clients
            seed = (int)((UnityEngine.Random.value) * (int.MaxValue - 1));
            server.SendTerrainSeed(seed);
            hasSentTerrainSeed = true;
            return;
        }
        else if(!createdTerrain){
            //find the planet, make it active, set the seed and generate it.
            GameObject planet = GameObject.FindGameObjectWithTag("planet");
            planet.GetComponent<PlanetMono>().Create(seed);
            createdTerrain = true;
            return;
        }
        else if(waitForClientCreateMessages){
            return;
        }
        else if(!hasCreatedPlayers){
            server.CreatePlayers();
            hasCreatedPlayers = true;
            return;
        }
        //after getting clients and sending terrain, just continue updating all gameObjects for all clients.
        server.UpdateGameObjects();
        server.HandleClientInput();
    }
}
