using EmailSender.MailConfiguration;

namespace EmailSender.MailSender;

public interface IEmailSender
{
    void SendEmail(Message message);
}