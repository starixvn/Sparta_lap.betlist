namespace Fanex.Athena.Controllers.Report.Result
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Athena.Result.Model.MetaModel;
    using Athena.Result.Model.ViewModel;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Result.Core.MicroGaming.DTO;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    public class ResultReportController : ReportBaseController
    {
        private const string ResultPageGetNormalSportsName = "ResultPage_GetNormalSports";

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IBridgeClientService _bridgeClientService;

        #region Result-Page

        public ResultReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IBridgeClientService bridgeClientService)
            : base(languageService, sessionService, appSetting, productService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _bridgeClientService = bridgeClientService;
        }

        [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ResultReportViewModel
            {
                Data = string.Empty,
                Param = new ResultMetalModel
                {
                    FromDate = DateTime.Now,
                    IsOutRight = false,
                    IsExportExcel = false,
                    ToDate = DateTime.Now,
                    BalanceUpDay = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay
                }
            };
            var productAccess = ProductService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            ViewBag.IsAllowedRNG = productAccess.IsAllowCS;
            ViewBag.IsAllowedKeno = productAccess.IsAllowKN;
            ViewBag.IsAllowedLottery = productAccess.IsAllow(Product.LT);
            return View("~/Views/Desktop/Report/Result/Main.cshtml", model);
        }

        [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(ResultMetalModel metaModel)
        {
            if (ModelState.IsValid)
            {
                var url = new Uri($"http://localhost:55050/ResultReport/Index");
                var data = await _bridgeClientService.PostAsyncString(url, metaModel).ConfigureAwait(false);

                return this.Content(data, "text/html");
            }

            return this.Content(string.Empty, "text/html");
        }

        [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
        [HttpGet]
        public ActionResult ResultExcel(ResultMetalModel metaModel)
        {
            const string FileName = "LRF-Result.xls";
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            var url = new Uri($"http://localhost:55050/ResultReport/ResultExcel");
            var stream = _bridgeClientService.PostAsyncStream(url, metaModel).Result;
            return File(stream, "application/vnd.ms-excel", FileName + ".xls");
        }

        [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> GetSports(GetSportResultMetalModel metaModel)
        {
            if (ModelState.IsValid)
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

                var productAccess = ProductService.GetProductAccess(
                   metaModel.LicenseeId,
                   SessionService.UserSession.Language,
                   ResultPageGetNormalSportsName);
                metaModel.ProductCodeList = string.Join(",", productAccess.ProductList.Select(x => x.ProductCode));
                var url = new Uri($"http://localhost:55050/ResultReport/GetSports");
                var result = await _bridgeClientService.PostAsyncString(url, metaModel).ConfigureAwait(false);

                return this.Content(result, "application/json");
            }

            return Json(string.Empty);
        }

        #endregion Result-Page

        #region PopUpPage

        [HttpGet]
        public async Task<ActionResult> ResultPopup(ResultPopupMetalModel metaModel)
        {
            if (ModelState.IsValid)
            {
                var url = new Uri($"http://localhost:55050/ResultReport/ResultPopup");
                var result = await _bridgeClientService.PostAsyncString(url, metaModel).ConfigureAwait(false);

                ViewBag.Data = result;
                return View("~/Views/Desktop/Report/Result/Popup/Main.cshtml");
            }

            return Redirect("~/Utility/Error?err=400");
        }

        [HttpGet]
        public async Task<ActionResult> MicrogamingResultPopup(int gameId, int custId)
        {
            if (ModelState.IsValid)
            {
                var metaModel = new MicroGaming
                {
                    CustId = custId,
                    SiteName = SessionService.LicenseeSession.LicenseeName,
                    GameId = gameId
                };
                var url = new Uri($"http://localhost:55050/ResultReport/MicrogamingResultPopup");
                var result = await _bridgeClientService.PostAsyncString(url, metaModel).ConfigureAwait(false);

                ViewBag.Data = result;
                return View("~/Views/Desktop/Report/Result/Popup/Microgaming.cshtml");
            }

            return Redirect("~/Utility/Error?err=400");
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> ShowRngCasinoSingleWalletResult(string refNo)
        {
            var url = new Uri($"http://localhost:55050/ResultReport/ShowRngCasinoSingleWalletResult");
            var result = await _bridgeClientService.PostAsyncString(url, new
            {
                refNo
            }).ConfigureAwait(false);

            return this.Content(result, "application/json");
        }

        [HttpGet]
        public async Task<string> VoidbridgeResult(string custId, string gamePlayId)
        {
            var url = new Uri($"http://localhost:55050/ResultReport/VoidbridgeResult");
            var model = new
            {
                custId,
                gamePlayId,
                languageCode = SessionService.UserSession.Language
            };
            return await _bridgeClientService.PostAsyncString(url, model).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ActionResult> AllBetResult(string transDesc, string betCheck, string betType)
        {
            var model = new
            {
                transDesc,
                betCheck,
                betType,
                languageCode = SessionService.UserSession.Language
            };
            var url = new Uri($"http://localhost:55050/ResultReport/AllBetResult");
            ViewBag.Data = await _bridgeClientService.PostAsyncString(url, model).ConfigureAwait(false);
            return View("~/Views/Desktop/Report/Result/Popup/AllBet.cshtml");
        }

        [HttpGet]
        public async Task<string> GlobalGamingResult(string custId, string gameId)
        {
            var model = new
            {
                custId,
                gameId,
            };
            var url = new Uri($"http://localhost:55050/ResultReport/GlobalGamingResult");
            return await _bridgeClientService.PostAsyncString(url, model).ConfigureAwait(false);
        }

        /// <summary>
        /// Generate tennis detailed result.
        /// </summary>
        /// <param name="matchId">The match ID.</param>
        /// /// <param name="sportType">The sport type.</param>
        /// <returns>The html result.</returns>
        [HttpGet]
        public async Task<ActionResult> ResultDetail(int matchId, int sportType)
        {
            var model = new
            {
                matchId,
                sportType,
                languageCode = SessionService.UserSession.Language
            };
            var url = new Uri($"http://localhost:55050/ResultReport/ResultDetail");
            var result = await _bridgeClientService.PostAsyncString(url, model).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(result))
            {
                ViewBag.Data = result;

                return View("~/Views/Desktop/Report/Result/Popup/Main.cshtml");
            }

            return this.Redirect("~/Utility/Error?err=400");
        }

        #endregion PopUpPage
    }
}