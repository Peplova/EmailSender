namespace EmailSender.MailConfiguration;

public interface IEmailSender
{
    void SendEmail(Message message);
}