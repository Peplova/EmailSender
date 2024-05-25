using EmailSender;
using EmailSender.Business.MailConfiguration;
using EmailSender.Business.MailProcessor;
using EmailSender.Business.MailSender;
using MailKit.Net.Smtp;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration")); 
builder.Services.AddSingleton<IEmailSender, EmailSender.Business.MailSender.EmailSender>();
builder.Services.AddSingleton<ISmtpClient, SmtpClient>();
builder.Services.AddSingleton<IMailProcessor, MailProcessor>();

var host = builder.Build();

host.Run();
