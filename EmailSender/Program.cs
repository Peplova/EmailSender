using EmailSender;
using EmailSender.MailConfiguration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton<IEmailSender, EmailSender.MailConfiguration.EmailSender>();

var host = builder.Build();

host.Run();
