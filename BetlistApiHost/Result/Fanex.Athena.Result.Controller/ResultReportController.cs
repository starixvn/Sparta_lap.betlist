namespace Fanex.Athena.Result.Controller
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Result.Core.AllBetCasino;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.GlobalGaming;
    using Fanex.Result.Core.GlobalGaming.DTO;
    using Fanex.Result.Core.MicroGaming;
    using Fanex.Result.Core.MicroGaming.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Result.Core.Voidbridge;
    using Model.MetaModel;
    using Resources;
    using Service.Interfaces;
    using VendorAdapter.Interfaces;

    public class ResultReportController : Controller
    {
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

        private readonly IParserService _microGamingParserService;

        private readonly IResultService _resultService;

        private readonly IRNGCasinoSingleWalletService _rngCasinoSingleWalletService;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private string _htmlResult;
        private MemoryStream _excelStream;

        #region Result-Page

        public ResultReportController(
            IAppSettings appSetting,
            IParserService microGamingParserService,
            IResultService resultService,
            IRNGCasinoSingleWalletService rngCasinoSingleWalletService,
            ICacheRefAdapter cacheRefAdapter)
        {
            _microGamingParserService = microGamingParserService;
            _resultService = resultService;
            _rngCasinoSingleWalletService = rngCasinoSingleWalletService;
            _cacheRefAdapter = cacheRefAdapter;
        }

        [HttpPost]
        public string Index(ResultMetalModel metalModel)
        {
            metalModel.IsExportExcel = false;
            this.GetData(metalModel);
            return _htmlResult;
        }

        [HttpPost]
        public void ResultExcel(ResultMetalModel metalModel)
        {
            var response = ControllerContext.HttpContext.Response;

            if (ModelState.IsValid)
            {
                metalModel.IsExportExcel = true;
                metalModel.SportId = metalModel.SportId == 0 ?
                    GetSportId(metalModel.FromDate, metalModel.LicenseeId) :
                    metalModel.SportId;

                if (metalModel.SportId > 0)
                {
                    GetData(metalModel);
                }

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

        [HttpPost]
        public ActionResult GetSports(GetSportResultMetalModel metalModel)
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

        [HttpPost]
        public string ResultPopup(ResultPopupMetalModel metalModel)
        {
            GetPopUpData(metalModel);
            return _htmlResult;
        }

        [HttpPost]
        public string MicrogamingResultPopup(MicroGaming param)
        {
            return _microGamingParserService.Parser(param);
        }

        [HttpPost]
        public ActionResult ShowRngCasinoSingleWalletResult(string refNo)
        {
            var resultInfo = _rngCasinoSingleWalletService.GetResultInfo(refNo);

            return Json(resultInfo);
        }

        [HttpPost]
        public string VoidbridgeResult(string custId, string gamePlayId, string languageCode)
        {
            var voidbridgeApi = new VoidbridgeApiImp();
            return voidbridgeApi.GetBetHistoryUrl(custId, gamePlayId, languageCode);
        }

        [HttpPost]
        public string AllBetResult(string transDesc, string betCheck, string betType, string languageCode)
        {
            var allbetResultService = new AllbetResultBuilder();
            return allbetResultService.Render(transDesc, betCheck, betType, languageCode);
        }

        [HttpPost]
        public string GlobalGamingResult(GlobalGamingDTO param)
        {
            var globalGamingResultApi = new GlobalGamingResultApi();

            return globalGamingResultApi.GetResultUrl(param);
        }

        /// <summary>
        /// Generate tennis detailed result.
        /// </summary>
        /// <param name="matchId">The match ID.</param>
        /// /// <param name="sportType">The sport type.</param>
        /// <returns>The html result.</returns>
        [HttpPost]
        public string ResultDetail(int matchId, int sportType, string languageCode)
        {
            var detailedData = _resultService.GetSportsbookGameDetail(matchId);
            if (detailedData != null && detailedData.MatchId > 0)
            {
                return _resultService.HtmlGameDetailParser(
                detailedData,
                new ResultSetting
                {
                    SportType = (SportType)sportType,
                    LanguageCode = languageCode
                });
            }

            return string.Empty;
        }

        #endregion PopUpPage

        #region Utilities

        #region popup-page

        private static bool IsPalayBetType(int betType)
        {
            return betType == ParlayBetType || betType == SystemParlayBetType || betType == (int)Constant.DataDomain.BetType.VirtualSports2MixParlay;
        }

        private static IEnumerable<Sport> RemoveNotAllowedSports(IEnumerable<Sport> sports, ProductAccess productAccess)
        {
            var limitedSports = sports;
            if (!productAccess.IsAllowNG)
            {
                limitedSports = limitedSports.Where(i => i.SportId != (int)SportType.NumberGame && i.SportId != (int)SportType.Happy5);
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
                LanguageCode = metalModel.LanguageCode,
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
                case SportType.VirtualHorseRacing:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualSpeedway:
                case SportType.VirtualCycling:
                    if (metalModel.RaceNo != null)
                    {
                        var dataRc = _resultService.GetPopUpVirtualSportMatch(metalModel);
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

                case SportType.Happy5:
                    var dataHappy5 = _resultService.GetPopUpHappy5Matches(metalModel.MatchId);
                    if (dataHappy5 != null)
                    {
                        _htmlResult = _resultService.HtmlParser(dataHappy5, setting);
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

        private JsonResult GetNormalSports(GetSportResultMetalModel metalModel)
        {
            var sports = _resultService.GetSports(metalModel.FromDate, metalModel.LicenseeId);
            if (sports.Any())
            {
                var productList = metalModel.ProductCodeList.Split(',').Select(x => new Product { ProductCode = x }).ToList();

                var productAccess = new ProductAccess(productList);
                var isAllowAllProducts = productAccess.IsAllowNG && productAccess.IsAllowVS;

                var limitedSports = isAllowAllProducts ? sports : RemoveNotAllowedSports(sports, productAccess);

                if (limitedSports.Any())
                {
                    var leagues = _resultService.GetLeagues(metalModel.FromDate, limitedSports.First().SportId);
                    return Json(new { Sports = limitedSports, Leagues = leagues });
                }
            }

            return Json(string.Empty);
        }

        private JsonResult GetOutRightSports(ResultMetalModel metalModel)
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
            var setting = GetOutRightResultSetting(metalModel.LanguageCode);
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
                case SportType.VirtualHorseRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualCycling:
                case SportType.VirtualSpeedway:
                    var dataRc = _resultService.GetVirtualSportMatches(metalModel);
                    ParseResult(dataRc, setting, metalModel.IsExportExcel);
                    break;

                case SportType.NumberGame:
                    var dataNg = _resultService.GetNumberGameMatches(metalModel);
                    ParseResult(dataNg, setting, metalModel.IsExportExcel);
                    break;

                case SportType.Happy5:
                    var dataHappy5 = _resultService.GetHappy5Matches(metalModel);
                    ParseResult(dataHappy5, setting, metalModel.IsExportExcel);
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
                var setting = GetOutRightResultSetting(metalModel.LanguageCode);
                return _resultService.ExcelParser(new List<Outright>(), setting);
            }
            else
            {
                metalModel.SportId = (int)SportType.Soccer;
                var setting = GetNormalResultSetting(metalModel);
                return _resultService.ExcelParser(new List<Sportsbook>(), setting);
            }
        }

        private ResultSetting GetOutRightResultSetting(string languageCode)
        {
            return new ResultSetting
            {
                IsOutright = true,
                ExcelTitle = ViewResources.Result,
                ExcelTemplatePath = HostingEnvironment.MapPath(@"~/App_Data/Fanex.Result/"),
                LanguageCode = languageCode,
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
                LanguageCode = metalModel.LanguageCode,
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
            return string.Format(PatternKey, metalModel.FromDate, metalModel.ToDate, metalModel.UserId);
        }

        #endregion Utilities
    }
}