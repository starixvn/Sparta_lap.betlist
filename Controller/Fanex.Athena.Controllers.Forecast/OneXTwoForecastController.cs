// --------------------------------------------------------------------------
// <copyright file="OneXTwoForecastController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Forecast.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Forecast.Interfaces;

    [FunctionAuthorization(PermissionKeys.ForecastOneXTwo)]
    public class OneXTwoForecastController : ForecastBaseController
    {
        private readonly ILeagueDataAccess _leagueDataAccess;

        private readonly IForecastOneXTwoDataAccess _forecastOneXTwoDataAccess;

        public OneXTwoForecastController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ILeagueDataAccess leagueDataAccess,
            IForecastOneXTwoDataAccess forecastOneXTwoDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _leagueDataAccess = leagueDataAccess;
            _forecastOneXTwoDataAccess = forecastOneXTwoDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var metaModel = new ForecastOneXTwoMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
            };

            SetConvetedUSDCurrencyName(metaModel);

            return await ProcessAction(metaModel).ConfigureAwait(false);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            return await ProcessAction(metaModel).ConfigureAwait(false);
        }

        public async Task<ActionResult> Popup(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            return await ProcessAction(metaModel).ConfigureAwait(false);
        }

        private async Task<ActionResult> ProcessAction(ForecastOneXTwoMetaModel metaModel)
        {
            var model = await GetViewModel(metaModel).ConfigureAwait(false);

            return View("~/Views/Desktop/Forecast/Forecast1X2/Main.cshtml", model);
        }

        private async Task<ForecastOneXTwoViewModel> GetViewModel(ForecastOneXTwoMetaModel metaModel)
        {
            var model = new ForecastOneXTwoViewModel();

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var leagueList = await _leagueDataAccess.GetRunningLeagues(metaModel.AgentListXml, metaModel.BetTypeString).ConfigureAwait(true);
            var currentLeagueId = metaModel.LeagueId;

            if (metaModel.MatchId != 0)
            {
                var forecastOneXTwoByMatch = _forecastOneXTwoDataAccess.GetForecastOneXTwoByMatch(metaModel);
                model.MainData = forecastOneXTwoByMatch;
            }
            else
            {
                metaModel.LeagueId = GetLeagueId(currentLeagueId, leagueList);

                var forecastcOneXTwoByLeague = _forecastOneXTwoDataAccess.GetForecastOneXTwoByLeague(metaModel);
                model.MainData = forecastcOneXTwoByLeague;
            }

            model.Param = metaModel;
            model.LeagueList = leagueList;
            model.CurrencyList = SessionService.UserSession.CurrencyList;

            return model;
        }
    }
}