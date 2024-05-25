using EmailSender.Business.MailConfiguration;
using EmailSender.Business.MailProcessor;

namespace EmailSender
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMailProcessor _mailProcessor;

        public Worker(ILogger<Worker> logger, IMailProcessor mailProcessor)
        {
            _logger = logger;
            _mailProcessor = mailProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    
                    try
                    {
                        var message = new Message(new string[] { "xnesart@mail.ru" }, "Test email", "Hello.");
                        var isSending = _mailProcessor.ProcessEndSendMail(message);
                    
                        if (isSending) _logger.LogInformation("Email sent in: {time}", DateTimeOffset.Now);
                        else _logger.LogError("Email sent in: {time}", DateTimeOffset.Now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "An unexpected error occurred while sending email.");
                        throw;
                    }
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}