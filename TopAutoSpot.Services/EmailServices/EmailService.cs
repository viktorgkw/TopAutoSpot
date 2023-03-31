using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailDto request)
        {
            MimeMessage email = new();
            email.From.Add(MailboxAddress.Parse(_configuration["Email:Username"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using SmtpClient smtp = new();
            smtp.Connect(_configuration["Email:Host"],
                587,
                SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["Email:Username"], _configuration["Email:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
