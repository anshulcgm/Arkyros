using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IpAddrGUI : MonoBehaviour {


    string ServerIP = "Enter Server IP";
    

    void OnGUI()
    {
        // Make a text field that modifies ServerIP
        ServerIP = GUI.TextField(new Rect(240, 140, 200, 20), ServerIP, 25); //Text field for taking in ip addr
        if (GUI.Button(new Rect(315, 175, 50, 30), "Start")) //bool button to start the game
        {
            BeginClient();
        }
    }
   void BeginClient()
    {
        //access the file, creates or opens, then writes in the server ip taken from the gui
        StreamWriter writer = File.CreateText("ipAddr.txt");
        writer.Write(ServerIP);
        writer.Close();
        //####################SHIVEN EDIT HERE####################
        //SceneManager.LoadScene("");    
    }
}
