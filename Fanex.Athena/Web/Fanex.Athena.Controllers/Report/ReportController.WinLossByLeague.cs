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
    using Fanex.Athena.Models.MetaModel.Product;
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
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;
            var productAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            var productList = productAccess.ProductList.Where(x => x.ProductCode == Product.Sb || x.ProductCode == Product.Ba);
            var metaModel = new WinLossByLeagueMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId,
                FromDate = new DateTime(date.Year, date.Month, 1),
                ToDate = date,
                ProductOptions = productList.Select(i => new ProductOption
                {
                    Code = i.ProductCode,
                    Name = i.ProductName,
                    Status = true
                }).ToList(),
            };
            
            SetConvetedUSDCurrencyName(metaModel);
            SetConvetedUSDCurrencyId(metaModel);
            var sports = this._sportTypeService.GetSportsAndProducts(licenseeSession.LicenseeId, userSession.Language, SPORTSBOOK);
            var leagues = this._leagueService.GetLeaguesBySport(
                            new LeagueMetaModel()
                            {
                                LicenseeId = licenseeSession.LicenseeId,
                                UserId = userSession.UserId,
                                FromDate = metaModel.FromDate,
                                ToDate = metaModel.ToDate
                            });                
            
            var firstLeague = leagues.FirstOrDefault();

            var matches = firstLeague == null ? new List<Match>()
                            : this._matchService.GetMatchesByLeague(firstLeague.LeagueId, metaModel.FromDate, metaModel.ToDate); 
            var viewModel = new WinLossByLeagueViewModel
            {                
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess
            };

            viewModel.Sports = sports.ToDictionary(x => x.ID, y => y.Name);
            viewModel.Leagues = leagues.ToDictionary(x => x.LeagueId, y => y.LeagueName);
            viewModel.Matches = matches.ToDictionary(x => x.MatchId, y => y.MatchName);
            
            return View("~/Views/Report/WinLossByLeague/Main.cshtml", viewModel);
        }
    }
}