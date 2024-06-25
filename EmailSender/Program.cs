using System.Text.Json;
using EmailSender;
using EmailSender.Business.MailConfiguration;
using EmailSender.Business.MailProcessor;
using EmailSender.Business.MailSender;
using EmailSender.Business.Rabbit;
using EmailSender.Configuration;
using MailKit.Net.Smtp;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.GetConfigurationFromEnvironment();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MailRequestConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("mail_shared", e => { e.ConfigureConsumer<MailRequestConsumer>(context); });
    });
});

builder.Services.AddSingleton<IEmailSender, EmailSender.Business.MailSender.EmailSender>();
builder.Services.AddSingleton<ISmtpClient, SmtpClient>();
builder.Services.AddSingleton<IMailProcessor, MailProcessor>();

var host = builder.Build();

host.Run();