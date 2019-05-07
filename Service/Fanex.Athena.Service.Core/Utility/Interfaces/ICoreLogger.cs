using System.Threading.Tasks;

namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface ICoreLogger
    {
        Task ErrorAsync(string message);

        void Error(System.Exception exception, string additionalMessage = null);

        Task InfoAsync(string message);
    }
}