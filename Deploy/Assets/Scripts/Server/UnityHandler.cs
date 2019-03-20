using System.Collections.Generic;
using UnityEngine;

public static class UnityHandler
{
    private static List<GameObject> gameObjects = new List<GameObject>();
    public static bool debug = false;

    public static List<GameObject> GetAllGameObjects()
    {
        List<GameObject> copy = new List<GameObject>();
        foreach(GameObject g in gameObjects)
        {
            copy.Add(g);
        }
        return copy;
    }

    public static void HandleMessage(Message m)
    {
           
        if (m.messageType == MessageType.UPDATE)
        {
            Update(m.messageText); //call update function
            if (debug == true)
            {
                Debug.Log("UNITY HANDLER: Recieved Message of type Update");
            }
        }
        else if (m.messageType == MessageType.CREATE)
        {
            Create(m.messageText); //call create function
            if (debug == true)
            {
                Debug.Log("UNITY HANDLER: Recieved Message of type Create");
            }
        }
        
    }

    private static void Update(string message)
    {
        if (message.Equals(""))
        {
            return;
        }
        //all the vars we'll need 
        string[] data = message.Substring(1, message.Length - 1).Split('|');  // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger
        Debug.Log(data[index] + " jjjjkklkkf " + gameObjects.Count);

        //get the object to update
        GameObject Object = gameObjects[int.Parse(data[index])];
        //destroy if null
        if (Object == null)
        {
            UnityEngine.Object.Destroy(Object);
        }

        Object.transform.position = DataParserAndFormatter.StringToVector3(data[index + 1]); //parse and update position
        Object.transform.rotation = DataParserAndFormatter.StringToQuaternion(data[index + 2]); //parse and update rotation
        if (debug == true)
        {
            Debug.Log("UNITY HANDLER: Updated Object of name: " + Object.name.ToString() + "to position "+ data[index+1] + " and rotation " + data[index+2]);
        }
    }

    private static void Create(string message)
    {
        string[] data; // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger

        data = message.Substring(1, message.Length - 2).Split('|'); //split each object into an array
        string resourcePath = data[index];
        Debug.Log(resourcePath + " <- that's it");
        Vector3 position = DataParserAndFormatter.StringToVector3(data[index + 1]); //parse and update position
        Quaternion orientation = DataParserAndFormatter.StringToQuaternion(data[index + 2].Substring(1, data[index + 2].Length - 1)); //parse and update rotation
        GameObject resource = (GameObject)Resources.Load(resourcePath); //get GameObject resource from resourcePath
        GameObject Object = GameObject.Instantiate(resource, position, orientation); //make the GameObject in the scene with the correct orientation and position

        //add this object to total list of gameObjects
        gameObjects.Add(Object);

        if (debug == true)
        {
            Debug.Log("UNITY HANDLER: Created Object with resource path " + data[index] + "to position " + data[index+1] + " and rotation " + data[index+2]);
        }
    }
}
