// -----------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Helpers
{
    using System;
    using System.Data.SqlClient;
    using System.Web;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LogHelper
    {
        public static void WriteError(Exception exception)
        {
            var request = HttpContext.Current.Request;
            string message = BuildMessage(exception.Message) +
                             "\r\nSOURCE: " + exception.Source +
                             "\r\nTARGETSITE: " + exception.TargetSite +
                             "\r\nSTACKTRACE: " + exception.StackTrace;

            var sqlException = exception as SqlException;
            if (sqlException != null)
            {
                message += "\r\nProcedure: " + sqlException.Procedure +
                    "\r\nLine Number: " + sqlException.LineNumber +
                    "\r\nServer: " + sqlException.Server;
            }
            
            LogService.Error(message, exception);
        }

        public static void WriteError(string message)
        {
            message = BuildMessage(message);
            LogService.Error(message);
        }

        private static string BuildMessage(string message)
        {
            var request = HttpContext.Current.Request;
            message += "\r\nDOMAIN: " + request.Headers["Host"] +
                                         "\r\nFORM: " + string.Join("~", request.Form) +
                                         "\r\nUrl: " + request.RawUrl +
                                         "\r\nQUERYSTRING: " + string.Join("~", request.QueryString) +
                                         "\r\nBrowser:" + request.Browser.Type.ToString() +
                                         "\r\nBrowserVer:" + request.Browser.Version.ToString() +
                                         "\r\nAgent:" + request.UserAgent.ToString();

            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            if (licenseeSession != null)
            {
                message += "\r\nSystemInfo:Licensee=" + licenseeSession.LicenseeId + "-" +
                    licenseeSession.LicenseeName + ";userId=" + licenseeSession.UserId + ";" +
                    licenseeSession.UserName;
            }

            return message;
        }
    }
}
