using System.Text.Json;
using EmailSender.Business.MailConfiguration;

namespace EmailSender.Configuration;

public static class ConfigurationFromEnvironment
{
    public static void GetConfigurationFromEnvironment(this IServiceCollection services)
    {
        var emailConfig = GetSerializedJson();

        services.Configure<EmailConfiguration>(options =>
        {
            options.From = emailConfig.From;
            options.SmtpServer = emailConfig.SmtpServer;
            options.Port = emailConfig.Port;
            options.Username = emailConfig.Username;
            options.Password = emailConfig.Password;
        });
    }

    private static EmailConfiguration GetSerializedJson()
    {
        var emailConfigurationJson = Environment.GetEnvironmentVariable("EmailConfiguration");
        if (string.IsNullOrEmpty(emailConfigurationJson))
            throw new InvalidOperationException("Email configuration not found in environment variables.");


        var emailConfig = JsonSerializer.Deserialize<EmailConfiguration>(emailConfigurationJson);

        return emailConfig;
    }
}