using System;
using System.Collections.Generic;
using UnityEngine;


// the Client class sends player inputs (for now, just keys and player orientation) to the Server
// it also processes and sends the data from the server to the UnityHandler object.
public class Client
{
    UDP udp;
    GameObject player;
    string serverIP;
    public bool debug = true;

    public Client(string serverIP, GameObject player, UDP udp)
    {
        this.udp = udp;
        this.player = player;
        this.serverIP = serverIP;
        if (debug == true)
        {
           // Debug.Log("CLIENT: new instance created");
        }
    }

    //sends player input to the serverIP
    public void SendPlayerInput()
    {
        string allKeysPressed = "";
        //loop through all ASCII chars, if the char is pressed, add it to string of keys pressed.
        for (int i = 0; i < 128; i++)
        {
            string keyPressed = Convert.ToChar(i) + "";
            if (Input.GetKeyDown(keyPressed))
            {
                allKeysPressed += keyPressed;
            }
        }
        string rotationStr = player.transform.rotation.ToString();
        string formattedInput = DataParserAndFormatter.GetClientInputFormatted(allKeysPressed, rotationStr);
        udp.Send(formattedInput, serverIP);
        if (debug == true)
        {
           // Debug.Log("Sent player input to " + serverIP);
        }
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
