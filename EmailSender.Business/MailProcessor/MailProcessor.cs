using EmailSender.Business.MailConfiguration;
using EmailSender.Business.MailSender;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;

namespace EmailSender.Business.MailProcessor;

public class MailProcessor : IMailProcessor
{
    private readonly IEmailSender _emailSender;
    private readonly ISmtpClient _smtpClient;
    private readonly ILogger<MailProcessor> _logger;
    
    public MailProcessor(IEmailSender sender, ILogger<MailProcessor> logger, ISmtpClient smtpClient)
    {
        _emailSender = sender;
        _logger = logger;
        _smtpClient = smtpClient;
    }

    public async Task<bool> ProcessEndSendMail(Message message)
    {
        try
        {
            return await _emailSender.SendEmailAsync(message);
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
}