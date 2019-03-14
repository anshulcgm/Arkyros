
//class for holding messages
public class Message
{
    public MessageType messageType;
    public string messageText;
    public Message(MessageType messageType, string messageText)
    {
        this.messageType = messageType;
        this.messageText = messageText;
    }
}

//enum for holding the types of messages
public enum MessageType { UPDATE, CREATE };
