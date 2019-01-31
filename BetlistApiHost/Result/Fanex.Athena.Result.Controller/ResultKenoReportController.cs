namespace Fanex.Athena.Result.Controller
{
    using System;
    using System.Web.Mvc;
    using Extension.Utility;
    using Logging;
    using Model.Const;
    using Model.MetaModel.Keno;
    using Service.Interfaces;

    public class ResultKenoReportController : Controller
    {
        private readonly IAppSettings _appSettings;

        private readonly IKenoApi _kenoApi;

        public ResultKenoReportController(IAppSettings appSetting, IKenoApi kenoApi)
        {
            _appSettings = appSetting;
            _kenoApi = kenoApi;
        }

        [HttpPost]
        public ActionResult Index(int userId, int vendorProductId)
        {
            return Json(
                new
                {
                    Url = _kenoApi.CreateKenoResultUrl(GetKenoSetting(vendorProductId), userId.ToString(), DateTime.Now),
                    UserId = userId
                });
        }

        [HttpPost]
        public JsonResult KeepSession(string userId, int vendorProductId)
        {
            return Json(_kenoApi.ExtendToken(GetKenoSetting(vendorProductId), userId));
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