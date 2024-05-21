namespace EmailSender.EmailConfiguration;

public interface IEmailSender
{
    void SendEmail(Message message);
}