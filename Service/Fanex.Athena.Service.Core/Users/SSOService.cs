// -----------------------------------------------------------------------
// <copyright file="SSOService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Web;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Authorization;
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

        private static string CurrentIdentityName
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

        public void SSO()
        {
            if (string.IsNullOrEmpty(_keyPath))
            {
                RedirectToErrorPage(DisableSsoErrorCode);
            }

            var metaModel = ParseQueryString();

            if (string.IsNullOrEmpty(metaModel.UserName) || string.IsNullOrEmpty(metaModel.Password))
            {
                RedirectToErrorPage(InValidParamErrorCode);
            }

            if (!Util.IsValidLanguage(metaModel.LanguageCode))
            {
                metaModel.LanguageCode = DefaultLanguage;
            }

            if (IsUserAuthenticated && metaModel.IdentityName == CurrentIdentityName)
            {
                var langService = new LanguageService();
                langService.ChangeLanguage(metaModel.LanguageCode);
                _formsAuthenticationService.SetAuthCookie(metaModel.IdentityName, false);
                _context.Redirect("~/Home/Index");
                return;
            }

            var resultInfo = InternalLogin(metaModel);
            if (resultInfo.IsSuccessful)
            {
                _formsAuthenticationService.SetAuthCookie(metaModel.IdentityName, false);
                _context.Redirect("~/Home/Index");
            }
            else
            {
                LogFailedLogin(metaModel, resultInfo);
                RedirectToErrorPage(resultInfo.Code);
            }
        }

        private static void LogFailedLogin(SsoMetaModel metaModel, IResultInfo resultInfo)
        {
            var message = InternalLoginErrorMessage +
                                "\r\nCode: " + resultInfo.Code +
                                "\r\nMessage: " + resultInfo.Message +
                                "\r\nExtendedData: " + resultInfo.ExtendedData +
                                "\r\nLicensee: " + metaModel.LicenseeName +
                                 "\r\nUserName: " + metaModel.UserName +
                                 "\r\nTimeSpan: " + metaModel.TimeStamp +
                                 "\r\nSiteOutUrl: " + metaModel.LoginUrl +
                                 "\r\nLanguageCode: " + metaModel.LanguageCode;

            Logging.LogService.Info(message);
        }

        private SsoMetaModel ParseQueryString()
        {
            var queryParams = GetQueryParams();
            long timeStamp;
            long.TryParse(queryParams["timestamp"], out timeStamp);
            return new SsoMetaModel
            {
                LicenseeName = queryParams["licname"] ?? string.Empty,
                UserName = queryParams["username"] ?? string.Empty,
                Password = queryParams["pwd"] ?? string.Empty,
                LoginUrl = queryParams["loginurl"] ?? string.Empty,
                LanguageCode = queryParams["lang"] ?? DefaultLanguage,
                TimeStamp = timeStamp
            };
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

        private IResultInfo InternalLogin(SsoMetaModel metaModel)
        {
            var requestUser = new User
            {
                Username = metaModel.UserName,
                Password = metaModel.Password,
                Request = new Request
                {
                    Ip = _context.Request.UserHostAddress,
                    Browser = _context.Request.Browser?.Browser,
                    Platform = _context.Request.Browser?.Platform,
                    Domain = metaModel.LoginUrl,
                }
            };
            var resultInfo = _authService.ValidateInternalUser(requestUser, metaModel.LicenseeName, metaModel.TimeStamp);

            if (resultInfo.IsSuccessful)
            {
                var user = resultInfo.ExtendedData;
                _sessionService.CreateNewSession(
                    user.UserID,
                    _authService.LoginOnUser(user, _context.Session.SessionID, user.ServerId, _context.Request.UserHostAddress),
                    user.Username,
                    true,
                    user.Licensee.Id,
                    metaModel.LanguageCode,
                    viewType: ClientViewType.Default);
            }

            return resultInfo;
        }
    }
}