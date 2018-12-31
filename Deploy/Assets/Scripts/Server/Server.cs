using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Server
{
    private UDP udp;
    private List<Player> players;
    private List<GameObject> gameObjectsToUpdate;

    public Server()
    {
        udp = new UDP();
        players = new List<Player>();
        gameObjectsToUpdate = new List<GameObject>();
    }
    
    ///@TODO this function needs to be finished. It should send a broadcast on the LAN with all the things
    ///required by the UnityHandler.Create function.
    public void Create(GameObject g, string resourcePath)
    {
        udp.SendBroadcastOnLAN("C{" + resourcePath + "|" + g.transform.position.ToString() + "|" + g.transform.rotation.ToString() + "}"); //first the resource path given, then g's position then rotation all compiled into a string seperated by a "|", see UnityHandler.cs for more info
        
        
        //add the new object to the list of objects that need to be updated.
        gameObjectsToUpdate.Add(g);
    }

    ///@TODO this function needs to be finished. It should send a broadcast on the LAN for 
    ///each gameObject in gameObjectsToUpdate with all the things required by the UnityHandler.Update function.
    public void UpdateGameObjects()
    {
        //loops through each gameObject in the list of game objects to update and sends a broadcast for updating them to the clients
        for (int g = 0; g < gameObjectsToUpdate.Count(); g += 1) {
            udp.SendBroadcastOnLAN("U{" + g.ToString() + "|" + gameObjectsToUpdate[g].transform.position.ToString() + "|" + gameObjectsToUpdate[g].transform.rotation.ToString() +"}");
        }
        
        
    }

    ///@TODO this function needs to be finished. It should take all of the data recieved by players and move them accordingly.
    public void HandlePlayerInputs()
    {

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


