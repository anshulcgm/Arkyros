using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDP
{
    //the port number
    private const int PORT_NUMBER = 15000;
    //the asynchronous thread we'll be working on
    private Thread t = null;
    //our most recent message
    public string mostRecentMessage = null;
    //the UdpClient
    private readonly UdpClient udp = new UdpClient(PORT_NUMBER);

    public string broadcastAddr = null;

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
        //print out so we know stuff work
      //  Debug.Log("Started listening");
        //actually start listening for clients talking
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

        //Debug.Log("From " + ip.Address.ToString() + " received: " + mostRecentMessage);

        //if this is an update or create messsage message
        if (mostRecentMessage.StartsWith("U") || mostRecentMessage.StartsWith("C")) {

            UnityUpdater UnityUpdater = new UnityUpdater();
            UnityUpdater.UnityHandler(mostRecentMessage,mostRecentMessage[0]);
        }

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
       // Debug.Log("Sent: " + message);
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
      //  Debug.Log("Sent: " + message + " to " + ipAddr);
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




public class UnityUpdater
{
    private static Vector3 StringToVector3(string vector)
    {
        //get rid of parantheses
        if (vector.StartsWith("(") && vector.EndsWith(")"))
        {
            vector = vector.Substring(1, vector.Length - 2);
        }

        string[] array = vector.Split(','); //sperate with each ","

        //store it all in a vector3
        Vector3 result = new Vector3(
            float.Parse(array[0]),
            float.Parse(array[1]),
            float.Parse(array[2])
            );

        return result;
    }

    //pretty much same parsing as in Vector3 code ^^^
    private static Quaternion StringToQuaternion(string quat)
    {
        //identify parantheses and get rid of them
        if (quat.StartsWith("(") && quat.EndsWith(")"))
        {

            quat = quat.Substring(1, quat.Length - 2);
        }

        string[] array = quat.Split(','); //split in every , then store in array

        // add it all to a quat
        Quaternion result = new Quaternion(
            float.Parse(array[0]),
            float.Parse(array[1]),
            float.Parse(array[2]),
            float.Parse(array[3])
            );

        return result;
    }
    public void UnityHandler(string mostRecentMessage, char type)
    {
        
        if (type == 'U')
        {
            Updater(mostRecentMessage.Substring(2)); //call update function
        }

        else if (type == 'C')
        {
            Creator(mostRecentMessage.Substring(2)); //call create function, notice how we cut the U from the string and the | following it
        }
       
    }
    private void Updater(string message) {
        //all the vars we'll need 
        string[] data = message.Split('|'); ; // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger
        GameObject Object = GameObject.Find(data[index]);

        Object.transform.position = StringToVector3(data[index + 1]); //parse and update position
        Object.transform.rotation = StringToQuaternion(data[index + 2]); //parse and update rotation
        //and so on
    }

    private void Creator(string message) {
        //some vars
        GameObject Object = new GameObject();
        string[] data; // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger
        data = message.Split('|'); //split each object into an array

        Object.name = data[index + 1]; //name the object
        Object.transform.position = StringToVector3(data[index + 1]); //parse and update position
        Object.transform.rotation = StringToQuaternion(data[index + 2]); //parse and update rotation
    }
}



public class BasicClient : MonoBehaviour
{

    // Use this for initialization
    UDP udp;
    Vector3 Position;
    //notice Quaternion for rotation
    Quaternion Rotation;
    GameObject ThisObject;

    void Start()
    {
        //id is unique, better
        ThisObject = GameObject.Find(gameObject.name); //identify this object

        //start udp stuff
        udp = new UDP();
        udp.StartUDP();



        Rotation = new Quaternion(); //initialize rotation var

        /*
         * Stuff needed for identifying the object's shape, rigid body , type of collider, etc.
         * 
         * 
         * 
         * 
         * */
        //string Objtype = PrefabUtility.GetPrefabObject(ThisObject).ToString(); //identify prefab

        //send the create function
        udp.SendBroadcastOnLAN("C " + "|" + ThisObject.name + "|" + Position.ToString("R") + "|" + Rotation.ToString("R") + "|");

    }

    // Update is called once per frame
    void Update()
    {

        Position = ThisObject.transform.position; //update postion of cube
        Rotation = ThisObject.transform.rotation; //update rotation of cube

        //update all of this^^^
        udp.SendBroadcastOnLAN("U " + "|" + ThisObject.name +  "|" + Position.ToString("R") + "|" + Rotation.ToString("R") + "|");

        /*
         * identification of what is being sent + position vector3 + rotation quaternion + end identification
         * notice '|' for easy string splitting
         *
        */
    }
}