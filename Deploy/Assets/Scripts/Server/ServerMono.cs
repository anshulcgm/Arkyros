using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMono : MonoBehaviour
{
<<<<<<< HEAD
    public UDP udp = new UDP();
    public Server server;
    public bool waitForClients = true;
    public bool hasSentTerrainSeed = false;

=======
    public Server server;
    public bool waitForClients = true;
    public bool waitForClientCreateMessages = true;
    public bool hasSentTerrainSeed = false;

    public bool hasCreatedPlayers = false;

>>>>>>> b873df93343e0b7a58bc826d57d8259e1bd7cd25
    private void Awake()
    {
        //deactivate all gameObjects except yourself
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject g in gameObjects)
        {
            if (!g.Equals(gameObject))
            {
                g.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
=======
        UDP udp = GameObject.FindGameObjectWithTag("UDP").GetComponent<UDPContainer>().udp;
>>>>>>> b873df93343e0b7a58bc826d57d8259e1bd7cd25
        //start the server
        server = new Server(udp);
    }

    // Update is called once per frame
    void Update()
    {
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
            int seed = (int)((UnityEngine.Random.value) * (int.MaxValue - 1));
            server.SendTerrainSeed(seed);

            //find the planet, make it active, set the seed and generate it.
            GameObject planet = GameObject.FindGameObjectWithTag("planet");
            planet.SetActive(true);
            planet.GetComponent<PlanetMono>().Create(seed);

            //activate all the pther objects
            GameObject[] gameObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject g in gameObjects)
            {
                if (!g.Equals(gameObject) && !g.Equals(planet))
                {
                    g.SetActive(true);
                }
            }
            
            hasSentTerrainSeed = true;
            return;
        }
<<<<<<< HEAD

        //after getting clients and sending terrain, just continue updating all gameObjects for all clients.
        server.UpdateGameObjects();
=======
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
>>>>>>> b873df93343e0b7a58bc826d57d8259e1bd7cd25
    }
}
