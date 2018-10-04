namespace Fanex.Athena.Controllers.Report.Result
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Casino;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Result.Core.AllBetCasino;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.GlobalGaming;
    using Fanex.Result.Core.GlobalGaming.DTO;
    using Fanex.Result.Core.MicroGaming;
    using Fanex.Result.Core.MicroGaming.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Result.Core.Voidbridge;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Report.Result;
    using Service.Result.Interfaces;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class ResultReportController : ReportBaseController
    {
        private const string ResultPageGetNormalSportsName = "ResultPage_GetNormalSports";
        private const int ParlayBetType = 9;
        private const int SystemParlayBetType = 29;

        private static readonly IEnumerable<SportType> ShowingMatchCodeSportTypes = new[] 
        {
            SportType.GoldDeluxe,
            SportType.HorseRacing,
            SportType.Greyhound,
            SportType.Harness,
            SportType.VirtualHorseRacing,
            SportType.VirtualGreyhounds,
            SportType.VirtualSpeedway,
            SportType.VirtualMotorRacing,
            SportType.VirtualCycling
        };

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IParserService _microGamingParserService;

        private readonly IResultService _resultService;

        private readonly IRNGCasinoSingleWalletService _rngCasinoSingleWalletService;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private string _htmlResult;
        private MemoryStream _excelStream;

        #region Result-Page

        public ResultReportController() : this(null, null, null, null, null, null, null, null, null, null)
        {
        }

#pragma warning disable S107 // Methods should not have too many parameters

        public ResultReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IParserService microGamingParserService,
            IResultService resultService,
            IRNGCasinoSingleWalletService rngCasinoSingleWalletService,
            ICacheRefAdapter cacheRefAdapter)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(languageService, sessionService, appSetting, clientViewHelper, productService)
        {
            _balanceUpDataAccess = balanceUpDataAccess ?? new BalanceUpDataAccess();
            _microGamingParserService = microGamingParserService ?? ParserService.Instance;
            _resultService = resultService ?? new ResultService();
            _rngCasinoSingleWalletService = rngCasinoSingleWalletService ?? new RngCasinoSingleWalletService();
            _cacheRefAdapter = cacheRefAdapter ?? new CacheRefAdapter();
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
            return View("~/Views/Report/Result/Main.cshtml", model);
        }

        [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public ActionResult Index(ResultMetalModel metalModel)
        {
            if (ModelState.IsValid)
            {
                metalModel.IsExportExcel = false;
                this.GetData(metalModel);
                return this.Content(_htmlResult, "text/html");
            }

            return this.Content(string.Empty, "text/html");
        }

        [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
        [HttpGet]
        public void ResultExcel(ResultMetalModel metalModel)
        {
            const string FileName = "LRF-Result.xls";
            var response = ControllerContext.HttpContext.Response;

            if (ModelState.IsValid)
            {
                metalModel.IsExportExcel = true;
                metalModel.SportId = metalModel.SportId == 0 ?
                    GetSportId(metalModel.FromDate, SessionService.LicenseeSession.LicenseeId) :
                    metalModel.SportId;

                if (metalModel.SportId > 0)
                {
                    GetData(metalModel);
                }

                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-disposition", "attachment; filename=" + FileName);

                if (_excelStream == null)
                {
                    _excelStream = ExportNoneData(metalModel);
                }

                _excelStream.WriteTo(response.OutputStream);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.StatusDescription = ViewResources.InvalidParams;
            }

            response.Flush();
        }

        [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult GetSports(ResultMetalModel metalModel)
        {
            if (ModelState.IsValid)
            {
                if (metalModel.IsOutRight)
                {
                    return GetOutRightSports(metalModel);
                }

                if (metalModel.SportId == 0)
                {
                    return GetNormalSports(metalModel);
                }
                else
                {
                    return Json(new { Leagues = _resultService.GetLeagues(metalModel.FromDate, metalModel.SportId) });
                }
            }

            return Json(string.Empty);
        }

        #endregion Result-Page

        #region PopUpPage

        [HttpGet]
        public ActionResult ResultPopup(ResultPopupMetalModel metalModel)
        {
            if (ModelState.IsValid)
            {
                GetPopUpData(metalModel);
                ViewBag.Data = _htmlResult;
                return View("~/Views/Report/Result/Popup/Main.cshtml");
            }

            return Redirect("~/Utility/Error?err=400");
        }

        [HttpGet]
        public ActionResult MicrogamingResultPopup(int gameId, int custId)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Data = _microGamingParserService.Parser(new MicroGaming
                {
                    CustId = custId,
                    SiteName = SessionService.LicenseeSession.LicenseeName,
                    GameId = gameId
                });
                return View("~/Views/Report/Result/Popup/Microgaming.cshtml");
            }

            return Redirect("~/Utility/Error?err=400");
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult ShowRngCasinoSingleWalletResult(string refNo)
        {
            var resultInfo = _rngCasinoSingleWalletService.GetResultInfo(refNo);

            return Json(resultInfo);
        }

        [HttpGet]
        public string VoidbridgeResult(string custId, string gamePlayId)
        {
            var voidbridgeApi = new VoidbridgeApiImp();
            var languageCode = SessionService.UserSession.Language;
            return voidbridgeApi.GetBetHistoryUrl(custId, gamePlayId, languageCode);
        }

        [HttpGet]
        public ActionResult AllBetResult(string transDesc, string betCheck, string betType)
        {
            var allbetResultService = new AllbetResultBuilder();
            ViewBag.Data = allbetResultService.Render(transDesc, betCheck, betType, SessionService.UserSession.Language);
            return View("~/Views/Report/Result/Popup/AllBet.cshtml");
        }

        [HttpGet]
        public string GlobalGamingResult(string custId, string gameId)
        {
            var globalGamingResultApi = new GlobalGamingResultApi();

            var criteria = new GlobalGamingDTO
            {
                CustId = custId,
                GameId = gameId,
            };

            return globalGamingResultApi.GetResultUrl(criteria);
        }

        /// <summary>
        /// Generate tennis detailed result.
        /// </summary>
        /// <param name="matchId">The match ID.</param>
        /// /// <param name="sportType">The sport type.</param>
        /// <returns>The html result.</returns>
        [HttpGet]
        public ActionResult ResultDetail(int matchId, int sportType)
        {
            var detailedData = _resultService.GetSportsbookGameDetail(matchId);
            if (detailedData != null && detailedData.MatchId > 0)
            {
                ViewBag.Data = _resultService.HtmlGameDetailParser(
                detailedData,
                new ResultSetting
                {
                    SportType = (SportType)sportType,
                    LanguageCode = SessionService.UserSession.Language
                });

                return View("~/Views/Report/Result/Popup/Main.cshtml");
            }

            return this.Redirect("~/Utility/Error?err=400");
        }

        #endregion PopUpPage

        #region Utilities

        #region popup-page

        private static bool IsPalayBetType(int betType)
        {
            return betType == ParlayBetType || betType == SystemParlayBetType || betType == (int)Constant.DataDomain.BetType.VirtualSports2MixParlay;
        }

        private static bool IsShowMatchCodeIDColumn(SportType sportType)
        {
#pragma warning disable S1067 // Expressions should not be too complex
            return sportType == SportType.GoldDeluxe
                || sportType == SportType.HorseRacing
                || sportType == SportType.Greyhound
                || sportType == SportType.Harness
                || sportType == SportType.VirtualHorseRacing
                || sportType == SportType.VirtualGreyhounds
                || sportType == SportType.VirtualSpeedway
                || sportType == SportType.VirtualMotorRacing
                || sportType == SportType.VirtualCycling;
#pragma warning restore S1067 // Expressions should not be too complex
        }

        private static IEnumerable<Sport> RemoveNotAllowedSports(IEnumerable<Sport> sports, ProductAccess productAccess)
        {
            var limitedSports = sports;
            if (!productAccess.IsAllowNG)
            {
                limitedSports = limitedSports.Where(i => i.SportId != (int)SportType.NumberGame);
            }

            if (!productAccess.IsAllowRB)
            {
                limitedSports = limitedSports.Where(i => i.SportId != (int)SportType.HorseRacing
                && i.SportId != (int)SportType.Greyhound && i.SportId != (int)SportType.Harness);
            }

            if (!productAccess.IsAllowVS)
            {
                limitedSports = limitedSports.Where(i => i.SportId != (int)SportType.VirtualSports);
            }

            return limitedSports;
        }

        /// <summary>
        /// Get data for pop-up page.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        private void GetPopUpData(ResultPopupMetalModel metalModel)
        {
            var sportType = (SportType)metalModel.SportType;
            var setting = new ResultSetting
            {
                SportType = sportType,
                IsOutright = metalModel.IsOutRight.HasValue && metalModel.IsOutRight.Value > 0,
                IsParlayPopUp = IsPalayBetType(metalModel.Bettype),
                IsPopUp = true,
                LanguageCode = SessionService.UserSession.Language,
                EnableCheckScoreCalculated = true
            };

            if (setting.IsParlayPopUp)
            {
                if (metalModel.RefNo != null)
                {
                    var parlayData = _resultService.GetPopUpParlayMatch(metalModel.RefNo.Value);
                    _htmlResult = _resultService.HtmlParser(parlayData, setting);
                }
            }
            else
            {
                GetNormalPopUpData(metalModel, sportType, setting);
            }
        }

#pragma warning disable S1541 // Methods and properties should not be too complex

        private void GetNormalPopUpData(ResultPopupMetalModel metalModel, SportType sportType, ResultSetting setting)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            switch (sportType)
            {
                case SportType.HorseRacing:
                case SportType.Greyhound:
                case SportType.Harness:
                case SportType.VirtualHorseRacing:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualSpeedway:
                case SportType.VirtualCycling:
                    if (metalModel.RaceNo != null)
                    {
                        var dataRc = _resultService.GetPopUpRacingMatch(metalModel);
                        if (dataRc != null)
                        {
                            this._htmlResult = _resultService.HtmlParser(dataRc, setting);
                        }
                    }

                    break;

                case SportType.NumberGame:
                    var dataNg = _resultService.GetPopUpNumberGameMatchs(metalModel.MatchId);
                    if (dataNg != null)
                    {
                        _htmlResult = _resultService.HtmlParser(dataNg, setting);
                    }

                    break;

                default:
                    var data = _resultService.GetPopUpMatch(metalModel);
                    if (data != null)
                    {
                        _htmlResult = _resultService.HtmlParser(data, setting);
                    }

                    break;
            }
        }

        #endregion popup-page

        #region spots list

        private ActionResult GetNormalSports(ResultMetalModel metalModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var sports = _resultService.GetSports(metalModel.FromDate, licenseeSession.LicenseeId);
            if (sports.Any())
            {
                var productAccess = ProductService.GetProductAccess(
                    licenseeSession.LicenseeId,
                    SessionService.UserSession.Language,
                    ResultPageGetNormalSportsName);
                var isAllowAllProducts =
                    productAccess.IsAllowRB && productAccess.IsAllowNG && productAccess.IsAllowVS;

                var limitedSports = isAllowAllProducts ? sports : RemoveNotAllowedSports(sports, productAccess);

                if (limitedSports.Any())
                {
                    var leagues = _resultService.GetLeagues(metalModel.FromDate, limitedSports.First().SportId);
                    return Json(new { Sports = limitedSports, Leagues = leagues });
                }
            }

            return Json(string.Empty);
        }

        private ActionResult GetOutRightSports(ResultMetalModel metalModel)
        {
            var cacheKey = GenerateOutRightCacheKey(metalModel);
            var outRightData = (IList<Outright>)HttpRuntime.Cache.Get(cacheKey) ?? _resultService.GetOutRight(metalModel);
            if (outRightData != null && outRightData.Count > 0)
            {
                var sports = outRightData.Where(i => i.SportType == metalModel.SportId || metalModel.SportId == 0)
                        .Select(i => i.SportType).Distinct()
                        .Select(i => new Sport
                        {
                            SportId = i,
                            SportName = _cacheRefAdapter.GetSportName(i)
                        }).ToList();

                if (sports.Count > 0)
                {
                    var selectedSport = metalModel.SportId > 0 ? metalModel.SportId : sports[0].SportId;
                    var leagues = outRightData.Where(i => i.SportType == selectedSport)
                            .Select(i => i.LeagueId).Distinct()
                            .Select(i => new League
                            {
                                LeagueId = i,
                                LeagueName = _cacheRefAdapter.GetLeagueName(i)
                            }).ToList();

                    return Json(new { Sports = sports, Leagues = leagues });
                }
            }

            return Json(string.Empty);
        }

        #endregion spots list

        #region main-page

        private int GetSportId(DateTime fromDate, int licenseeId)
        {
            var sports = _resultService.GetSports(fromDate, licenseeId);
            return sports.Any() ? sports[0].SportId : 0;
        }

        private void GetOutRightData(ResultMetalModel metalModel)
        {
            var setting = GetOutRightResultSetting();
            var cacheKey = GenerateOutRightCacheKey(metalModel);
            var outRightData = (IList<Outright>)HttpRuntime.Cache.Get(cacheKey) ?? _resultService.GetOutRight(metalModel);
            if (outRightData != null)
            {
                var data =
                    outRightData.Where(i => i.SportType == metalModel.SportId && (metalModel.LeagueId == 0 || metalModel.LeagueId == i.LeagueId)).ToList();

                if (data.Count > 0)
                {
                    if (metalModel.IsExportExcel)
                    {
                        this._excelStream = _resultService.ExcelParser(data, setting);
                    }
                    else
                    {
                        this._htmlResult = _resultService.HtmlParser(data, setting);
                    }
                }
            }
        }

#pragma warning disable S1541 // Methods and properties should not be too complex

        private void GetNormalData(ResultMetalModel metalModel)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            var setting = GetNormalResultSetting(metalModel);
            switch (setting.SportType)
            {
                case SportType.HorseRacing:
                case SportType.Greyhound:
                case SportType.Harness:
                case SportType.VirtualHorseRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualCycling:
                case SportType.VirtualSpeedway:
                    var dataRc = _resultService.GetRacingMatches(metalModel);
                    ParseResult(dataRc, setting, metalModel.IsExportExcel);
                    break;

                case SportType.NumberGame:
                    var dataNg = _resultService.GetNumberGameMatches(metalModel);
                    ParseResult(dataNg, setting, metalModel.IsExportExcel);
                    break;

                default:
                    var data = _resultService.GetMatches(metalModel);
                    ParseResult(data, setting, metalModel.IsExportExcel);
                    break;
            }
        }

        private void ParseResult<T>(IList<T> data, ResultSetting setting, bool isExportExcel)
        {
            if (data != null)
            {
                if (isExportExcel)
                {
                    _excelStream = _resultService.ExcelParser(data, setting);
                }
                else
                {
                    _htmlResult = _resultService.HtmlParser(data, setting);
                }
            }
        }

        private MemoryStream ExportNoneData(ResultMetalModel metalModel)
        {
            if (metalModel.IsOutRight)
            {
                var setting = GetOutRightResultSetting();
                return _resultService.ExcelParser(new List<Outright>(), setting);
            }
            else
            {
                metalModel.SportId = (int)SportType.Soccer;
                var setting = GetNormalResultSetting(metalModel);
                return _resultService.ExcelParser(new List<Sportsbook>(), setting);
            }
        }

        private ResultSetting GetOutRightResultSetting()
        {
            return new ResultSetting
            {
                IsOutright = true,
                ExcelTitle = ViewResources.Result,
                ExcelTemplatePath = HostingEnvironment.MapPath(@"~/App_Data/Fanex.Result/"),
                LanguageCode = SessionService.UserSession.Language,
                VisibleMatchCodeIDColumn = true
            };
        }

        private ResultSetting GetNormalResultSetting(ResultMetalModel metalModel)
        {
            var sportType = (SportType)metalModel.SportId;
            return new ResultSetting
            {
                SportType = sportType,
                ExcelTitle = ViewResources.Result,
                ExcelTemplatePath = HostingEnvironment.MapPath(@"~/App_Data/Fanex.Result/"),
                LanguageCode = SessionService.UserSession.Language,
                VisibleDetailedLink = true,
                VisibleFreeHeader = true,
                EnableCheckScoreCalculated = sportType == SportType.Soccer,
                VisibleMatchCodeIDColumn = ShowingMatchCodeSportTypes.Contains(sportType)
            };
        }

        /// <summary>
        /// Get data from service layer.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        private void GetData(ResultMetalModel metalModel)
        {
            if (metalModel.IsOutRight)
            {
                this.GetOutRightData(metalModel);
            }
            else
            {
                this.GetNormalData(metalModel);
            }
        }

        #endregion main-page

        private string GenerateOutRightCacheKey(ResultMetalModel metalModel)
        {
            const string PatternKey = "{0}{1}Result.OutRight.Data_{2}";
            return string.Format(PatternKey, metalModel.FromDate, metalModel.ToDate, SessionService.UserSession.UserId);
        }

        #endregion Utilities
    }
}