using System.Collections.Generic;
using UnityEngine;

public static class UnityHandler
{
    private static List<GameObject> gameObjects = new List<GameObject>();


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
        }
        else if (m.messageType == MessageType.CREATE)
        {
            Create(m.messageText); //call create function
        }
    }

    private static void Update(string message)
    {
        //all the vars we'll need 
        string[] data = message.Split('|'); ; // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger

        //get the object to update
        GameObject Object = gameObjects[int.Parse(data[index])];

        Object.transform.position = DataParserAndFormatter.StringToVector3(data[index + 1]); //parse and update position
        Object.transform.rotation = DataParserAndFormatter.StringToQuaternion(data[index + 2]); //parse and update rotation
    }

    private static void Create(string message)
    {
        string[] data; // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger
        data = message.Split('|'); //split each object into an array
        string resourcePath = data[index];
        Vector3 position = DataParserAndFormatter.StringToVector3(data[index + 1]); //parse and update position
        Quaternion orientation = DataParserAndFormatter.StringToQuaternion(data[index + 2]); //parse and update rotation
        GameObject resource = (GameObject)Resources.Load(resourcePath); //get GameObject resource from resourcePath
        GameObject Object = GameObject.Instantiate(resource, position, orientation); //make the GameObject in the scene with the correct orientation and position

        //add this object to total list of gameObjects
        gameObjects.Add(Object);
    }
}
