using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
