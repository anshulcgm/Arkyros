using System;
using System.Collections.Generic;
using UnityEngine;

public class DataParserAndFormatter
{
    //these are for the client inputs to the server, not used for now
    static string beginKeyInput = "{{";
    static string endKeyInput = "}}";

    static string beginMouseInput = "((";
    static string endMouseInput = "))";
    static string beginOrientationInput = "[[";
    static string endOrientationInput = "]]";

    static string beginIpInput = "^^";
    static string endIpInput = "**";

    static string beginAbilityIdInput = "@@";

    static string endAbilityIdInput = "!!";

    static string beginClassPathInput = "??";

    static string endClassPathInput = "&&";

    static string beginCamPosInput = "%%";

    static string endCamPosInput = "$$";

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

    public static string GetKeysIn(string clientInput){
        return clientInput.Split(new string[] {beginKeyInput}, StringSplitOptions.None)[1].Split(new string[] {endKeyInput}, StringSplitOptions.None)[0];
    }
    public static bool[] GetMouseIn(string clientInput){
        String mouseIn = clientInput.Split(new string[] {beginMouseInput}, StringSplitOptions.None)[1].Split(new string[] {endMouseInput}, StringSplitOptions.None)[0];
        char[] mouseVals = mouseIn.ToCharArray();
        return new bool[]{mouseVals[0] == 'T', mouseVals[1] == 'T'};
    }

    public static Quaternion[] GetRotationIn(string clientInput){
        String rotIn = clientInput.Split(new string[] {beginOrientationInput}, StringSplitOptions.None)[1].Split(new string[] {endOrientationInput}, StringSplitOptions.None)[0];
        String rotation = rotIn.Split('|')[0];
        String camRotation = rotIn.Split('|')[1];
        return new Quaternion[]{StringToQuaternion(rotation), StringToQuaternion(camRotation)};
    }

    public static Vector3 GetCamPos(string clientInput){
        String posIn = clientInput.Split(new string[] {beginCamPosInput}, StringSplitOptions.None)[1].Split(new string[] {endCamPosInput}, StringSplitOptions.None)[0];
        return StringToVector3(posIn);
    }

    public static string GetIP(string clientInput){
        return clientInput.Split(new string[] {beginIpInput}, StringSplitOptions.None)[1].Split(new string[] {endIpInput}, StringSplitOptions.None)[0];
    }

    public static string GetClassPath(string clientInput){
        return clientInput.Split(new string[] {beginClassPathInput}, StringSplitOptions.None)[1].Split(new string[] {endClassPathInput}, StringSplitOptions.None)[0];
    }

    public static int[] GetAbilityIds(string clientInput){
        string abilities = clientInput.Split(new string[] {beginAbilityIdInput}, StringSplitOptions.None)[1].Split(new string[] {endAbilityIdInput}, StringSplitOptions.None)[0];
        string[] abilitiesParsed = abilities.Split('|');
        int[] abilityIds = new int[abilitiesParsed.Length];
        for(int i = 0; i < abilitiesParsed.Length; i++){
            abilityIds[i] = int.Parse(abilitiesParsed[i]);
        }
        return abilityIds;
    }

    public static String GetClassPathAndAbilityIdsFormatted(string classpath, int[] abilityIds, string clientIp){
        String ids = "";
        foreach(int id in abilityIds){
            ids += id + "|";
        }        
        ids = ids.Substring(0, ids.Length - 1);
        return beginClassPathInput + classpath + endClassPathInput + beginAbilityIdInput + ids + endAbilityIdInput + beginIpInput + clientIp + endIpInput;
    }

    #endregion

    #region formatting
    public static string GetClientInputFormatted(string keysPressed, bool m1Down, bool m2Down, Quaternion rotation, Quaternion camRotation, Vector3 camPosition, string ipAddr)
    {
        string fullString = beginKeyInput;
        fullString += keysPressed;
        fullString += endKeyInput;
        fullString += beginMouseInput;
        fullString += m1Down ? "T" : "F";
        fullString += m2Down ? "T" : "F";
        fullString += endMouseInput;
        fullString += beginOrientationInput;
        fullString += rotation;
        fullString += "|";
        fullString += camRotation;
        fullString += endOrientationInput;
        fullString += beginCamPosInput;
        fullString += camPosition;
        fullString += endCamPosInput;
        fullString += beginIpInput;
        fullString += ipAddr;
        fullString += endIpInput;
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
