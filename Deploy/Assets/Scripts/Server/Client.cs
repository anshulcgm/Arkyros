using System;
using System.Collections.Generic;
using UnityEngine;


// the Client class sends player inputs (for now, just keys and player orientation) to the Server
// it also processes and sends the data from the server to the UnityHandler object.
public class Client
{

    UDP udp;
    GameObject player;
    GameObject camera;
    string serverIP;
    public bool debug = false;

    public Client(string serverIP, GameObject player, GameObject camera, UDP udp)
    {
        this.udp = udp;
        this.player = player;
        this.camera = camera;
        this.serverIP = serverIP;
        if (debug == true)
        {
           // Debug.Log("CLIENT: new instance created");
        }
    }
    public void SendPlayerData()
    {        
        string clientData = DataParserAndFormatter.GetClientInputFormatted(Input.inputString, Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1), player.transform.rotation, camera.transform.rotation, camera.transform.position, UDP.GetLocalIPAddress());
        udp.Send(clientData, serverIP); //send position and orientation and ipaddr of client to server for update
    }

    public void SendClassData(String classPath, int[] abilityIds){
        udp.Send(DataParserAndFormatter.GetClassPathAndAbilityIdsFormatted(classPath, abilityIds, UDP.GetLocalIPAddress()), serverIP);
    }

    //reads in server output and does what the server says
    public void HandleServerOutput()
    {
        List<string> serverOutput = udp.ReadMessages();
       
        //get the whole output in one string, from oldest to newest messages
        string fullOutput = "";
        foreach (string s in serverOutput)
        {
            fullOutput += s;
        }

        if (debug == true)
        {
            Debug.Log("CLIENT: recieved stuff from server: \n" + fullOutput);
        }

        //turn the string into a nice list of messages
        List<Message> messages = DataParserAndFormatter.GetMessagesFromServerOutput(fullOutput);

        //handle each message (oldest to newest)
        foreach (Message message in messages)
        {
            UnityHandler.HandleMessage(message);
            if (debug == true)
            {
              //  Debug.Log("CLIENT: Sent message to Unity Handler");
            }
        }
    }
}
