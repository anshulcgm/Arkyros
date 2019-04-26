using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

public class Server
{
    private UDP udp;
    private List<Player> players;
    private List<GameObject> gameObjectsToUpdate;
    public bool debug = true;
    
    public Server(UDP udp)
    {
        this.udp = udp;
        players = new List<Player>();
        gameObjectsToUpdate = new List<GameObject>();
        if (debug == true)
        {
           // Debug.Log("SERVER: new istance was created");
        }
    }
    
    ///@TODO this function needs to be finished. It should send a broadcast on the LAN with all the things
    ///required by the UnityHandler.Create function.
    public void Create(GameObject g, string resourcePath)
    {
        string message = "C{" + resourcePath + "|" + g.transform.position.ToString() + "|" + g.transform.rotation.ToString() + "}";
        if (debug == true)
        {
            Debug.Log("SERVER: sent out a create statement: ");
            Debug.Log(message);
        }

        //sending to all clients
        foreach (string clientIP in clientIPs)
        {
            udp.Send(message, clientIP);
        }

        //add the new object to the list of objects that need to be updated.
        gameObjectsToUpdate.Add(g);
    }

    ///@TODO this function needs to be finished. It should send a broadcast on the LAN for 
    ///each gameObject in gameObjectsToUpdate with all the things required by the UnityHandler.Update function.
    public void UpdateGameObjects()
    {
        //loops through each gameObject in the list of game objects to update and sends a broadcast for updating them to the clients
        for (int g = 0; g < gameObjectsToUpdate.Count(); g += 1) {
            if(gameObjectsToUpdate[g] == null) { continue; } //skip if it has been destroyed
            string message = "U{" + g.ToString() + "|" + gameObjectsToUpdate[g].transform.position.ToString() + "|" + gameObjectsToUpdate[g].transform.rotation.ToString() + "}";

            //sending to all clients
            foreach (string clientIP in clientIPs)
            {
                udp.Send(message, clientIP);
            }
        }
        if (debug == true)
        {
            Debug.Log("SEVER: Updated " + gameObjectsToUpdate.Count().ToString() + " Objects");
        }

    }
    //the destroy function
    public void Destroy(GameObject g)
    {
        int gindex = gameObjectsToUpdate.IndexOf(g);
        gameObjectsToUpdate[gindex] = null; //sets the gameobject to null

        //create the message and send to all the clients
        string message = "D{" + gindex.ToString() + "}";

        foreach (string clientIP in clientIPs)
        {
            udp.Send(message, clientIP);
        }
    }

    public void SendTerrainSeed(int seed)
    {
        string message = "T{" + seed + "}";

        foreach (string clientIP in clientIPs)
        {
            udp.Send(message, clientIP);
        }
    }

    public void SendAnimation(GameObject Object, string animation_name, bool isAnimating)
    {
        string boolchar = "F";
        if (isAnimating)
        {
            boolchar = "T";
        }
        string message = "A{" + gameObjectsToUpdate.IndexOf(Object).ToString() + "|" + animation_name + "|" + boolchar + "}");
        foreach (string clientIP in clientIPs)
        {
            udp.Send(message, clientIP);
        }
    }

    public List<string> clientIPs = new List<string>();
    public void GetClients()
    {
        List<string> messages = udp.ReadMessages();
        foreach(string message in messages)
        {
            //if the client message is an ip address, then add it to the list of clients. Otherwise, ignore.
            IPAddress addr = null;
            if (IPAddress.TryParse(message, out addr))
            {
                clientIPs.Add(message);
            }
        }        
    }

    //private because we only want to access it from here
    private class Player
    {
        string ipAddr;
        GameObject playerGameObject;
        public Player(string ipAddr, GameObject playerGameObject)
        {
            this.ipAddr = ipAddr;
            this.playerGameObject = playerGameObject;
        }
    }
}


