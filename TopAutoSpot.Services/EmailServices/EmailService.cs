namespace TopAutoSpot.Services.EmailServices
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
    using MimeKit.Text;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// This class realizes the emailing services used through the website to email clients.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method sends email to the specified client.
        /// The client data is stored in the EmailDto parameter.
        /// </summary>
        /// <param name="request">This is EmailDto which contains data about the client's email.</param>
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
