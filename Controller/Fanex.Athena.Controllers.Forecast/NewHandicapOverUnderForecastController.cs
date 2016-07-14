// ForecastController.NewHandicapOverUnder.cs--------------------------------
// <copyright file="NewHandicapOverUnderForecastController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bravo.Tran" date="2013.08.06">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Service.Core.Helpers;
    using Model.Core.DataDomain;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ForecastHandicapOverUnderLive)]
    public class NewHandicapOverUnderForecastController : ForecastBaseController
    {
        private const string ParentUrl = "HandicapOverUnderForecast";

        private readonly IMatchService _matchService;

        public NewHandicapOverUnderForecastController()
        {
            _matchService = new MatchService();
        }

        [HttpGet]
        public ActionResult Index(int leagueId = 0, int matchId = 0, int currencyId = 0, string baseCurrency = null)
        {
            var licenseeOfferNewForecastList = GetLicenseeOfferNewForecastList();
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            if (licenseeOfferNewForecastList.Contains(licenseeId.ToString()))
            {
                var metaModel = new NewForecastHandicapOverUnderMetaModel
                {
                    LicenseeId = licenseeId,
                    UserId = SessionService.UserSession.UserId,
                    MatchId = matchId,
                    CurrencyId = currencyId,
                    LeagueId = leagueId,
                };

                if (string.IsNullOrEmpty(baseCurrency))
                {
                    SetConvetedUSDCurrencyName(metaModel);
                }
                else
                {
                    metaModel.BaseCurrency = baseCurrency;
                }

                return View("~/Views/Forecast/NewForecastHdpOu/Main.cshtml", this.GetViewModel(metaModel));
            }
            else
            {
                return RedirectToAction("ForecastHandicapOverUnder");
            }
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(NewForecastHandicapOverUnderMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            return View("~/Views/Forecast/NewForecastHdpOu/Main.cshtml", this.GetViewModel(metaModel));
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ViewResult GetMatchListByLeague(int leagueId)
        {
            var matchList = _matchService.GetForecastMatchListByLeague(leagueId).OrderBy(x => x.MatchName).ToList();
            return View("~/Views/Forecast/NewForecastHdpOu/MatchOptionList.cshtml", matchList);
        }

        [SuppressMessage(
                  "StyleCop.CSharp.StylecopPlus.Rules",
                  "SP2101:MethodMustNotContainMoreLinesThan",
                  Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        private NewForecastHdpOuViewModel GetViewModel(NewForecastHandicapOverUnderMetaModel metaModel)
        {
            IList<Match> matchList = new List<Match>();
            IList<League> leagueList = new List<League>();
            var headerInfo = new NewForecastHdpOuMatchInfo();
            var model = new NewForecastHdpOuViewModel
            {
                Param = metaModel,
                LeagueList = leagueList,
                MatchList = matchList,
                HeaderInfo = headerInfo,
                MainData = new NewForecastHdpOuData(),
                CurrencyList = SessionService.UserSession.CurrencyList
            };
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    model.LeagueList = ForecastService.GetForecastRunningLeague(metaModel);
                    if (model.LeagueList.Count > 0)
                    {
                        metaModel.LeagueId = GetLeagueId(metaModel.LeagueId, model.LeagueList);
                        if (metaModel.LeagueId > 0)
                        {
                            model.MatchList = _matchService.GetForecastMatchListByLeague(metaModel.LeagueId).OrderBy(x => x.MatchName).ToList();

                            if (model.MatchList.Count > 0)
                            {
                                /*get current match or first match*/
                                var match = model.MatchList.Where(l => l.MatchId == metaModel.MatchId).SingleOrDefault();
                                if (match == null)
                                {
                                    match = model.MatchList.FirstOrDefault();
                                }

                                if (match != null)
                                {
                                    metaModel.MatchId = match.MatchId;
                                    metaModel.HomeTeamId = match.HomeId;
                                    metaModel.AwayTeamId = match.AwayId;
                                    metaModel.IsNeutral = match.IsNeutral;
                                }
                                else
                                {
                                    metaModel.MatchId = -1;
                                }
                            }
                        }
                    }

                    model.Param = metaModel;
                }

                metaModel.HomeTeamName = CacheRef.GetTeamName(metaModel.HomeTeamId) + (metaModel.IsNeutral ? " (n)" : string.Empty);
                metaModel.AwayTeamName = CacheRef.GetTeamName(metaModel.AwayTeamId);
                model.MainData = ForecastService.GetNewForecastData(metaModel, headerInfo);
                model.HeaderInfo = headerInfo;
            }
            catch (System.Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return model;
        }
    }
}