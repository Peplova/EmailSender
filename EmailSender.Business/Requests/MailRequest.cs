namespace Messaging.Shared;

public class MailRequest
{
    public string From { get; set; }
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}