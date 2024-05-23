using EmailSender.MailConfiguration;
using EmailSender.MailSender;

namespace EmailSender
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailSender _emailSender;

        public Worker(ILogger<Worker> logger, IEmailSender sender)
        {
            _logger = logger;
            _emailSender = sender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var message = new Message(new string[] { "xnesart@mail.ru" }, "Test email", "Hello.");
                    _emailSender.SendEmail(message);
                    _logger.LogInformation("Email sent in: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
