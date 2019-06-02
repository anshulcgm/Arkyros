
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
<<<<<<< HEAD
public enum ServerMessageType { UPDATE, CREATE, DESTROY};
=======
public enum ServerMessageType { UPDATE, CREATE, DESTROY, TERRAIN, ANIMATION};
>>>>>>> b873df93343e0b7a58bc826d57d8259e1bd7cd25
