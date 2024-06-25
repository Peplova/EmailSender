using EmailSender.Business.MailConfiguration;
using EmailSender.Business.MailProcessor;
using MassTransit;
using Messaging.Shared;
using Microsoft.Extensions.Logging;

namespace EmailSender.Business.Rabbit;

public class MailRequestConsumer : IConsumer<MailRequest>
{
    private readonly ILogger<MailRequestConsumer> _logger;
    private readonly IMailProcessor _mailProcessor;

    public MailRequestConsumer(ILogger<MailRequestConsumer> logger, IMailProcessor mailProcessor)
    {
        _logger = logger;
        _mailProcessor = mailProcessor;
    }

    public async Task Consume(ConsumeContext<MailRequest> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Received message:From: {message.From}, To={string.Join(", ", message.To)}, Subject={message.Subject}");

        var mailMessage = new Message(message.To, message.From, message.Subject, message.Body);
        var isSending = await _mailProcessor.ProcessEndSendMail(mailMessage);

        if (isSending)
            _logger.LogInformation("Email sent successfully");
        else
            _logger.LogError("Error sending email");
    }
}