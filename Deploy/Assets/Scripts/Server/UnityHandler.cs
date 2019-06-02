using System.Collections.Generic;
using UnityEngine;

public static class UnityHandler
{
    public static GameObject player;

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
           
        if (m.messageType == ServerMessageType.UPDATE)
        {
            Update(m.messageText); //call update function
            if (debug == true)
            {
                Debug.Log("UNITY HANDLER: Recieved Message of type Update");
            }
        }
        else if (m.messageType == ServerMessageType.CREATE)
        {
            Create(m.messageText); //call create function
            if (debug == true)
            {
                Debug.Log("UNITY HANDLER: Recieved Message of type Create");
            }
        }
        else if (m.messageType == ServerMessageType.DESTROY)
        {
            Destroy(m.messageText);
        }
        else if (m.messageType == ServerMessageType.TERRAIN)
        {
            Terrain(m.messageText);
        }
        else if (m.messageType == ServerMessageType.ANIMATION)
        {
            Animate(m.messageText);
        }
    }


    private static void Update(string message)
    {
        if (message.Equals(""))
        {
            return;
        }
        //all the vars we'll need 
        string[] data = message.Substring(1, message.Length - 2).Split('|');  // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger
        Debug.Log(data[index] + " jjjjkklkkf " + gameObjects.Count);
        
        //if it's a player, just update the position.
        if(gameObjects[int.Parse(data[index])].Equals(player)){
            player.transform.position = DataParserAndFormatter.StringToVector3(data[index + 1]);
            return;
        }

        //get the object to update
        GameObject Object = gameObjects[int.Parse(data[index])];
        //ignore if null
        if (Object != null)
        {
            Object.transform.position = DataParserAndFormatter.StringToVector3(data[index + 1]); //parse and update position
            Object.transform.rotation = DataParserAndFormatter.StringToQuaternion(data[index + 2]); //parse and update rotation
            if (debug == true)
            {
                Debug.Log("UNITY HANDLER: Updated Object of name: " + Object.name.ToString() + "to position "+ data[index+1] + " and rotation " + data[index+2]);
            }
        }
    }

    private static void Create(string message)
    {
        string[] data; // for splitting string
        int index = 0; // will be easier when mostRecentMessage gets larger

        data = message.Substring(1, message.Length - 2).Split('|'); //split each object into an array
        if (!data[index].Equals("player")){        //if this create message is for me then add a null object and move on
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
        else {
            gameObjects.Add(player);
        }
    }
    
    private static void Destroy(string message)
    {
        string data = message.Substring(1, message.Length - 2);
        int index = int.Parse(data);
        GameObject.Destroy(gameObjects[index]); //destroy the object then set it to a null
        gameObjects[index] = null;     
    }
    
    private static void Terrain(string message)
    {
        string string_seed = message.Substring(1, message.Length - 2); // retrieve seed from string
        int seed = int.Parse(string_seed); //parse into an integar
        //find the planet, activate it, set the seed then generate
        GameObject planet = GameObject.FindGameObjectWithTag("planet");
        planet.SetActive(true);
        planet.GetComponent<PlanetMono>().Create(seed);
    }

    private static void Animate(string message)
    {
        string[] data;
        int index = 0;
        data = message.Substring(1, message.Length - 2).Split('|'); //split the message
        if (data[index + 1] == "T")        //call overlay depending on True or False
        {
            //call overlay when true
            gameObjects[int.Parse(data[index])].GetComponent<AnimationController>().StartOverlayAnim(data[index+2], float.Parse(data[index+3]), float.Parse(data[index+4]));
        }
        else
        {
            gameObjects[int.Parse(data[index])].GetComponent<AnimationController>().PlayLoopingAnim(data[index+2]);
        }       
    }
}
