using EmailSender.Business.MailConfiguration;

namespace EmailSender.Business.MailSender;

public interface IEmailSender
{
    bool SendEmail(Message message);
}