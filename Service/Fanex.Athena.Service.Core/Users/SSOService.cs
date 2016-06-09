// -----------------------------------------------------------------------
// <copyright file="SSOService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web;
    using System.Web.Security;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using RSACryptoPad;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SSOService : ISSOService
    {
        private const string InternalLoginErrorMessage = "Internal login failed.";

        private const int DecryptKeySize = 1024;

        private const int InValidParamErrorCode = 1;
        private const int DisableSsoErrorCode = 2;

        private const string PrivateKey = "12bet.snk";
        private const string DefaultLanguage = "en-US";

        private readonly HttpContext _context;
        private readonly string _keyPath = string.Empty;
        private readonly string _deniedPath = string.Empty;
        private readonly ISessionService _sessionService = new SessionService();

        public SSOService()
        {
            _context = HttpContext.Current;
            _deniedPath = ConfigurationManager.AppSettings["SSOUnAuthorizePath"];
            _keyPath = ConfigurationManager.AppSettings["SSOKeyPath"] ?? string.Empty;
        }

        private static string CurrentUserName
        {
            get
            {
                var user = System.Web.HttpContext.Current.User;
                if (user != null)
                {
                    return user.Identity == null ? string.Empty : user.Identity.Name;
                }

                return string.Empty;
            }
        }

        private bool IsUserAuthenticated
        {
            get
            {
                var context = System.Web.HttpContext.Current;
                var isAuthenticated = context.User != null
                    && context.User.Identity != null
                    && context.User.Identity.IsAuthenticated
                    && context.Request.IsAuthenticated;
                var userSession = _sessionService.UserSession;
                var licenseeSession = _sessionService.LicenseeSession;
                var isSessionValid = userSession != null
                    && userSession.UserId > 0
                    && licenseeSession != null
                    && licenseeSession.LicenseeId > 0;

                return isAuthenticated && isSessionValid;
            }
        }

        /// <summary>
        /// Single sign on from internal site.
        /// </summary>
        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults",
            Justification = "Reviewed. Suppression is OK here.")]
        public void SSO()
        {
            if (string.IsNullOrEmpty(_keyPath))
            {
                RedirectToErrorPage(DisableSsoErrorCode);
            }

            var queryParams = GetQueryParams();
            var licname = queryParams["licname"] ?? string.Empty;
            var userName = queryParams["username"] ?? string.Empty;
            var passWord = queryParams["pwd"] ?? string.Empty;
            var siteOutUrl = queryParams["loginurl"] ?? string.Empty;
            var languageCode = queryParams["lang"] ?? DefaultLanguage;
            if (!Util.IsValidLanguage(languageCode))
            {
                languageCode = DefaultLanguage;
            }

            if (IsUserAuthenticated)
            {
                if (userName == CurrentUserName)
                {
                    // update language code
                    var langService = new LanguageService();
                    langService.ChangeLanguage(languageCode);
                    FormsAuthentication.SetAuthCookie(userName, false);
                    _context.Response.Redirect("~/Home/Index");
                    return;
                }
            }

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                RedirectToErrorPage(InValidParamErrorCode);
            }

            long timeSpan;
            long.TryParse(queryParams["timestamp"], out timeSpan);
            IResultInfo resultInfo;
            InternalLogin(licname, userName, passWord, timeSpan, siteOutUrl, languageCode, out resultInfo);
            if (resultInfo.IsSuccessful)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                _context.Response.Redirect("~/Home/Index");
            }
            else
            {
                string message = InternalLoginErrorMessage +
                    "\r\nCode: " + resultInfo.Code +
                    "\r\nMessage: " + resultInfo.Message +
                    "\r\nExtendedData: " + resultInfo.ExtendedData +
                    "\r\nLicensee: " + licname +
                     "\r\nUserName: " + userName +
                     "\r\nTimeSpan: " + timeSpan +
                     "\r\nSiteOutUrl: " + siteOutUrl +
                     "\r\nLanguageCode: " + languageCode;

                Logging.LogService.Info(message);

                RedirectToErrorPage(resultInfo.Code);
            }
        }

        private NameValueCollection GetQueryParams()
        {
            string parameter = GetRequestKey();
            var privateKeyPath = GetPrivateKeyPath();

            parameter = parameter.Replace(" ", "+");
            var descryptParam = new EncryptionThread().DecryptString(parameter, DecryptKeySize, privateKeyPath);

            return HttpUtility.ParseQueryString(descryptParam);
        }

        private string GetPrivateKeyPath()
        {
            var privateKeyPath = _context.Server.MapPath("~/" + _keyPath + PrivateKey);
            if (!File.Exists(privateKeyPath))
            {
                RedirectToErrorPage(DisableSsoErrorCode);
            }

            return privateKeyPath;
        }

        private string GetRequestKey()
        {
            string parameter = HttpContext.Current.Request["key"] ?? string.Empty;

            if (string.IsNullOrEmpty(parameter))
            {
                RedirectToErrorPage(InValidParamErrorCode);
            }

            return parameter;
        }

        private void RedirectToErrorPage(int code)
        {
            _context.Response.Redirect(Util.RootPath + string.Format(_deniedPath, code));
        }

        /// <summary>
        /// Internal sign in.
        /// </summary>
        /// <param name="licname">The licensee name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="passWord">The pass word.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="siteOutUrl">The sign out url.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="resultInfo">The result information.</param>
        private void InternalLogin(
          string licname,
          string userName,
          string passWord,
          long timeSpan,
          string siteOutUrl,
          string languageCode,
          out IResultInfo resultInfo)
        {
            var userService = new UserService();
            var requestUser = new User
            {
                Username = userName,
                Password = passWord,
                Request = new Request
                {
                    Ip = _context.Request.UserHostAddress,
                    Browser = _context.Request.Browser.Browser,
                    Platform = _context.Request.Browser.Platform,
                    Domain = siteOutUrl,
                }
            };
            IResultInfo loginResultInfo;
            User user = userService.ValidateInternalUser(requestUser, licname, timeSpan, out loginResultInfo);

            if (loginResultInfo.IsSuccessful)
            {
                _sessionService.CreateNewSession(
                    user.Id,
                    userService.LoginOnUser(user, _context.Session.SessionID, user.ServerId, _context.Request.UserHostAddress),
                    user.Username,
                    true,
                    user.Licensee.Id,
                    languageCode);
            }

            resultInfo = loginResultInfo;
        }
    }
}