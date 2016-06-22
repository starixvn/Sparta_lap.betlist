// --------------------------------------------------------------------------
// <copyright file="HandicapOverUnderForecastController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.26">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    [FunctionAuthorization(PermissionKeys.ForecastHandicapOverUnderLive)]
    public class HandicapOverUnderForecastController : ForecastBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var liceseeSession = SessionService.LicenseeSession;
            var licenseeOfferNewForecastList = GetLicenseeOfferNewForecastList();

            if (licenseeOfferNewForecastList.Contains(liceseeSession.LicenseeId.ToString()))
            {
                return this.RedirectToAction("Index", "NewHandicapOverUnderForecast");
            }

            var metaModel = new ForecastHandicapOverUnderMetaModel
            {
                LicenseeId = liceseeSession.LicenseeId,
                UserId = SessionService.UserSession.UserId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = this.GetViewModel(metaModel);

            return View("~/Views/Forecast/ForecastHDPOU/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(ForecastHandicapOverUnderMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            var model = this.GetViewModel(metaModel);

            return View("~/Views/Forecast/ForecastHDPOU/Main.cshtml", model);
        }

        private ForecastHdpOuViewModel GetViewModel(ForecastHandicapOverUnderMetaModel metaModel)
        {
            var leagueList = ForecastService.GetForecastRunningLeague(metaModel);
            var licenseeSession = SessionService.LicenseeSession;
            var currentLeagueId = metaModel.LeagueId;
            metaModel.LeagueId = GetLeagueId(currentLeagueId, leagueList);

            var forecastHdpOuByLeague = ForecastService.GetForecastHandicapOverUnder(metaModel);

            return new ForecastHdpOuViewModel
            {
                Param = metaModel,
                MainData = forecastHdpOuByLeague,
                LeagueList = leagueList,
                LicenseeTypeList = licenseeSession.LicenseeType,
                CurrencyList = GetUserCurrencies(licenseeSession.LicenseeId)
            };
        }
    }
}