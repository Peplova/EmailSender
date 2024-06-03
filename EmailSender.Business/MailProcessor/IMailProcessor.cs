using EmailSender.Business.MailConfiguration;
using MimeKit;

namespace EmailSender.Business.MailProcessor;

public interface IMailProcessor
{
    Task<bool> ProcessEndSendMail(Message message);
}