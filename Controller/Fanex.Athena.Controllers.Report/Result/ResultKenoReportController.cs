namespace Fanex.Athena.Controllers.Report.Result
{
    using System;
    using System.Web.Mvc;
    using Logging;
    using Model.Core.DataDomain;
    using Model.Report.MetaModel.Keno;
    using Service.Report.Result;
    using Service.Report.Result.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
    public class ResultKenoReportController : ReportBaseController
    {
        private readonly IKenoApi _kenoApi;

        public ResultKenoReportController() : this(null)
        {
        }

        public ResultKenoReportController(IKenoApi kenoApi)
        {
            _kenoApi = kenoApi ?? new KenoApi();
        }

        [HttpGet]
        [AjaxRequest]
        public ActionResult Index(int vendorProductId)
        {
            var userId = SessionService.UserSession.UserId;
            return Json(
                new
                {
                    Url = _kenoApi.CreateKenoResultUrl(GetKenoSetting(vendorProductId), userId.ToString(), DateTime.Now),
                    UserId = userId
                },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public string KeepSession(string userId, int vendorProductId)
        {
            if (string.IsNullOrEmpty(userId) || !SessionService.UserSession.UserId.ToString().Equals(userId))
            {
                LogService.Error("Keno Result page just submited invalid userId");

                return Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme);
            }

            var result = _kenoApi.ExtendToken(GetKenoSetting(vendorProductId), userId);

            if (result.ErrorCode > 0)
            {
                LogService.Error(result.ErrorMsg);
                return Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme);
            }

            return string.Empty;
        }

        private KenoResultMetaModel GetKenoSetting(int vendorProductId)
        {
            return new KenoResultMetaModel
            {
                Language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                SiteID = AppSettings.GetValue("KenoSiteID"),
                AuthorizedUrl = AppSettings.GetValue("KenoAuthorizedUrl"),
                ExpireTokenUrl = AppSettings.GetValue("KenoExpireTokenUrl"),
                ExtendTokenUrl = AppSettings.GetValue("KenoExtendTokenUrl"),
                VenderKey = AppSettings.GetValue("KenoVenderKey"),
                Vendor = AppSettings.GetValue("KenoVendor"),
                ResultUrl = vendorProductId == ResultVendorProductTypes.Keno ? AppSettings.GetValue("KenoResultUrl") : AppSettings.GetValue("LotteryResultUrl"),
                KickOutUrl = Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme),
                VendorProduct = vendorProductId
            };
        }
    }
}