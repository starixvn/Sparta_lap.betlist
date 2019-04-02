namespace Fanex.Athena.Controllers
{
    using System.Text;
    using System.Web.Mvc;
    using Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.OneAuth;
    using Newtonsoft.Json;
    using ONEbook.UM.Services;
    using Service.Core.Utility.Interfaces;

    public class OneAuthorizationController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IOneAuthenticationService _oneAuthenticationService;
        private readonly IAppSettings _appSetting;
        private readonly ICoreLogger _logger;

        public OneAuthorizationController(
            IMaintenanceService maintenanceService,
            IOneAuthenticationService oneAuthenticationService,
            IAppSettings appSetting,
            ICoreLogger logger)
        {
            _maintenanceService = maintenanceService;
            _oneAuthenticationService = oneAuthenticationService;
            _appSetting = appSetting;
            _logger = logger;
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

            const string ReturnUrl = "~/Utility/OneTokenRestriction?code=";
            if (string.IsNullOrWhiteSpace(token))
            {
                return Redirect(Url.Content(ReturnUrl + OneAuthorizationErrorCode.TokenIsInValid));
            }

            var tokenInfo = _oneAuthenticationService.GetTokenInfo(token);
            if (tokenInfo == null)
            {
                return Redirect(Url.Content(ReturnUrl + OneAuthorizationErrorCode.TokenIsInValid));
            }

            if (VerifyToken(tokenInfo))
            {
                return Redirect(Url.Content(GetReturnPathForValidToken(ReturnUrl, tokenInfo)));
            }

            WriteLog(JsonConvert.SerializeObject(tokenInfo));
            return Redirect(Url.Content(ReturnUrl + OneAuthorizationErrorCode.TokenIsInValid));
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
            message.AppendLine(" - Crawler: " + Request.Browser.Crawler);
            _logger.Error(message.ToString());
        }
    }
}