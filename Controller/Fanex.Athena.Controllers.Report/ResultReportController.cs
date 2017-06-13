// -----------------------------------------------------------------------
// <copyright file="ReportController.ResultPage.cs" company="">
// ReportController copyright.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.MetaModel.Keno;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Casino;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Fanex.Athena.Service.Report.Keno;
    using Fanex.Athena.Service.Report.Keno.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.MicroGaming;
    using Fanex.Result.Core.MicroGaming.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Logging;
    using Resources;
    using Result.Core.AllBetCasino;
    using Result.Core.GlobalGaming;
    using Result.Core.GlobalGaming.DTO;
    using Result.Core.Voidbridge;

    /// <summary>
    /// The result controller.
    /// </summary>
    [FunctionAuthorization(Model.Core.DataDomain.PermissionKeys.ReportResults)]
    public class ResultReportController : ReportBaseController
    {
        private const string ResultPageGetNormalSportsName = "ResultPage_GetNormalSports";
        private const int ParlayBetType = 9;
        private const int SystemParlayBetType = 29;
        private readonly IResultService _resultService;
        private readonly IRNGCasinoSingleWalletService _rngCasinoSingleWalletService;
        private readonly IKenoApi _kenoApi;

        private string _htmlResult;
        private MemoryStream _excelStream;

        #region Result-Page

        public ResultReportController() : this(null, null, null)
        {
        }

        public ResultReportController(
            IResultService resultService,
            IRNGCasinoSingleWalletService rngCasinoSingleWalletService,
            IKenoApi kenoApi)
        {
            _resultService = resultService ?? new ResultService();
            _rngCasinoSingleWalletService = rngCasinoSingleWalletService ?? new RngCasinoSingleWalletService();
            _kenoApi = kenoApi ?? new KenoApi();
        }

        /// <summary>
        /// The result page.
        /// </summary>
        /// <returns>The action result.</returns>
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
                    BalanceUpDay = this.BalanceUpService.BalanceUpInfo.BalanceUpDay
                }
            };
            var productAccess = ProductService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            ViewBag.IsAllowedRNG = productAccess.IsAllowBG || productAccess.IsAllowCS;
            ViewBag.IsAllowedKeno = productAccess.IsAllowKN;
            return View("~/Views/Report/Result/Main.cshtml", model);
        }

        /// <summary>
        /// The result report.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The actin result.</returns>
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

        /// <summary>
        /// The result excel report.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ResultExcel(ResultMetalModel metalModel)
        {
            const string FileName = "LRF-Result.xls";
            if (ModelState.IsValid)
            {
                metalModel.IsExportExcel = true;
                if (metalModel.SportId == 0)
                {
                    /*Get firstSport*/
                    var sports = _resultService.GetSports(metalModel.FromDate, SessionService.LicenseeSession.LicenseeId);
                    if (sports == null || sports.Count == 0)
                    {
                        return null;
                    }

                    metalModel.SportId = int.Parse(sports[0].SportId);
                }

                this.GetData(metalModel);
                var response = ControllerContext.HttpContext.Response;
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-disposition", "attachment; filename=" + FileName);
                if (_excelStream != null)
                {
                    _excelStream.WriteTo(response.OutputStream);
                }

                response.Flush();
            }

            return null;
        }

        /// <summary>
        /// Get sports.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult GetSports(ResultMetalModel metalModel)
        {
            if (ModelState.IsValid)
            {
                IList<Sport> sports;
                IList<League> leagues;
                if (metalModel.IsOutRight)
                {
                    return GetOutRightSports(metalModel, out sports, out leagues);
                }

                if (metalModel.SportId == 0)
                {
                    return GetNormalSports(metalModel, out sports, out leagues);
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

        /// <summary>
        /// The pop up result page.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ResultPopup(ResultPopupMetalModel metalModel)
        {
            if (ModelState.IsValid)
            {
                GetPopUpData(metalModel);
                ViewBag.Data = _htmlResult;
                return View("~/Views/Report/Result/Popup/Main.cshtml");
            }

            return this.Redirect(Util.RootPath + "Utility/Error?err=400");
        }

        [HttpGet]
        public ActionResult MicrogamingResultPopup(int gameId, int custId)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Data = ParserService.Instance.Parser(new MicroGaming
                {
                    CustId = custId,
                    SiteName = SessionService.LicenseeSession.LicenseeName,
                    GameId = gameId
                });
                return View("~/Views/Report/Result/Popup/Microgaming.cshtml");
            }

            return this.Redirect(Util.RootPath + "Utility/Error?err=400");
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
                ViewBag.Data = ResultEngine.HtmlGameDetailParser(
                detailedData,
                new ResultSetting
                {
                    SportType = (SportType)sportType,
                    LanguageCode = SessionService.UserSession.Language
                });

                return View("~/Views/Report/Result/Popup/Main.cshtml");
            }

            return this.Redirect(Util.RootPath + "Utility/Error?err=400");
        }

        #endregion PopUpPage

        #region KenoPage

        /// <summary>
        /// Keno result action.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [AjaxRequest]
        public ActionResult KenoResult()
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

        /// <summary>
        /// Extend Keno result session.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The result.</returns>
        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public string KenoResult(string userId)
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

        #endregion KenoPage

        #region Utilities

        #region popup-page

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
                IsParlayPopUp = metalModel.Bettype == ParlayBetType || metalModel.Bettype == SystemParlayBetType,
                IsPopUp = true,
                LanguageCode = SessionService.UserSession.Language,
                EnableCheckScoreCalculated = true
            };

            if (setting.IsParlayPopUp)
            {
                if (metalModel.RefNo != null)
                {
                    var parlayData = _resultService.GetPopUpParlayMatch(metalModel.RefNo.Value);
                    _htmlResult = ResultEngine.HtmlParser(parlayData, setting);
                }
            }
            else
            {
                GetNormalPopUpData(metalModel, sportType, setting);
            }
        }

        /// <summary>
        /// Get normal pop up data.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <param name="sportType">The sport type.</param>
        /// <param name="setting">The setting result.</param>
        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void GetNormalPopUpData(ResultPopupMetalModel metalModel, SportType sportType, ResultSetting setting)
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
                        var dataRc = _resultService.GetPopUpRacingMatch(
                            metalModel.MatchId,
                            metalModel.RaceNo.Value,
                            metalModel.Bettype,
                            metalModel.BetId);
                        if (dataRc != null)
                        {
                            this._htmlResult = ResultEngine.HtmlParser(dataRc, setting);
                        }
                    }

                    break;

                case SportType.LiveCasino:

                    var dataLc = _resultService.GetPopUpLiveCasinoMatch(
                        metalModel.MatchId,
                        metalModel.Bettype,
                        metalModel.BetId);
                    if (dataLc != null)
                    {
                        this._htmlResult = ResultEngine.HtmlParser(dataLc, setting);
                    }

                    break;

                case SportType.NumberGame:
                    var dataNg = _resultService.GetPopUpNumberGameMatchs(metalModel.MatchId);
                    if (dataNg != null)
                    {
                        _htmlResult = ResultEngine.HtmlParser(dataNg, setting);
                    }

                    break;

                default:
                    var data = _resultService.GetPopUpMatch(metalModel.MatchId, metalModel.Bettype, metalModel.BetId);
                    if (data != null)
                    {
                        _htmlResult = ResultEngine.HtmlParser(data, setting);
                    }

                    break;
            }
        }

        #endregion popup-page

        #region spots list

        /// <summary>
        /// Get normal sports.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <param name="sports">The sport list.</param>
        /// <param name="leagues">The league list.</param>
        /// <returns>The JSON result.</returns>
        [SuppressMessage(
          "StyleCop.CSharp.StylecopPlus.Rules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        private ActionResult GetNormalSports(ResultMetalModel metalModel, out IList<Sport> sports, out IList<League> leagues)
        {
            var licenseeSession = SessionService.LicenseeSession;
            leagues = null;
            sports = _resultService.GetSports(metalModel.FromDate, licenseeSession.LicenseeId);
            if (sports.Count > 0)
            {
                IEnumerable<Sport> limitedSports;
                var productAccess = ProductService.GetProductAccess(
                    licenseeSession.LicenseeId,
                    SessionService.UserSession.Language,
                    ResultPageGetNormalSportsName);
                var limitedProduct =
                    productAccess.IsAllowRB && productAccess.IsAllowLC && productAccess.IsAllowNG
                    && productAccess.IsAllowVS;
                if (!limitedProduct)
                {
                    const int LCSport = (int)SportType.LiveCasino;
                    const int NGSport = (int)SportType.NumberGame;
                    const int HRSport = (int)SportType.HorseRacing;
                    const int GRSport = (int)SportType.Greyhound;
                    const int HNSport = (int)SportType.Harness;
                    const int VSSoccer = (int)SportType.VirtualSports;
                    limitedSports = sports.Where(i => !i.SportId.Equals(LCSport.ToString())
                      && !i.SportId.Equals(NGSport.ToString()) && !i.SportId.Equals(VSSoccer.ToString()) && !i.SportId.Equals(HRSport.ToString())
                      && !i.SportId.Equals(GRSport.ToString()) && !i.SportId.Equals(HNSport.ToString())).ToList();

                    if (productAccess.IsAllowNG)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(NGSport.ToString())));
                    }

                    if (productAccess.IsAllowLC)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(LCSport.ToString())));
                    }

                    if (productAccess.IsAllowRB)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(HRSport.ToString())
                        || i.SportId.Equals(GRSport.ToString())
                        || i.SportId.Equals(HNSport.ToString())));
                    }

                    if (productAccess.IsAllowVS)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(VSSoccer.ToString())));
                    }
                }
                else
                {
                    limitedSports = sports;
                }

                if (limitedSports.Any())
                {
                    leagues = _resultService.GetLeagues(
                       metalModel.FromDate,
                       int.Parse(limitedSports.First().SportId));
                    return Json(new { Sports = limitedSports, Leagues = leagues });
                }
            }

            return Json(string.Empty);
        }

        /// <summary>
        /// Get out right sport list.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <param name="sports">The sport list.</param>
        /// <param name="leagues">The league list.</param>
        /// <returns>The JSON result.</returns>
        private ActionResult GetOutRightSports(ResultMetalModel metalModel, out IList<Sport> sports, out IList<League> leagues)
        {
            sports = null;
            leagues = null;
            var cacheKey = GenerateOutRightCacheKey(metalModel);
            var outRightData = (IList<Outright>)HttpRuntime.Cache.Get(cacheKey) ?? _resultService.GetOutRight(metalModel);
            if (outRightData != null && outRightData.Count > 0)
            {
                sports =
                    outRightData.Where(i => i.SportType == metalModel.SportId || metalModel.SportId == 0)
                    .Select(i => i.SportType).Distinct()
                    .Select(i => new Sport(i.ToString(CultureInfo.InvariantCulture)))
                        .ToList();
                if (sports.Count > 0)
                {
                    var selectedSport = metalModel.SportId > 0 ? metalModel.SportId
                        : int.Parse(sports[0].SportId);
                    leagues =
                        outRightData.Where(i => i.SportType == selectedSport)
                            .Select(i => i.LeagueId)
                            .Distinct()
                            .Select(i => new League { LeagueId = i }).ToList();

                    return Json(new { Sports = sports, Leagues = leagues });
                }
            }

            return Json(string.Empty);
        }

        #endregion spots list

        #region main-page

        /// <summary>
        /// Get out right data.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        private void GetOutRightData(ResultMetalModel metalModel)
        {
            var setting = new ResultSetting
            {
                IsOutright = true,
                ExcelTitle = ViewResources.Result,
                ExcelTemplatePath = Server.MapPath(@"~/App_Data/Fanex.Result/"),
                LanguageCode = SessionService.UserSession.Language
            };
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
                        this._excelStream = ResultEngine.ExcelParser(data, setting);
                    }
                    else
                    {
                        this._htmlResult = ResultEngine.HtmlParser(data, setting);
                    }
                }
            }
        }

        /// <summary>
        /// Get data for normal page.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void GetNormalData(ResultMetalModel metalModel)
        {
            var sportType = (SportType)metalModel.SportId;
            var setting = new ResultSetting
            {
                SportType = sportType,
                ExcelTitle = ViewResources.Result,
                ExcelTemplatePath = Server.MapPath(@"~/App_Data/Fanex.Result/"),
                LanguageCode = SessionService.UserSession.Language,
                VisibleDetailedLink = true,
                VisibleFreeHeader = true,
                EnableCheckScoreCalculated = sportType == SportType.Soccer
            };
            switch (sportType)
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
                    if (dataRc != null)
                    {
                        if (metalModel.IsExportExcel)
                        {
                            _excelStream = ResultEngine.ExcelParser(dataRc, setting);
                        }
                        else
                        {
                            _htmlResult = ResultEngine.HtmlParser(dataRc, setting);
                        }
                    }

                    break;

                case SportType.LiveCasino:
                    var dataLc = _resultService.GetLiveCasinoMatches(metalModel);

                    if (dataLc != null)
                    {
                        if (metalModel.IsExportExcel)
                        {
                            _excelStream = ResultEngine.ExcelParser(dataLc, setting);
                        }
                        else
                        {
                            _htmlResult = ResultEngine.HtmlParser(dataLc, setting);
                        }
                    }

                    break;

                case SportType.NumberGame:
                    var dataNg = _resultService.GetNumberGameMatches(
                        metalModel);
                    if (dataNg != null)
                    {
                        if (metalModel.IsExportExcel)
                        {
                            _excelStream = ResultEngine.ExcelParser(dataNg, setting);
                        }
                        else
                        {
                            _htmlResult = ResultEngine.HtmlParser(dataNg, setting);
                        }
                    }

                    break;

                default:
                    var data = _resultService.GetMatches(metalModel);
                    if (data != null)
                    {
                        if (metalModel.IsExportExcel)
                        {
                            _excelStream = ResultEngine.ExcelParser(data, setting);
                        }
                        else
                        {
                            _htmlResult = ResultEngine.HtmlParser(data, setting);
                        }
                    }

                    break;
            }
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

        /// <summary>
        /// Get Keno API setting.
        /// </summary>
        /// <returns>The API setting.</returns>
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

        #endregion Utilities
    }
}