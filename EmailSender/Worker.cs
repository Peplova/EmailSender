using EmailSender.Business.MailConfiguration;
using EmailSender.Business.MailProcessor;
using MassTransit;
using Messaging.Shared;

namespace EmailSender
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}