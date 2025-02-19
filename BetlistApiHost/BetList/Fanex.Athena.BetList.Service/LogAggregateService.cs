namespace Fanex.Athena.BetList.Service
{
    using System;
    using System.Text;
    using Interfaces;
    using Logging;

    public class LogAggregateService : ILogAggregateService
    {
        private readonly StringBuilder _logBuilder = new StringBuilder();

        public void Append(string line)
        {
            _logBuilder.AppendLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: {line}");
        }

        public void SendLog()
        {
            var msg = _logBuilder.ToString();
            Logger.Log.Info(msg);
        }
    }
}