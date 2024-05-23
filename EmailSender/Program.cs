using EmailSender;
using EmailSender.MailConfiguration;
using EmailSender.MailSender;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration")); 
builder.Services.AddSingleton<IEmailSender, EmailSender.MailSender.EmailSender>();

var host = builder.Build();

host.Run();
