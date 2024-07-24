using EmailSender.Business.MailConfiguration;

namespace EmailSender.Business.MailSender;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(Message message);
}