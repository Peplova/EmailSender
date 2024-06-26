using MimeKit;

namespace EmailSender.Business.MailConfiguration;

public class Message
{
    public List<MailboxAddress> To { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public Message(IEnumerable<string> to,string from, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(email => new MailboxAddress(string.Empty, email)));
        From = from;
        Subject = subject;
        Content = content;        
    }
}