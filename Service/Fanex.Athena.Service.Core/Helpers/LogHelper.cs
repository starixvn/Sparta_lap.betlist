// -----------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Fanex.Athena.Service.Core.Users;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LogHelper
    {
        public static void WriteError(Exception exception, string additionalMessage = null)
        {
            var data = GetCustomInfo(additionalMessage);
            Logging.Extensions.Web.ExceptionHandler.Handle(exception, data);
        }

        public static void WriteError(string message)
        {
            var data = GetCustomInfo();
            var jsonData = new JavaScriptSerializer().Serialize(data);
            Logging.LogService.Error(message + "\n" + jsonData);
        }

        private static IDictionary<string, object> GetCustomInfo(string message = null)
        {
            var dictionary = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(message))
            {
                dictionary.Add("Message", message);
            }

            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            if (licenseeSession != null && userSession != null)
            {
                dictionary.Add("Licensee", licenseeSession.LicenseeId + "-" + licenseeSession.LicenseeName);
                dictionary.Add("User", userSession.UserId + "-" + userSession.UserName);
            }

            return dictionary;
        }
    }
}