namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Logging;
    using Model.Report.MetaModel.Keno;
    using Service.Report.Keno;
    using Service.Report.Keno.Interfaces;
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
        public ActionResult Index()
        {
            var userId = SessionService.UserSession.UserId;
            return Json(
                new
                {
                    Url = _kenoApi.CreateKenoResultUrl(GetKenoSetting(), userId.ToString(), DateTime.Now),
                    UserId = userId
                },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public string KeepSession(string userId)
        {
            if (string.IsNullOrEmpty(userId) || !SessionService.UserSession.UserId.ToString().Equals(userId))
            {
                Fanex.Logging.LogService.Error("Keno Result page just submited invalid userId");

                return Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme);
            }

            var result = _kenoApi.ExtendToken(GetKenoSetting(), userId);

            if (result.ErrorCode > 0)
            {
                LogService.Error(result.ErrorMsg);
                return Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme);
            }

            return string.Empty;
        }

        private KenoResultMetalModel GetKenoSetting()
        {
            return new KenoResultMetalModel
            {
                Language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                SiteID = AppSettings.GetValue("KenoSiteID"),
                AuthorizedUrl = AppSettings.GetValue("KenoAuthorizedUrl"),
                ExpireTokenUrl = AppSettings.GetValue("KenoExpireTokenUrl"),
                ExtendTokenUrl = AppSettings.GetValue("KenoExtendTokenUrl"),
                VenderKey = AppSettings.GetValue("KenoVenderKey"),
                Vendor = AppSettings.GetValue("KenoVendor"),
                ResultUrl = AppSettings.GetValue("KenoResultUrl"),
                KickOutUrl = Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme)
            };
        }
    }
}