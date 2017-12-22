// -----------------------------------------------------------------------
// <copyright file="SSOService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;
    using Utility;
    using Utility.Interfaces;

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

        private readonly IHttpContextService _context;
        private readonly string _keyPath;
        private readonly string _deniedPath;

        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;
        private readonly IRSAEncryptor _rsaEncryptor;
        private readonly IFileSystem _fileSystem;
        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public SSOService()
        {
            _context = new HttpContextService();
            _authService = new AuthService();
            _sessionService = new SessionService();
            _rsaEncryptor = new RSAEncryptor();
            _fileSystem = new FileSystem();
            _formsAuthenticationService = new FormsAuthenticationService();
            _deniedPath = ConfigurationManager.AppSettings["SSOUnAuthorizePath"];
            _keyPath = ConfigurationManager.AppSettings["SSOKeyPath"] ?? string.Empty;
        }

#pragma warning disable S107 // Methods should not have too many parameters
        public SSOService(
            IHttpContextService httpContextService,
            IAuthService authService,
            ISessionService sessionService,
            IRSAEncryptor rsaEncryptor,
            IFileSystem fileSystem,
            IFormsAuthenticationService formsAuthenticationService,
            string deniedPath,
            string keyPath)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _context = httpContextService;
            _authService = authService;
            _sessionService = sessionService;
            _rsaEncryptor = rsaEncryptor;
            _fileSystem = fileSystem;
            _formsAuthenticationService = formsAuthenticationService;
            _deniedPath = deniedPath;
            _keyPath = keyPath;
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

            if (IsUserAuthenticated && userName == CurrentUserName)
            {
                var langService = new LanguageService();
                langService.ChangeLanguage(languageCode);
                _formsAuthenticationService.SetAuthCookie(userName, false);
                _context.Redirect("~/Home/Index");
                return;
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
                _formsAuthenticationService.SetAuthCookie(userName, false);
                _context.Redirect("~/Home/Index");
            }
            else
            {
                var message = InternalLoginErrorMessage +
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
            var parameter = GetRequestKey();
            var privateKeyPath = GetPrivateKeyPath();

            parameter = parameter.Replace(" ", "+");
            var descryptParam = _rsaEncryptor.DecryptString(parameter, DecryptKeySize, privateKeyPath);

            return HttpUtility.ParseQueryString(descryptParam);
        }

        private string GetPrivateKeyPath()
        {
            var privateKeyPath = _context.MapPath("~/" + _keyPath + PrivateKey);
            if (!_fileSystem.Exists(privateKeyPath))
            {
                RedirectToErrorPage(DisableSsoErrorCode);
            }

            return privateKeyPath;
        }

        private string GetRequestKey()
        {
            var parameter = HttpContext.Current.Request["key"] ?? string.Empty;

            if (string.IsNullOrEmpty(parameter))
            {
                RedirectToErrorPage(InValidParamErrorCode);
            }

            return parameter;
        }

        private void RedirectToErrorPage(int code)
        {
            _context.Redirect(Util.RootPath + string.Format(_deniedPath, code));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        private void InternalLogin(
          string licname,
          string userName,
          string passWord,
          long timeSpan,
          string siteOutUrl,
          string languageCode,
          out IResultInfo resultInfo)
        {
            var requestUser = new User
            {
                Username = userName,
                Password = passWord,
                Request = new Request
                {
                    Ip = _context.Request.UserHostAddress,
                    Browser = _context.Request.Browser?.Browser,
                    Platform = _context.Request.Browser?.Platform,
                    Domain = siteOutUrl,
                }
            };
            IResultInfo loginResultInfo;
            var user = _authService.ValidateInternalUser(requestUser, licname, timeSpan, out loginResultInfo);

            if (loginResultInfo.IsSuccessful)
            {
                _sessionService.CreateNewSession(
                    user.UserID,
                    _authService.LoginOnUser(user, _context.Session.SessionID, user.ServerId, _context.Request.UserHostAddress),
                    user.Username,
                    true,
                    user.Licensee.Id,
                    languageCode,
                    viewType: ClientViewType.Default);
            }

            resultInfo = loginResultInfo;
        }
    }
}