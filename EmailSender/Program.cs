using EmailSender;
using EmailSender.MailConfiguration;
using EmailSender.MailSender;
using MailKit.Net.Smtp;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration")); 
builder.Services.AddSingleton<IEmailSender, EmailSender.MailSender.EmailSender>();
builder.Services.AddSingleton<ISmtpClient, SmtpClient>();

var host = builder.Build();

host.Run();
