namespace email_tool.shared.Models;

public class MessageModel
{
    public MessageModel(string sender,  string recipient, string subject, string body)
    {
        Sender = sender;
        Recipient = recipient;
        Subject = subject;
        Body = body;
    }
    


    public string Sender { get; set; }
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
