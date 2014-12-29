// -----------------------------------------------------------------------
// <copyright file="HandHistoryController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.RNGCasino
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Casino;
    using Fanex.Athena.Models.DTO.Casino.RNGResultDetail;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.Casino;
    using Fanex.Athena.Models.ViewModel.Casino.RNGResultDetail;
    using Fanex.Athena.Service.Implement.Casino;
    using Fanex.Athena.Service.Interface.Casino;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Logging;

    /// <summary>
    ///  Hand history controller.
    /// </summary>
    [SessionState(SessionStateBehavior.Required)]
    public class HandHistoryController : BaseController
    {
        /// <summary>
        /// RNG Casino Service.
        /// </summary>
        private readonly IRNGCasinoService _rgnCasinoService = new RNGCasinoService();

        /// <summary>
        /// Get RNG Casino data.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <returns>The data string.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult RNGCasinoData(RNGCasinoResultMetalModel metaModel)
        {
            try
            {
                if (metaModel != null)
                {
                    metaModel.BaseCurrency = metaModel.BaseCurrency ?? string.Empty;
                    var data = metaModel.Isvalid() && (metaModel.CurrencyId == BaseCurrencyType.All || 
                        this.SessionService.UserSession.CurrencyList.Values.Contains(metaModel.CurrencyId))
                        ? this._rgnCasinoService.GetRNGCasinoResult(metaModel)
                        : new List<RNGCasinoResult>();
                    var total = new List<RNGCasinoResult>();

                    if (data != null)
                    {
                        var item = new RNGCasinoResult
                        {
                            Stake = data.Sum(x => x.Stake),
                            Bet = data.Sum(x => x.Bet),
                            WinLoss = data.Sum(x => x.WinLoss),
                            RollingComm = data.Sum(x => x.RollingComm),
                            Result = string.Empty,
                        };
                        total.Add(item);
                    }

                    var jsonStr = Json(new { Param = metaModel, Data = data, Total = total, }, JsonRequestBehavior.AllowGet);
                    jsonStr.MaxJsonLength = int.MaxValue;
                    return jsonStr;
                }

                return this.Json(new { Param = string.Empty, Data = string.Empty, Total = string.Empty, });
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Load RNGCasino result page.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult RNGCasino(RNGCasinoResultMetalModel metalModel)
        {
            var model = new RNGCasinoService().GetRNGCasinoResultDataTable(metalModel);

            return this.View("~/Views/RNGCasino/Main.cshtml", model);
        }

        /// <summary>
        /// Load RNGCasino result page.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult RNGCasinoCallAPIDataTable(RNGCasinoResultMetalModel metalModel)
        {
            var model = new RNGCasinoResultViewModel
            {
                Param = metalModel,
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            return this.View("~/Views/RNGCasino/DataTableFromAPI.cshtml", model);
        }


        /// <summary>
        /// RNG Casino detail.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ViewResult RNGCasinoDetail(RNGCasinoResultDetailMetaModel metaModel)
        {
            metaModel.CurrencyName = metaModel.CurrencyName.ToUpper();
            const string ViewPathTemplate = "~/Views/RNGCasino/RNGCasinoDetail/Group{0}.cshtml";
            var group = GetGroupByGameTypeId(metaModel.GameTypeId);
            var model = metaModel.IsValid && (metaModel.CurrencyId == 0 || this.SessionService.UserSession.CurrencyList.Values.Contains(metaModel.CurrencyId))
                ? this._rgnCasinoService.GetRNGCasinoResultDetail(metaModel)
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

        /// <summary>
        /// Get game type Id.
        /// </summary>
        /// <param name="gameTypeId">The game type Id.</param>
        /// <returns>The game type id.</returns>
        [SuppressMessage(
              "StyleCop.CSharp.Nexcel.NexcelCustomRules",
              "SP2101:MethodMustNotContainMoreLinesThan",
              Justification = "Reviewed. Suppression is OK here.")]
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
    }
}