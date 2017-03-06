using System.Threading.Tasks;

namespace MicroSB.Server.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
