namespace Fanex.Athena.Controllers.Report.Result
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Athena.Result.Model.DTO.Keno;
    using Athena.Result.Model.MetaModel.Keno;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportResults)]
    public class ResultKenoReportController : ReportBaseController
    {
        private readonly ICoreLogger _logger;

        private readonly IBridgeClientService _bridgeClientService;

        public ResultKenoReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            ICoreLogger logger,
            IBridgeClientService bridgeClientService)
            : base(languageService, sessionService, appSetting, productService)
        {
            _logger = logger;
            _bridgeClientService = bridgeClientService;
        }

        [HttpGet]
        [AjaxRequest]
        public async Task<ActionResult> Index(int vendorProductId)
        {
            var userId = SessionService.UserSession.UserId;
            var model = new KenoMetaModel
            {
                VendorProductId = vendorProductId,
                UserId = userId.ToString()
            };
            var url = new Uri($"http://localhost:55050/ResultKenoReport/GetKenoResultUrl");
            var kenoUrl = await _bridgeClientService.PostAsyncString(url, model).ConfigureAwait(false);

            return Json(
                new
                {
                    Url = kenoUrl,
                    UserId = userId
                },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public async Task<string> KeepSession(string userId, int vendorProductId)
        {
            if (string.IsNullOrEmpty(userId) || !SessionService.UserSession.UserId.ToString().Equals(userId))
            {
                _logger.Error("Keno Result page just submited invalid userId");

                return Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme);
            }

            var model = new KenoMetaModel
            {
                VendorProductId = vendorProductId,
                UserId = userId
            };

            var url = new Uri($"http://localhost:55050/ResultKenoReport/KeepSession");
            var result = await _bridgeClientService.PostAsync<KenoExtendedToken>(url, model).ConfigureAwait(false);
            
            if (result.ErrorCode > 0)
            {
                _logger.Error("Keno Session: " + result.ErrorMsg);
                return Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme);
            }

            return string.Empty;
        }
    }
}