// -----------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Helpers
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using System.Web;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LogHelper
    {
        public static void WriteError(Exception exception, string additionalMessage = null)
        {
            var request = HttpContext.Current.Request;
            var content = BuildMessage(exception.Message);
            if (!string.IsNullOrWhiteSpace(additionalMessage))
            {
                content.AppendLine(additionalMessage);
            }

            content.AppendLine("SOURCE: " + exception.Source)
                   .AppendLine("TARGETSITE: " + exception.TargetSite)
                   .AppendLine("STACKTRACE: " + exception.StackTrace);

            var sqlException = exception as SqlException;
            if (sqlException != null)
            {
                content.AppendLine("Procedure: " + sqlException.Procedure)
                    .AppendLine("Line Number: " + sqlException.LineNumber)
                    .AppendLine("Server: " + sqlException.Server);
            }

            LogService.Error(content.ToString(), exception);
        }

        public static void WriteError(string message)
        {
            var content = BuildMessage(message);
            LogService.Error(content.ToString());
        }

        private static StringBuilder BuildMessage(string message)
        {
            var request = HttpContext.Current.Request;
            var builder = new StringBuilder(message).AppendLine();
            builder.AppendLine("DOMAIN: " + request.Headers["Host"])
                   .AppendLine("FORM: " + string.Join("~", request.Form))
                   .AppendLine("Url: " + request.RawUrl)
                   .AppendLine("QUERYSTRING: " + string.Join("~", request.QueryString))
                   .AppendLine("Browser:" + request.Browser.Type.ToString())
                   .AppendLine("BrowserVer:" + request.Browser.Version.ToString())
                   .AppendLine("Agent:" + request.UserAgent.ToString());

            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            if (licenseeSession != null && userSession != null)
            {
                builder.AppendLine("SystemInfo:Licensee=" + licenseeSession.LicenseeId + "-" +
                    licenseeSession.LicenseeName + ";userId=" + userSession.UserId + ";" +
                    userSession.UserName);
            }

            return builder;
        }
    }
}