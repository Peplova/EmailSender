using EmailSender.Business.MailConfiguration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailSender.Business.MailSender;

public class EmailSender : IEmailSender
{
    private readonly IOptionsMonitor<EmailConfiguration> _emailConfig;
    private readonly ILogger<EmailSender> _logger;
    private readonly ISmtpClient _smtpClient;

    public EmailSender(IOptionsMonitor<EmailConfiguration> config, ILogger<EmailSender> logger, ISmtpClient smtpClient)
    {
        _emailConfig = config;
        _smtpClient = smtpClient;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(Message message)
    {
        _logger.LogInformation($"{DateTime.Now} Creating email at time in EmailSender class");
        var emailMessage = CreateEmailMessage(message);
        _logger.LogInformation($"{DateTime.Now} Call to Send method in EmailSender class");
        var result = await SendAsync(emailMessage);
        if (result)
        {
            _logger.LogInformation($"{DateTime.Now} Called the Send method successfully, email sent");
        }
        else
        {
            _logger.LogError($"{DateTime.Now} Something in the Send method is wrong, email is not sent");
        }

        return result;
    }

    private async Task<bool> SendAsync(MimeMessage mailMessage)
    {
        var result = false;

        _logger.LogInformation($"{DateTime.Now} Set emailConfig in Send method in EmailSender class");
        var emailConfig = _emailConfig.CurrentValue;

        _logger.LogInformation($"{DateTime.Now} SMTP CLient connecting in Send method in EmailSender class");
        await _smtpClient.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true);
        _logger.LogInformation(
            $"{DateTime.Now} SMTP CLient remove the auth mechanisms in Send method in EmailSender class");
        _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
        _logger.LogInformation(
            $"{DateTime.Now} SMTP CLient attempt to authentification in Send method in EmailSender class");
        await _smtpClient.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
        _logger.LogInformation(
            $"{DateTime.Now} SMTP CLient attempt to Send message in Send method in EmailSender class");
        await _smtpClient.SendAsync(mailMessage);
        result = true;

        return result;
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