using EmailSender.MailConfiguration;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EmailSender.MailSender;

public class EmailSender : IEmailSender
{
    private readonly IOptionsMonitor<EmailConfiguration> _emailConfig;
    private readonly ILogger<Worker> _logger;
    private readonly ISmtpClient _smtpClient;


    public EmailSender(IOptionsMonitor<EmailConfiguration> config, ILogger<Worker> logger, ISmtpClient smtpClient)
    {
        _emailConfig = config;
        _smtpClient = smtpClient;
        _logger = logger;
    }

    public void SendEmail(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    private void Send(MimeMessage mailMessage)
    {
        try
        {
            var emailConfig = _emailConfig.CurrentValue;

            _smtpClient.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
            _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            _smtpClient.Authenticate(emailConfig.UserName, emailConfig.Password);
            _smtpClient.Send(mailMessage);
        }
        catch (SmtpCommandException ex) when (ex.ErrorCode == SmtpErrorCode.RecipientNotAccepted)
        {
            // Ловит ошибки, связанные с конкретным получателем
            _logger.LogError(ex, "Failed to deliver email to {FailedRecipient}", ex.Mailbox);
            throw;
        }
        catch (SmtpCommandException ex) when (ex.ErrorCode == SmtpErrorCode.SenderNotAccepted)
        {
            // Ловит ошибки, связанные с отправителем
            _logger.LogError(ex, "Sender address {Sender} not accepted by the SMTP server.", ex.Mailbox);
            throw;
        }
        catch (SmtpCommandException ex) when (ex.ErrorCode == SmtpErrorCode.MessageNotAccepted)
        {
            // Ловит ошибки, связанные с сообщением
            _logger.LogError(ex, "Message not accepted by the SMTP server.");
            throw;
        }
        catch (SmtpProtocolException ex)
        {
            // Ловит ошибки, связанные с SMTP протоколом
            _logger.LogError(ex, "Protocol error while sending email.");
            throw;
        }
        catch (Exception ex)
        {
            // Ловит все остальные ошибки
            _logger.LogError(ex, "An unexpected error occurred while sending email.");
            throw;
        }
        finally
        {
            _smtpClient.Disconnect(true);
        }
    }
    
    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailConfig = _emailConfig.CurrentValue;
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("email", emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;
    }
}