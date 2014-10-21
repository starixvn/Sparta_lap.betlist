// -----------------------------------------------------------------------
// <copyright file="ReportController.ResultPage.cs" company="Nexcel">
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
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Keno;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Keno;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Keno;
    using Fanex.Athena.Service.Interface.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Result.Core;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// The result controller.
    /// </summary>
    public partial class ReportController
    {
        private string _htmlResult;
        private MemoryStream _excelStream;

        #region Result-Page

        /// <summary>
        /// The result page.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Result()
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
                    BalanceUpDay = this._balanceUpService.BalanceUpInfo.BalanceUpDay
                }
            };
            var productAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            ViewBag.IsAllowedRNG = productAccess.IsAllowBG || productAccess.IsAllowCS;
            ViewBag.IsAllowedKeno = productAccess.IsAllowKN;
            return View("~/Views/Report/Result/Main.cshtml", model);
        }

        /// <summary>
        /// The result report.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The actin result.</returns>
        [AjaxFunctionAuthorization]
        [HttpPost]
        [AjaxRequest]
        public ActionResult Result(ResultMetalModel metalModel)
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
            IResultService resultService = new ResultService();

            const string FileName = "LRF-Result.xls";
            if (ModelState.IsValid)
            {
                metalModel.IsExportExcel = true;
                if (metalModel.SportId == 0)
                {
                    /*Get firstSport*/
                    var sports = resultService.GetSports(metalModel.FromDate, SessionService.LicenseeSession.LicenseeId);
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
        public ActionResult GetSports(ResultMetalModel metalModel)
        {
            if (ModelState.IsValid)
            {
                IResultService resultService = new ResultService();

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
                    leagues = resultService.GetLeagues(metalModel.FromDate, metalModel.SportId);
                    return Json(new { Leagues = leagues });
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
            IKenoApi api = new KenoApi();
            var userId = SessionService.UserSession.UserId;
            return Json(
                new
            {
                Url = api.CreateKenoResultUrl(GetKenoSetting(), userId.ToString(), DateTime.Now),
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
        public string KenoResult(string userId)
        {
            IKenoApi api = new KenoApi();

            if (string.IsNullOrEmpty(userId) || !SessionService.UserSession.UserId.ToString().Equals(userId))
            {
                Fanex.Logging.LogService.Error("Keno Result page just submited invalid userId");

                return Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme);
            }

            var result = api.ExtendToken(GetKenoSetting(), userId);

            if (result.ErrorCode > 0)
            {
                Fanex.Logging.LogService.Error(result.ErrorMsg);
            }

            return result.ErrorCode > 0 ? Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme) : string.Empty;
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
            var userSession = SessionService.UserSession;
            IResultService resultService = new ResultService();
            var sportType = (SportType)metalModel.SportType;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userId = userSession.UserId;
            var setting = new ResultSetting
            {
                SportType = sportType,
                IsOutRight = metalModel.IsOutRight.HasValue && metalModel.IsOutRight.Value > 0,
                IsParlayPopUp = metalModel.Bettype == 9 || metalModel.Bettype == 29,
                IsPopUp = true,
                LanguageCode = userSession.Language
            };
            if (setting.IsParlayPopUp)
            {
                if (metalModel.RefNo != null)
                {
                    var parlayData = resultService.GetPopUpParlayMatch(metalModel.RefNo.Value);
                    _htmlResult = ResultEngine.HtmlParser(parlayData, setting);
                }
            }
            else
            {
                GetNormalPopUpData(metalModel, sportType, licenseeId, userId, setting);
            }
        }

        /// <summary>
        /// Get normal pop up data.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <param name="sportType">The sport type.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="setting">The setting result.</param>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void GetNormalPopUpData(ResultPopupMetalModel metalModel, SportType sportType, int licenseeId, int userId, ResultSetting setting)
        {
            IResultService resultService = new ResultService();

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
                        var dataRc = resultService.GetPopUpRacingMatch(
                            metalModel.MatchId,
                            metalModel.RaceNo.Value,
                            licenseeId,
                            userId);
                        if (dataRc != null)
                        {
                            this._htmlResult = ResultEngine.HtmlParser(dataRc, setting);
                        }
                    }

                    break;

                case SportType.LiveCasino:

                    var dataLc = resultService.GetPopUpLiveCasinoMatch(
                        metalModel.MatchId,
                        licenseeId,
                        userId);
                    if (dataLc != null)
                    {
                        this._htmlResult = ResultEngine.HtmlParser(dataLc, setting);
                    }

                    break;

                case SportType.NumberGame:
                    var dataNg = resultService.GetPopUpNumberGameMatchs(metalModel.MatchId);
                    if (dataNg != null)
                    {
                        _htmlResult = ResultEngine.HtmlParser(dataNg, setting);
                    }

                    break;

                default:
                    var data = resultService.GetPopUpMatch(metalModel.MatchId, licenseeId, userId);
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
          "StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        private ActionResult GetNormalSports(ResultMetalModel metalModel, out IList<Sport> sports, out IList<League> leagues)
        {
            IResultService resultService = new ResultService();
            var licenseeSession = SessionService.LicenseeSession;
            leagues = null;
            sports = resultService.GetSports(metalModel.FromDate, licenseeSession.LicenseeId);
            if (sports.Count > 0)
            {
                IEnumerable<Sport> limitedSports;
                var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreCasinoAngBingo);
                var limitedProduct =
                    productAccess.IsAllowRB && productAccess.IsAllowLC && productAccess.IsAllowNG
                    && productAccess.IsAllowVS;
                if (!limitedProduct)
                {
                    var lcsport = (int)SportType.LiveCasino;
                    var ngsport = (int)SportType.NumberGame;
                    var hrsport = (int)SportType.HorseRacing;
                    var grsport = (int)SportType.Greyhound;
                    var hnsport = (int)SportType.Harness;
                    var vssoccer = (int)SportType.VirtualSports;
                    limitedSports = sports.Where(i => !i.SportId.Equals(lcsport.ToString())
                      && !i.SportId.Equals(ngsport.ToString()) && !i.SportId.Equals(vssoccer.ToString()) && !i.SportId.Equals(hrsport.ToString())
                      && !i.SportId.Equals(grsport.ToString()) && !i.SportId.Equals(hnsport.ToString())).ToList();

                    if (productAccess.IsAllowNG)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(ngsport.ToString())));
                    }

                    if (productAccess.IsAllowLC)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(lcsport.ToString())));
                    }

                    if (productAccess.IsAllowRB)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(hrsport.ToString())
                        || i.SportId.Equals(grsport.ToString())
                        || i.SportId.Equals(hnsport.ToString())));
                    }

                    if (productAccess.IsAllowVS)
                    {
                        limitedSports = limitedSports.Union(sports.Where(i => i.SportId.Equals(vssoccer.ToString())));
                    }
                }
                else
                {
                    limitedSports = sports;
                }

                if (limitedSports.Count() > 0)
                {
                    leagues = resultService.GetLeagues(
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
            IResultService resultService = new ResultService();

            sports = null;
            leagues = null;
            var cacheKey = GenerateOutRightCacheKey(metalModel);
            var outRightData = (IList<OutRight>)HttpRuntime.Cache.Get(cacheKey) ?? resultService.GetOutRight(metalModel);
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
            IResultService resultService = new ResultService();

            var setting = new ResultSetting
            {
                IsOutRight = true,
                ExcelTitle = ViewResources.Result,
                ExcelTemplatePath = Server.MapPath(@"~/App_Data/Fanex.Result/"),
                LanguageCode = SessionService.UserSession.Language
            };
            var cacheKey = GenerateOutRightCacheKey(metalModel);
            var outRightData = (IList<OutRight>)HttpRuntime.Cache.Get(cacheKey) ?? resultService.GetOutRight(metalModel);
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
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void GetNormalData(ResultMetalModel metalModel)
        {
            IResultService resultService = new ResultService();

            var sportType = (SportType)metalModel.SportId;
            var setting = new ResultSetting
            {
                SportType = (SportType)metalModel.SportId,
                ExcelTitle = ViewResources.Result,
                ExcelTemplatePath = Server.MapPath(@"~/App_Data/Fanex.Result/"),
                LanguageCode = SessionService.UserSession.Language
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
                    var dataRc = resultService.GetRacingMatches(metalModel);
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
                    var dataLc = resultService.GetLiveCasinoMatches(metalModel);

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
                    var dataNg = resultService.GetNumberGameMatches(
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
                    var data = resultService.GetMatches(metalModel);
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