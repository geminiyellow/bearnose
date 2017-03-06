using System.Threading.Tasks;

namespace MicroSB.Server.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
