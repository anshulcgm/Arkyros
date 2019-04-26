using System.Collections.Generic;
using UnityEngine;

public class DataParserAndFormatter
{
    //these are for the client inputs to the server
    static string beginKeyInput = "{{";
    static string endKeyInput = "}}";
    static string beginOrientationInput = "[[";
    static string endOrientationInput = "]]";

    //these are for the server outputs to the client
    static string beginServerMessageOutput = "{";
    static string endServerMessageOutput = "}";

    #region parsing
    public static Vector3 StringToVector3(string vector)
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
    public static Quaternion StringToQuaternion(string quat)
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
    #endregion

    #region formatting
    public static string GetClientInputFormatted(string keysPressed, string rotation)
    {
        string fullString = beginKeyInput;
        fullString += keysPressed;
        fullString += endKeyInput;
        fullString += beginOrientationInput;
        fullString += rotation;
        fullString += endOrientationInput;
        return fullString;
    }

    public static List<Message> GetMessagesFromServerOutput(string serverOutput)
    {
        List<Message> messages = new List<Message>();

        string[] messagesStrs = serverOutput.Split('}');

        foreach(string s in messagesStrs)
        {
            if(s.Length == 0)
            {
                continue;
            }
            string messageType = s.Substring(0, 1);
            string messageText = s.Substring(1, s.Length -  1);
            Message m = null;
            if (messageType.Equals("U"))
            {
                m = new Message(ServerMessageType.UPDATE, messageText);
            }
            else if (messageType.Equals("C"))
            {
                m = new Message(ServerMessageType.CREATE, messageText);
            }
            else if (messageType.Equals("D"))
            {
                m = new Message(ServerMessageType.DESTROY, messageText);
            }
            else if (messageType.Equals("T"))
            {
                m = new Message(ServerMessageType.TERRAIN, messageText);
            }
            else if (messageType.Equals("A"))
            {
                m = new Message(ServerMessageType.ANIMATION, messageText);
            }
            messages.Add(m);
        }
        return messages;
    }
    #endregion
}
