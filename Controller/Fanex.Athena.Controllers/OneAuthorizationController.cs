namespace Fanex.Athena.Controllers
{
    using System.Text;
    using System.Web.Mvc;
    using Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Logging;
    using Model.Core.DTO.OneAuth;
    using Newtonsoft.Json;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    public class OneAuthorizationController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IOneAuthenticationService _oneAuthenticationService;
        private readonly IAppSettings _appSetting;
        private readonly ILogger _logger;

        public OneAuthorizationController() : this(null, null, null, null)
        {
        }

        public OneAuthorizationController(
            IMaintenanceService maintenanceService,
            IOneAuthenticationService oneAuthenticationService,
            IAppSettings appSetting,
            ILogger logger)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
            _oneAuthenticationService = oneAuthenticationService ?? new OneAuthenticationService();
            _appSetting = appSetting ?? new AppSettings();
            _logger = logger ?? Logger.Log;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Auth/CreateSession")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public RedirectResult CreateSession(string token)
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                return Redirect(Url.Content("~/Utility/UnderMaintenance?notReload=true"));
            }

            var returnUrl = "~/Utility/OneTokenRestriction?code=";
            var errMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(token))
            {
                errMsg = "Token param is empty.";
                returnUrl += OneAuthorizationErrorCode.TokenIsInValid;
            }
            else
            {
                var tokenInfo = _oneAuthenticationService.GetTokenInfo(token);
                if (tokenInfo != null && VerifyToken(tokenInfo))
                {
                    returnUrl = GetReturnPathForValidToken(returnUrl, tokenInfo);
                }
                else
                {
                    errMsg = tokenInfo != null ? JsonConvert.SerializeObject(tokenInfo) : "token is null";
                    returnUrl += OneAuthorizationErrorCode.TokenIsInValid;
                }
            }

            WriteLog(errMsg);

            return Redirect(Url.Content(returnUrl));
        }

        private static bool VerifyToken(OneAuthToken token)
        {
            return token.UserSEQ > 0 &&
                !StringHelper.IsAnyNullOrEmpty(token.SiteName, token.VendorID, token.UserID, token.Language, token.ActionLink);
        }

        private string GetReturnPathForValidToken(string returnPath, OneAuthToken tokenInfo)
        {
            if (IsInternalDomain(tokenInfo.ActionLink))
            {
                return returnPath + OneAuthorizationErrorCode.LocationRestrictionMsg;
            }
            else
            {
                var result = _oneAuthenticationService.SignIn(HttpContext, tokenInfo);
                return result.IsSuccessful ? "~/" + result.ExtendedData : returnPath + result.Code;
            }
        }

        private bool IsInternalDomain(string actionLink)
        {
            var internalSubDomain = _appSetting.GetValue("InternalSubDomain");
            return !string.IsNullOrEmpty(internalSubDomain) && actionLink.Contains(internalSubDomain);
        }

        private void WriteLog(string errMsg)
        {
            if (string.IsNullOrEmpty(errMsg) || _appSetting.GetValue("EnableLogForOneAuthorization") != "1")
            {
                return;
            }

            var message = new StringBuilder();
            message.AppendLine("OneAuthorization Token is invalid - Token = " + errMsg);
            message.AppendLine("Url: " + Request.Url);
            message.AppendLine("UrlReferrer: " + Request.UrlReferrer);
            message.AppendLine("UserAgent: " + Request.UserAgent);
            message.AppendLine("UserHostAddress: " + Request.UserHostAddress);
            message.AppendLine("Browser: ");
            message.AppendLine(" - Platform: " + Request.Browser.Platform);
            message.AppendLine(" - Version: " + Request.Browser.Version);
            message.AppendLine(" - Crawl: " + Request.Browser.Crawler);
            _logger.Error(message.ToString());
        }
    }
}