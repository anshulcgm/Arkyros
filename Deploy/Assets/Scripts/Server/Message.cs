
//class for holding messages
public class Message
{
    public ServerMessageType messageType;
    public string messageText;
    public Message(ServerMessageType messageType, string messageText)
    {
        this.messageType = messageType;
        this.messageText = messageText;
    }
}

//enum for holding the types of messages
public enum ServerMessageType { UPDATE, CREATE, DESTROY, TERRAIN, ANIMATION, PLAYER};
