// -----------------------------------------------------------------------
// <copyright file="ReportController.WinLossByLeague.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Resources;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    public class WinLossByLeagueReportController : ReportBaseController
    {
        private readonly ISportTypeService _sportTypeService;
        private readonly ILeagueService _leagueService;
        private readonly IMatchService _matchService;

        public WinLossByLeagueReportController()
        {
            _sportTypeService = new SportTypeService();
            _leagueService = new LeagueService();
            _matchService = new MatchService();
        }

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;
            var productAccess = this.ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            var productList = productAccess.ProductList.Where(x => x.ProductCode == Product.SB || x.ProductCode == Product.BA);
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
            var sports = this._sportTypeService.GetSportsAndProducts(licenseeSession.LicenseeId, userSession.Language, ProductTypes.Sportsbook);
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

        [HttpPost]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        [AjaxRequest]
        [FunctionAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            return Json(
                _leagueService.GetLeaguesBySport(metaModel)
                .Select(x => new { Key = x.LeagueId, Value = x.LeagueName }));
        }

        [HttpPost]
        [AjaxRequest]
        [FunctionAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult GetMatchesByLeague(int leagueid, DateTime fromDate, DateTime toDate)
        {
            var matches = _matchService.GetMatchesByLeague(leagueid, fromDate, toDate);
            matches.Insert(0, new Match() { MatchName = ViewResources.All, MatchId = 0 });

            return Json(matches.Select(x => new { Key = x.MatchId, Value = x.MatchName }));
        }
    }
}