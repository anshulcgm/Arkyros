using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDP
{
    //the port number
    private int PORT_NUMBER = 1500;
    //the asynchronous thread we'll be working on
    private Thread t = null;
    //our most recent message
    private string mostRecentMessage = "";
    private List<string> allMessages = new List<string>();
    //the UdpClient
    private readonly UdpClient udp;
    public string broadcastAddr = null;

    public UDP(int port){
        this.PORT_NUMBER = port;
        udp = new UdpClient(PORT_NUMBER);
    }

    #region start and stop functions
    //starts the udp client
    public void StartUDP()
    {
        //if the listener thread is not null, then give exception. Programmer is stoopid, decided to start the client twice.
        if (t != null)
        {
            throw new Exception("Already started, stop first");
        }
        string localIP = GetLocalIPAddress();
        string[] split = localIP.Split('.');
        broadcastAddr = split[0] + "." + split[1] + "." + split[2] + ".255";
            
        StartListening();
    }

    //stop
    public void Stop()
    {
        try
        {
            udp.Close();
          //  Debug.Log("Stopped listening");
        }
        catch { /* don't care */ }
    }
    #endregion

    #region listening for client messages
    //this is what stores the asynchronous result
    IAsyncResult ar_ = null;
    //start listening on port, call recieve function when we've recieved the packet of info.
    private void StartListening()
    {
        ar_ = udp.BeginReceive(Receive, new object());
    }
    //recieve our packet of info.
    private void Receive(IAsyncResult ar)
    {
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
        byte[] bytes = udp.EndReceive(ar, ref ip);
        mostRecentMessage = Encoding.ASCII.GetString(bytes);
        allMessages.Add(mostRecentMessage);
        StartListening();
    }
    #endregion

    //sends broadcast to everybody on the local network
    public void SendBroadcastOnLAN(string message)
    {
        UdpClient client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(broadcastAddr), PORT_NUMBER);
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        client.Send(bytes, bytes.Length, ip);
        client.Close();
        StartListening();
    }

    //sends string message to person w/ipAddress whatever
    public void Send(string message, string ipAddr)
    {
        UdpClient client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ipAddr), PORT_NUMBER);
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        client.Send(bytes, bytes.Length, ip);
        client.Close();
    }

    public List<string> ReadMessages()
    {
        List<string> copy = new List<string>();
        //copy over elements of list
        for(int i = 0; i < allMessages.Count; i++)
        {
            copy.Add(allMessages[i]);
        }
        //clear all messages
        allMessages = new List<string>();
        //return the copy
        return copy;
    }

    public String ReadMostRecentMessage()
    {
        return mostRecentMessage;
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

}
