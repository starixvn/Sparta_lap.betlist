// --------------------------------------------------------------------------
// <copyright file="OneXTwoForecastController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.25">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    public class OneXTwoForecastController : ForecastBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var metaModel = new ForecastOneXTwoMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = this.GetViewModel(metaModel);

            return View("~/Views/Forecast/Forecast1X2/Main.cshtml", model);
        }

        [HttpPost]
        [FunctionAuthorization]
        [CustomValidateAntiForgery]
        public ActionResult Index(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            var model = this.GetViewModel(metaModel);

            return View("~/Views/Forecast/Forecast1X2/Main.cshtml", model);
        }

        [FunctionAuthorization]
        public ActionResult Popup(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            var model = this.GetViewModel(metaModel);

            return View("~/Views/Forecast/Forecast1X2/Main.cshtml", model);
        }

        private ForecastOneXTwoViewModel GetViewModel(ForecastOneXTwoMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var model = new ForecastOneXTwoViewModel();
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            var leagueList = ForecastService.GetForecastRunningLeague(metaModel);
            var currentLeagueId = metaModel.LeagueId;

            if (metaModel.MatchId != 0)
            {
                var forecastOneXTwoByMatch = ForecastService.GetForecastOneXTwoByMatch(metaModel, userSession.UserId);
                model.MainData = forecastOneXTwoByMatch;
            }
            else
            {
                metaModel.LeagueId = GetLeagueId(currentLeagueId, leagueList);

                var forecastcOneXTwoByLeague = ForecastService.GetForecastOneXTwoByLeague(metaModel);
                model.MainData = forecastcOneXTwoByLeague;
            }

            model.Param = metaModel;
            model.LeagueList = leagueList;
            model.LicenseeTypeList = licenseeSession.LicenseeType;
            model.CurrencyList = GetUserCurrencies();

            return model;
        }
    }
}