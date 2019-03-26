using System.Collections.Generic;
using System.Web.Script.Serialization;
using Fanex.Athena.Service.Core.Utility.Interfaces;
using Fanex.Athena.Session.Interfaces;
using Fanex.Logging;

namespace Fanex.Athena.Service.Core.Utility
{
    public class CoreLogger : ICoreLogger
    {
        private readonly ILogger _logger;

        private readonly ISessionStorage _sessionStorage;

        [System.CLSCompliant(false)]
        public CoreLogger(ILogger logger, ISessionStorage sessionStorage)
        {
            _logger = logger;
            _sessionStorage = sessionStorage;
        }

        public void Error(System.Exception exception, string additionalMessage = null)
        {
            var data = GetCustomInfo(additionalMessage);
            Logging.Extensions.Web.ExceptionHandler.Handle(exception, data);
        }

        public void Error(string message)
        {
            var data = GetCustomInfo();
            var jsonData = new JavaScriptSerializer().Serialize(data);
            _logger.ErrorAsync(message + "\n" + jsonData);
        }

        public void Info(string message)
        {
            _logger.InfoAsync(message);
        }

        private IDictionary<string, object> GetCustomInfo(string message = null)
        {
            var dictionary = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(message))
            {
                dictionary.Add("Message", message);
            }

            var licenseeSession = _sessionStorage.LicenseeSession;
            var userSession = _sessionStorage.UserSession;
            if (licenseeSession != null && userSession != null)
            {
                dictionary.Add("Licensee", licenseeSession.LicenseeId + "-" + licenseeSession.LicenseeName);
                dictionary.Add("User", userSession.UserId + "-" + userSession.UserName);
            }

            return dictionary;
        }
    }
}