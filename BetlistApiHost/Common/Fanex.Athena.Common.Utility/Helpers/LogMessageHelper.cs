using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Fanex.Data;
namespace Fanex.Athena.Common.Utility.Helpers
{
    public static class LogMessageHelper
    {
        public static string GenerateMessage(Exception exception)
        {
            var webInfo = GetRequestInfo(HttpContext.Current.Request);
            var dbInfo = GetObjectDbExceptionInfo(exception);
            var messageBuilder = new StringBuilder();

            foreach (var item in webInfo)
            {
                messageBuilder.AppendLine($"\t{item.Key}: {item.Value}");
            }
            foreach (var item in dbInfo)
            {
                messageBuilder.AppendLine($"\t{item.Key}: {item.Value}");
            }
            messageBuilder.Append($"\t ErrMsg: {exception.Message}");
            messageBuilder.Append($"\t StackTrace: {exception.StackTrace}");
            return messageBuilder.ToString();
        }

        private static IDictionary<string, object> GetObjectDbExceptionInfo(Exception ex)
        {
            if (ex is ObjectDbException)
            {
                var dbException = ex as ObjectDbException;
                var connString = new SqlConnectionStringBuilder(dbException.ConnectionString);

                return new Dictionary<string, object>
                {
                    ["Server"] = connString.DataSource,
                    [nameof(dbException.DbName)] = dbException.DbName,
                    [nameof(connString.UserID)] = connString.UserID,
                    [nameof(dbException.SpName)] = dbException.SpName,
                    ["Parameters"] = GetParameters(dbException.Parameters),
                    [nameof(dbException.CommandTimeout)] = dbException.CommandTimeout
                };
            }
            return new Dictionary<string, object>();
        }

        private static string GetParameters(ICollection parameters)
           => parameters.Count > 0
               ? parameters.Cast<KeyValuePair<string, object>>()
                       .Select(parameter => $"{parameter.Key}={parameter.Value}")
                       .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
               : string.Empty;
        private static IDictionary<string, object> GetRequestInfo(
            HttpRequest request)
            => request == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>
                {
                    [nameof(request.Url)] = request.Url,
                    [nameof(request.UrlReferrer)] = request.UrlReferrer,
                    [nameof(request.Form)] = request.Form,
                    [nameof(request.UserHostAddress)] = request.UserHostAddress,
                    [nameof(request.UserAgent)] = GetUserAgent(request.ServerVariables),
                    ["Client IP"] = GetClientIP(request.ServerVariables),
                };
        private static string GetClientIP(NameValueCollection serverVariables)
        {
            var clientIP = serverVariables["HTTP_X_FORWARDED_FOR"]
                ?? serverVariables["REMOTE_ADDR"]
                ?? string.Empty;

            return string.IsNullOrEmpty(clientIP) || clientIP.Equals("::1")
                ? "127.0.0.1"
                : clientIP;
        }

        private static string GetUserAgent(NameValueCollection serverVariables)
            => serverVariables["HTTP_USER_AGENT"];
    }
}
