// --------------------------------------------------------------------------
// <copyright file="HandicapOverUnderForecastController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Linq;
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

    [FunctionAuthorization(PermissionKeys.ForecastHandicapOverUnderLive)]
    public class HandicapOverUnderForecastController : ForecastBaseController
    {
        private readonly ILeagueDataAccess _leagueDataAaccess;

        private readonly IForecastDataAccess _forecastDataAccess;

        public HandicapOverUnderForecastController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ILeagueDataAccess leagueDataAccess,
            IForecastDataAccess forecastDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _leagueDataAaccess = leagueDataAccess;
            _forecastDataAccess = forecastDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var liceseeSession = SessionService.LicenseeSession;
            if (AppSettings.LicenseeOfferNewForecast.Contains(liceseeSession.LicenseeId))
            {
                return this.RedirectToAction("Index", "NewHandicapOverUnderForecast");
            }

            var metaModel = new ForecastHandicapOverUnderMetaModel
            {
                LicenseeId = liceseeSession.LicenseeId,
                UserId = SessionService.UserSession.UserId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = await GetViewModel(metaModel).ConfigureAwait(false);

            return View("~/Views/Desktop/Forecast/ForecastHDPOU/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(ForecastHandicapOverUnderMetaModel metaModel)
        {
            var model = await GetViewModel(metaModel).ConfigureAwait(false);

            return View("~/Views/Desktop/Forecast/ForecastHDPOU/Main.cshtml", model);
        }

        private async Task<ForecastHdpOuViewModel> GetViewModel(ForecastHandicapOverUnderMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var leagueList = await _leagueDataAaccess.GetRunningLeagues(metaModel.AgentListXml, metaModel.BetTypeString).ConfigureAwait(true);
            var currentLeagueId = metaModel.LeagueId;
            metaModel.LeagueId = GetLeagueId(currentLeagueId, leagueList);
            var forecastHdpOuByLeague = _forecastDataAccess.GetForecastHandicapOverUnder(metaModel);

            return new ForecastHdpOuViewModel
            {
                Param = metaModel,
                MainData = forecastHdpOuByLeague,
                LeagueList = leagueList,
                CurrencyList = SessionService.UserSession.CurrencyList
            };
        }
    }
}