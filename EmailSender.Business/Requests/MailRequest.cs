namespace Messaging.Shared;

public class MailRequest
{
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}