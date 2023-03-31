using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Services.EmailServices
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
