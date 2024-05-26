using EmailSender.Business.MailConfiguration;

namespace EmailSender.Requests;

//public class MailRequest
//{
//    public Message CreateMailTemplate(List<string> mailList, string subject, string content)
//    {
//        var message = new Message(mailList, subject, content);

//        return message;
//    }
//}
public class MailRequest
{
    public string From { get; set; }
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Attachment { get; set; }

}





