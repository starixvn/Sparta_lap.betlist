// ForecastController.NewHandicapOverUnder.cs--------------------------------
// <copyright file="NewHandicapOverUnderForecastController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Forecast
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Forecast.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.Core.DataDomain;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.Forecast.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ForecastHandicapOverUnderLive)]
    public class NewHandicapOverUnderForecastController : ForecastBaseController
    {
        private readonly ICoreLogger _logger;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IMatchService _matchService;

        private readonly ILeagueDataAccess _leagueDataAccess;

        private readonly INewForecastDataAccess _forecastDataAccess;

#pragma warning disable S107 // Methods should not have too many parameters

        public NewHandicapOverUnderForecastController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            INewForecastDataAccess forecastDataAccess,
            ICoreLogger logger,
            ICacheRefAdapter cacheRefAdapter,
            IMatchService matchService,
            ILeagueDataAccess leagueDataAccess)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting)
        {
            _logger = logger;
            _cacheRefAdapter = cacheRefAdapter;
            _matchService = matchService;
            _forecastDataAccess = forecastDataAccess;
            _leagueDataAccess = leagueDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int leagueId = 0, int matchId = 0, int currencyId = 0, string baseCurrency = null)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            if (AppSettings.LicenseeOfferNewForecast.Contains(licenseeId))
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

                var viewModel = await GetViewModel(metaModel).ConfigureAwait(false);
                return View("~/Views/Desktop/Forecast/NewForecastHdpOu/Main.cshtml", viewModel);
            }
            else
            {
                return RedirectToAction("ForecastHandicapOverUnder");
            }
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(NewForecastHandicapOverUnderMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var viewModel = await this.GetViewModel(metaModel).ConfigureAwait(false);
            return View("~/Views/Desktop/Forecast/NewForecastHdpOu/Main.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ViewResult> GetMatchListByLeague(int leagueId)
        {
            var matchList = (await _matchService.GetForecastMatchListByLeague(leagueId).ConfigureAwait(false)).OrderBy(x => x.MatchName).ToList();
            return View("~/Views/Desktop/Forecast/NewForecastHdpOu/MatchOptionList.cshtml", matchList);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        private async Task<NewForecastHdpOuViewModel> GetViewModel(NewForecastHandicapOverUnderMetaModel metaModel)
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
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

                if (!Request.IsAjaxRequest())
                {
                    await AssignDataForNonAjaxRequest(metaModel, model).ConfigureAwait(false);
                }

                metaModel.HomeTeamName = _cacheRefAdapter.GetTeamName(metaModel.HomeTeamId) + (metaModel.IsNeutral ? " (n)" : string.Empty);
                metaModel.AwayTeamName = _cacheRefAdapter.GetTeamName(metaModel.AwayTeamId);
                model.MainData = metaModel.IsValid ? 
                    _forecastDataAccess.GetNewForecastData(metaModel, headerInfo) :
                    new NewForecastHdpOuData();
                model.HeaderInfo = headerInfo;
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            return model;
        }

        private async Task AssignDataForNonAjaxRequest(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuViewModel model)
        {
            model.LeagueList = await _leagueDataAccess.GetRunningLeagues(metaModel.AgentListXml, metaModel.BetTypeString).ConfigureAwait(false);
            if (model.LeagueList.Any())
            {
                metaModel.LeagueId = GetLeagueId(metaModel.LeagueId, model.LeagueList);
                if (metaModel.LeagueId > 0)
                {
                    await AssignMatchData(metaModel, model).ConfigureAwait(false);
                }
            }

            model.Param = metaModel;
        }

        private async Task AssignMatchData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuViewModel model)
        {
            model.MatchList = (await _matchService.GetForecastMatchListByLeague(metaModel.LeagueId).ConfigureAwait(false)).OrderBy(x => x.MatchName).ToList();

            if (model.MatchList.Count > 0)
            {
                /*get current match or first match*/
                var match = model.MatchList.SingleOrDefault(l => l.MatchId == metaModel.MatchId);
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
}