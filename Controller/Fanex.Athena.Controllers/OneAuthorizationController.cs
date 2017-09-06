namespace Fanex.Athena.Controllers
{
    using System.Text;
    using System.Web.Mvc;
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

        internal OneAuthorizationController(
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
                    if (IsInternalDomain(tokenInfo.ActionLink))
                    {
                        returnUrl += OneAuthorizationErrorCode.LocationRestrictionMsg;
                    }
                    else
                    {
                        var result = _oneAuthenticationService.SignIn(HttpContext, tokenInfo);
                        returnUrl = result.IsSuccessful ? "~/" + result.ExtendedData : returnUrl + result.Code;
                    }
                }
                else
                {
                    errMsg = tokenInfo != null ? JsonConvert.SerializeObject(tokenInfo) : "token is null";
                    returnUrl += OneAuthorizationErrorCode.TokenIsInValid;
                }
            }

            if (_appSetting.GetValue("EnableLogForOneAuthorization") == "1" && !string.IsNullOrEmpty(errMsg))
            {
                var message = new StringBuilder();
                message.AppendLine("OneAuthorization Token is invalid - Token = " + errMsg);
                message.AppendLine("Url: " + Request.Url);
                message.AppendLine("UrlReferrer: " + Request.UrlReferrer);
                _logger.Error(message.ToString());
            }

            return Redirect(Url.Content(returnUrl));
        }

        private static bool VerifyToken(OneAuthToken tokenInfo)
        {
            return !string.IsNullOrEmpty(tokenInfo.SiteName) && tokenInfo.UserSEQ > 0
                && !string.IsNullOrEmpty(tokenInfo.VendorID) && !string.IsNullOrEmpty(tokenInfo.UserID)
                && !string.IsNullOrEmpty(tokenInfo.Language) && !string.IsNullOrEmpty(tokenInfo.ActionLink);
        }

        private bool IsInternalDomain(string actionLink)
        {
            var internalSubDomain = _appSetting.GetValue("InternalSubDomain");
            return !string.IsNullOrEmpty(internalSubDomain) && actionLink.Contains(internalSubDomain);
        }
    }
}