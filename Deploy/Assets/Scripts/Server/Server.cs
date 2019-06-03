﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class Server
{
    private Vector3 spawn = new Vector3(0,5000,0);
    private UDP udp;
    private UDP udpListen;
    private List<PlayerClient> players;
    public static List<GameObject> gameObjectsToUpdate;
    public bool debug = false;

    public static bool serverExists = false;

    public static Server instance = null;
    
    public Server(UDP udp, UDP updListen)
    {
        serverExists = true;
        this.udp = udp;
        this.udpListen = updListen;
        players = new List<PlayerClient>();
        gameObjectsToUpdate = new List<GameObject>();
        //instance = this;
    }
    
    // this function sends a broadcast on the LAN with all the things necessary for client to creat the new object
    public void Create(GameObject g, string resourcePath, string ip = "")
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
            if (clientIP.Equals(ip))
            {
                udp.Send("C{player}", ip);
            }
            else
            {
                udp.Send(message, clientIP);
            }
        }

        //add the new object to the list of objects that need to be updated.
        gameObjectsToUpdate.Add(g);
    }

    //this function sends all the necessary things for the clinet to update the objrct
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
        string message = "D{" + gindex.ToString() + "1}";

        foreach (string clientIP in clientIPs)
        {
            udp.Send(message, clientIP);
        }
    }

    public void SendTerrainSeed(int seed)
    {
        string message = "T{" + seed + "1}";

        foreach (string clientIP in clientIPs)
        {
            Debug.Log("sent " + message + " to " + clientIP);
            udp.Send(message, clientIP);
        }
    }

    public void SendAnimation(int index, string animation_name, bool isOverlay, float strength = 0, float duration = 0)
    {
        string message;        
        if (isOverlay)
        {
            message = "A{" + index + "|" + "T" + "|" + animation_name + strength.ToString() + duration.ToString() +"}"; // gameobject index with animation and false char
        }
        else
        {
            message = "A{" + index + "|" + "F" + "|" + animation_name + "}"; //game obj, and other overlay parameters
        }
        
        foreach (string clientIP in clientIPs)
        {
            udp.Send(message, clientIP);
        }
    }

    public List<string> clientIPs = new List<string>();
    public void GetClients()
    {
        List<string> messages = udpListen.ReadMessages();
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

    public void CreatePlayers(){
        List<string> messages = udpListen.ReadMessages();
        foreach(string message in messages){
            string ip = DataParserAndFormatter.GetIP(message);
            string classPath = DataParserAndFormatter.GetClassPath(message);
            int[] abilityIds = DataParserAndFormatter.GetAbilityIds(message);
            GameObject player = GameObject.Instantiate(Resources.Load(classPath) as GameObject, spawn, Quaternion.identity);            
            player.GetComponent<PlayerScript>().SetAbilities(abilityIds);
            players.Add(new PlayerClient(ip, player, classPath));
            Create(player, classPath, ip);
         }
    }

    public void HandleClientInput(){
        List<string> messages = udpListen.ReadMessages();
        foreach(string message in messages){
            string ip = DataParserAndFormatter.GetIP(message);
            for(int i = 0; i < players.Count; i++){
                if(players[i].ipAddr.Equals(ip)){
                    Quaternion[] rots = DataParserAndFormatter.GetRotationIn(message);
                    players[i].playerGameObject.transform.rotation = rots[0];
                    players[i].playerGameObject.GetComponent<PlayerScript>().HandleInput(DataParserAndFormatter.GetKeysIn(message), rots[0], DataParserAndFormatter.GetCamPos(message), DataParserAndFormatter.GetMouseIn(message));
                }
            }
        }
    }

    //private because we only want to access it from here
    private class PlayerClient
    {
        public string ipAddr;
        public GameObject playerGameObject;
        public string classPath;
        public PlayerClient(string ipAddr, GameObject playerGameObject, string classPath)
        {
            this.ipAddr = ipAddr;
            this.playerGameObject = playerGameObject;
            this.classPath = classPath;
        }
    }
}


