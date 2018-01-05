// -----------------------------------------------------------------------
// <copyright file="HandHistoryController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report.RNGCasino
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Excel.Report.RNGCasino;
    using Extension.Utility;
    using Fanex.Athena.Model.Casino.DTO;
    using Fanex.Athena.Model.Casino.DTO.RNGResultDetail;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Casino.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Casino;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Web.ResultExtension;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.Required)]
    public class HandHistoryController : BaseController
    {
        private readonly IRNGCasinoService _rngCasinoService;

        public HandHistoryController() : this(null, null, null, null)
        {
        }

        public HandHistoryController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IRNGCasinoService rngCasinoService)
            : base(languageService, sessionService, appSetting)
        {
            _rngCasinoService = rngCasinoService ?? new RNGCasinoService();
        }

        [HttpGet]
        public ActionResult RNGCasinoData(RNGCasinoResultMetaModel metaModel)
        {
            if (metaModel != null)
            {
                var data = GetData(metaModel);
                var totalData = GetTotalData(data);
                var json = Json(new { Param = metaModel, Data = data, Total = totalData }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }

            return this.Json(new { Param = string.Empty, Data = string.Empty, Total = string.Empty, });
        }

        [HttpGet]
        public ActionResult RNGCasino(RNGCasinoResultMetaModel metaModel)
        {
            if (metaModel.IsExportExcel)
            {
                return GetExcelResult(metaModel);
            }

            var model = new RNGCasinoResultViewModel
            {
                Param = metaModel,
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            return this.View("~/Views/RNGCasino/Main.cshtml", model);
        }

        [HttpGet]
        public ViewResult RNGCasinoDetail(RNGCasinoResultDetailMetaModel metaModel)
        {
            metaModel.CurrencyName = metaModel.CurrencyName.ToUpperInvariant();
            const string ViewPathTemplate = "~/Views/RNGCasino/RNGCasinoDetail/Group{0}.cshtml";
            var group = GetGroupByGameTypeId(metaModel.GameTypeId);
            var model = metaModel.IsValid && (metaModel.CurrencyId == 0 || this.SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
                ? this._rngCasinoService.GetRNGCasinoResultDetail(metaModel)
                : new CasinoHandHistoryDetailViewModel();

            model.Param = metaModel;

            if (string.IsNullOrEmpty(group))
            {
                return this.View("~/Views/RNGCasino/RNGCasinoDetail/NoGameErr.cshtml");
            }

            if (model.CasinoDetailHeaderList != null && model.CasinoDetailHeaderList.Count > 0)
            {
                return this.View(string.Format(ViewPathTemplate, group), model);
            }

            return this.View("~/Views/RNGCasino/RNGCasinoDetail/Noinfo.cshtml");
        }

        private static RNGCasinoResult GetTotalData(IList<RNGCasinoResult> data)
        {
            return new RNGCasinoResult
            {
                Stake = data.Sum(x => x.Stake),
                Bet = data.Sum(x => x.Bet),
                WinLoss = data.Sum(x => x.WinLoss),
                RollingComm = data.Sum(x => x.RollingComm),
                Result = string.Empty,
            };
        }

        [SuppressMessage(
              "StyleCop.CSharp.StylecopPlus.Rules",
              "SP2101:MethodMustNotContainMoreLinesThan",
              Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage(
            "Sonar Code Smell",
            "S1541:Methods and properties should not be too complex",
            Justification = "Reviewed.")]
        private static string GetGroupByGameTypeId(int gameTypeId)
        {
            switch (gameTypeId)
            {
                case (int)CasinoGameType.Baccarat:
                case (int)CasinoGameType.Blackjack:
                case (int)CasinoGameType.CasinoHoldEm:
                case (int)CasinoGameType.BlackjackPerfectPair:
                case (int)CasinoGameType.BaccaratSuper6:
                case (int)CasinoGameType.ThreeKnights:
                case (int)CasinoGameType.DragonTiger:
                case (int)CasinoGameType.BattleWar:
                case (int)CasinoGameType.PokerTri:
                case (int)CasinoGameType.PaiGow:
                case (int)CasinoGameType.FaceUp21:
                case (int)CasinoGameType.ThreeCardsRummy:
                case (int)CasinoGameType.SicBO:
                case (int)CasinoGameType.RoulettePro:
                case (int)CasinoGameType.RouletteProFast:
                case (int)CasinoGameType.MiniJacksorBetter:
                case (int)CasinoGameType.MiniBaccarat:
                case (int)CasinoGameType.MiniSicBO:
                case (int)CasinoGameType.MiniBlackjack:
                case (int)CasinoGameType.MiniCasinoHoldEm:
                    return "1";

                case (int)CasinoGameType.TexasHoldEm:
                    return "1-1";

                case (int)CasinoGameType.RedDog:
                case (int)CasinoGameType.MiniRoulette:
                case (int)CasinoGameType.FishPrawnCrab:
                case (int)CasinoGameType.CardWheel:
                case (int)CasinoGameType.FanTan:
                    return "2";

                case (int)CasinoGameType.MoneySlot:
                case (int)CasinoGameType.Engagement:
                case (int)CasinoGameType.FoxForce5:
                case (int)CasinoGameType.TattooArt:
                case (int)CasinoGameType.MiniMoneySlot:
                    return "3";

                case (int)CasinoGameType.Champions:
                case (int)CasinoGameType.WaterMargin:
                    return "4";

                case (int)CasinoGameType.WesternZodiac:
                    return "4-1";

                case (int)CasinoGameType.MoneyWheel:
                    return "5";

                case (int)CasinoGameType.JourneytotheWest:
                    return "6";

                case (int)CasinoGameType.ChineseZodiac:
                    return "6-1";

                case (int)CasinoGameType.JacksorBetter:
                    return "7-0";

                case (int)CasinoGameType.JungleWild:
                case (int)CasinoGameType.GodofProsperity:
                    return "7-1";

                case (int)CasinoGameType.No4LineJacksorBetter:
                case (int)CasinoGameType.No25LineJacksorBetter:
                case (int)CasinoGameType.No50LineJacksorBetter:
                    return "7-2";

                case (int)CasinoGameType.No10SOrBetter:
                    return "7-3";

                case (int)CasinoGameType.MonkeyHilo:
                case (int)CasinoGameType.MahjongHilo:
                    return "7-4";

                case (int)CasinoGameType.KenoChinese:
                case (int)CasinoGameType.KenoCamera:
                    return "7-5";

                case (int)CasinoGameType.TurfKing:
                    return "7-6";

                case (int)CasinoGameType.MamakCorner:
                case (int)CasinoGameType.WinterSlot:
                case (int)CasinoGameType.DragonSlot:
                    return "7-8";

                case (int)CasinoGameType.MayaPush:
                case (int)CasinoGameType.Knock2Eggs:
                case (int)CasinoGameType.HoHoHo:
                case (int)CasinoGameType.PokeMole:
                case (int)CasinoGameType.GongXiFaCai:
                case (int)CasinoGameType.FruitScratch:
                case (int)CasinoGameType.FlamesofOlympics:
                    return "8";

                case (int)CasinoGameType.PenaltyHatTrick:
                case (int)CasinoGameType.FancyScratch:
                case (int)CasinoGameType.LoveStory:
                    return "9";

                default:
                    return string.Empty;
            }
        }

        private ActionResult GetExcelResult(RNGCasinoResultMetaModel metaModel)
        {
            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/RNGCasino/HandHistory.xls");
            metaModel.IsShowPic = false;
            var data = GetData(metaModel);
            var totalData = GetTotalData(data);
            var viewModel = new RNGCasinoResultViewModel
            {
                Data = data,
            };

            var excel = CreateExcel(templatePath, metaModel, totalData, viewModel);
            excel.Export();

            return new ExcelResult(excel.Workbook, "CasinoHandHistory");
        }

        protected internal virtual HandHistoryExcel CreateExcel(
            string templatePath,
            RNGCasinoResultMetaModel metaModel,  
            RNGCasinoResult totalData, 
            RNGCasinoResultViewModel viewModel)
        {
            return new HandHistoryExcel(templatePath, metaModel, viewModel, totalData);
        }

        private IList<RNGCasinoResult> GetData(RNGCasinoResultMetaModel metaModel)
        {
            metaModel.BaseCurrency = metaModel.BaseCurrency ?? string.Empty;
            return metaModel.IsValid() && (metaModel.CurrencyId == BaseCurrencyType.All ||
                SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
                ? _rngCasinoService.GetRNGCasinoResult(metaModel)
                : new List<RNGCasinoResult>();
        }
    }
}