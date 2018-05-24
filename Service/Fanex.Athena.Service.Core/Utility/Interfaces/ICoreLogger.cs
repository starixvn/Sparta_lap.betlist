namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface ICoreLogger
    {
        void Error(string message);

        void Error(System.Exception exception, string additionalMessage = null);

        void Info(string message);
    }
}