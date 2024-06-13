using english_learning_server.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace english_learning_server.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly Random _random = new Random();

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(_config["Smtp:SenderEmail"]));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            //send email
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate(_config["Smtp:SenderEmail"], _config["Smtp:AppPassword"]);
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }

        public string GenerateOTP()
        {
            return _random.Next(100000, 999999).ToString();
        }
    }
}
