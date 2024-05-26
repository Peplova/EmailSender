using EmailSender.Business.MailConfiguration;
using MimeKit;

namespace EmailSender.Business.MailProcessor;

public interface IMailProcessor
{
    bool ProcessEndSendMail(Message message);
}