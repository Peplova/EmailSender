using EmailSender.Business.MailConfiguration;

namespace EmailSender.MailRecipient;

public class MailTemplateGenerator
{
    public Message CreateMailTemplate(List<string> mailList, string subject, string content)
    {
        var message = new Message(mailList, subject, content);
        
        return message;
    }
}