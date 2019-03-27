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
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.MetaModel.Common;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportWinLossByLeague)]
    public class WinLossByLeagueReportController : ReportBaseController
    {
        private readonly ISportTypeDataAccess _sportTypeDataAccess;
        private readonly ILeagueDataAccess _leagueDataAccess;
        private readonly IMatchService _matchService;

#pragma warning disable S107 // Methods should not have too many parameters

        public WinLossByLeagueReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            ISportTypeDataAccess sportTypeDataAccess,
            ILeagueDataAccess leagueDataAccess,
            IMatchService matchService)
            : base(languageService, sessionService, appSetting, productService)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _sportTypeDataAccess = sportTypeDataAccess;
            _leagueDataAccess = leagueDataAccess;
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
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
                ProductOptions = ToProductOptions(productList)
            };

            SetConvetedUSDCurrencyName(metaModel);
            SetConvetedUSDCurrencyId(metaModel);
            var sports = this._sportTypeDataAccess.GetSportsAndProducts(licenseeSession.LicenseeId, userSession.Language, Product.SB)
                .Where(x => x.ParentId >= 0);
            var leagues = await this._leagueDataAccess.GetLeaguesBySport(
                            new LeagueMetaModel
                            {
                                LicenseeId = licenseeSession.LicenseeId,
                                UserId = userSession.UserId,
                                FromDate = metaModel.FromDate,
                                ToDate = metaModel.ToDate,
                                AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId)
                            }).ConfigureAwait(false);

            var firstLeague = leagues.FirstOrDefault();

            var matches = firstLeague == null ? new List<Match>()
                            : await this._matchService.GetMatchesByLeague(firstLeague.LeagueId, metaModel.FromDate, metaModel.ToDate).ConfigureAwait(false);
            var viewModel = new WinLossByLeagueViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
                Sports = sports.ToDictionary(x => x.ID, y => y.Name),
                Leagues = leagues.ToDictionary(x => x.LeagueId, y => y.LeagueName),
                Matches = matches.ToDictionary(x => x.MatchId, y => y.MatchName),
                HasAmountDueToColumn = AppSettings.HasAmountDueToColumn
            };

            return View("~/Views/Desktop/Report/WinLossByLeague/Main.cshtml", viewModel);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var leagues = (await _leagueDataAccess.GetLeaguesBySport(metaModel).ConfigureAwait(false))
                            .Select(x => new { Key = x.LeagueId, Value = x.LeagueName });
            return Json(leagues);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> GetMatchesByLeague(int leagueid, DateTime fromDate, DateTime toDate)
        {
            var matches = await _matchService.GetMatchesByLeague(leagueid, fromDate, toDate).ConfigureAwait(false);
            matches.Insert(0, new Match { MatchName = ViewResources.All, MatchId = 0 });

            return Json(matches.Select(x => new { Key = x.MatchId, Value = x.MatchName }));
        }
    }
}