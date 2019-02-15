namespace Fanex.Athena.Result.Controller
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Common.Utility.Attributes;
    using Common.Utility.Services.Interfaces;
    using Logging;
    using Model.Const;
    using Model.DTO.Keno;
    using Model.MetaModel.Keno;
    using Service.Interfaces;

    [CustomAuth]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ResultKenoReportController : Controller
    {
        private readonly ILogger _logger;

        private readonly IAppSettings _appSettings;

        private readonly IKenoApi _kenoApi;

        public ResultKenoReportController(ILogger logger, IAppSettings appSetting, IKenoApi kenoApi)
        {
            _logger = logger;
            _appSettings = appSetting;
            _kenoApi = kenoApi;
        }

        [HttpPost]
        public string GetKenoResultUrl(KenoMetaModel model)
        {
            try
            {
                return _kenoApi.CreateKenoResultUrl(GetKenoSetting(model.VendorProductId), model.UserId.ToString(), DateTime.Now);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message, exception);

                return string.Empty;
            }
        }

        [HttpPost]
        public JsonResult KeepSession(KenoMetaModel model)
        {
            try
            {
                var result = _kenoApi.ExtendToken(GetKenoSetting(model.VendorProductId), model.UserId);
                return Json(result);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message, exception);

                return Json(new KenoExtendedToken
                {
                    ErrorCode = 1,
                    ErrorMsg = exception.Message
                });
            }
        }

        private KenoResultMetaModel GetKenoSetting(int vendorProductId)
        {
            return new KenoResultMetaModel
            {
                Language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                SiteID = _appSettings.GetValue("KenoSiteID"),
                AuthorizedUrl = _appSettings.GetValue("KenoAuthorizedUrl"),
                ExpireTokenUrl = _appSettings.GetValue("KenoExpireTokenUrl"),
                ExtendTokenUrl = _appSettings.GetValue("KenoExtendTokenUrl"),
                VenderKey = _appSettings.GetValue("KenoVenderKey"),
                Vendor = _appSettings.GetValue("KenoVendor"),
                ResultUrl = vendorProductId == ResultVendorProductTypes.Keno ? _appSettings.GetValue("KenoResultUrl") : _appSettings.GetValue("LotteryResultUrl"),
                KickOutUrl = Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme),
                VendorProduct = vendorProductId
            };
        }
    }
}