using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using EmailSender.Business.MailConfiguration;
using EmailSender.Business.MailSender;
using Moq;
using Message = EmailSender.Business.MailConfiguration.Message;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace EmailSender.Business.Test;


public class EmailSenderTest
{


    private readonly Mock<IOptionsMonitor<EmailConfiguration>> _configEmailMock;
    private readonly Mock<ILogger<IEmailSender>> _loggerMock;
    private readonly Mock<ISmtpClient> _smtpClientMock;

    public EmailSenderTest()
    {
        _configEmailMock = new Mock<IOptionsMonitor<EmailConfiguration>>();
        _loggerMock = new Mock<ILogger<EmailSender.Business.MailSender.IEmailSender>>();
        _smtpClientMock = new Mock<ISmtpClient>();
    }

    [Fact]
    public void SendEmail_SuccessSend_Recerved()
    {
        // Arrange
        var sender = new MailSender.EmailSender(_configEmailMock.Object, (ILogger<MailSender.EmailSender>)_loggerMock.Object, _smtpClientMock.Object);
        var message = new Message
        {
            To = new List<MailboxAddress> { new MailboxAddress("recipient", "recipient@mail.ru") },
            Subject = "Test Subject",
            Content = "Test Content"
        };
        
        var emailConfig = new EmailConfiguration
        {
            SmtpServer = "smtp.example.com",
            Port = 587,
            UserName = "testuser",
            Password = "testpassword",
            From = "sender@example.com"
        };
        _configEmailMock.Setup(x => x.CurrentValue).Returns(emailConfig);

        // Act
        var result = sender.SendEmail(message);

        // Assert
        Assert.True(result);
        _smtpClientMock.Verify(
            x => x.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()),
            Times.Once);
        _smtpClientMock.Verify(
            x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        _smtpClientMock.Verify(x => x.Send(It.IsAny<MimeMessage>()), Times.Once);
    }
}





    