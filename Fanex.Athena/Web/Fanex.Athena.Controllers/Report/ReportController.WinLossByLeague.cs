// -----------------------------------------------------------------------
// <copyright file="ReportController.WinLossByLeague.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// Product id that is used only for GetSportsAndProducts() method.
        /// </summary>
        private const int SPORTSBOOK = 0;

        /// <summary>
        /// Win loss by product report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult WinLossByLeague()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var date = DateTime.Now;
            
            var metaModel = new WinLossByLeagueMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = SessionService.UserSession.UserId,
                FromDate = new DateTime(date.Year, date.Month, 1),
                ToDate = date
            };

            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";

            // Base USD currency.
            metaModel.CurrencyId = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? -2 : -1;           
            
            var sports = this._sportTypeService.GetSportsAndProducts(licenseeSession.LicenseeId, SessionService.UserSession.Language, SPORTSBOOK);

            var leagues = this._leagueService.GetLeaguesBySport(
                            new LeagueMetaModel()
                            {
                                CurrencyId = 0,
                                LicenseeId = licenseeSession.LicenseeId,
                                SportId = 0,
                                UserId = SessionService.UserSession.UserId,
                                FromDate = metaModel.FromDate,
                                ToDate = metaModel.ToDate
                            });                
            
            var firstLeague = leagues.FirstOrDefault();

            var matches = firstLeague == null ? new List<Match>()
                            : this._matchService.GetMatchesByLeague(firstLeague.LeagueId, metaModel.FromDate, metaModel.ToDate); 
            var viewModel = new WinLossByLeagueViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                CurrencyList = SessionService.UserSession.CurrencyList,
            };
            viewModel.Sports = sports.ToDictionary(x => x.ID, y => y.Name);
            viewModel.Leagues = leagues.ToDictionary(x => x.LeagueId, y => y.LeagueName);
            viewModel.Matches = matches.ToDictionary(x => x.MatchId, y => y.MatchName);
            
            return View("~/Views/Report/WinLossByLeague/Main.cshtml", viewModel);
        }
    }
}