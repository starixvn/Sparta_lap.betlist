// -----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController : ReadonlyBaseController
    {
        #region Constant

        public const int PageSizeExcel = int.MaxValue - 1000;

        #endregion Constant

        #region Fields

        private readonly IBalanceUpService _balanceUpService;
        private readonly IProductService _productService;
        private readonly ISportTypeService _sportTypeService;
        private readonly ILeagueService _leagueService;
        private readonly IMatchService _matchService;
        private readonly IMoveBetService _moveBetService;

        #endregion Fields

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportController" /> class.
        /// </summary>
        public ReportController()
            : base()
        {
            this._balanceUpService = new BalanceUpService();
            this._productService = new ProductService();
            _sportTypeService = new SportTypeService();
            this._leagueService = new LeagueService();
            this._matchService = new MatchService();
            this._moveBetService = new MoveBetService();
        }

        [HttpGet]
        [AjaxRequest]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        public ActionResult GetSportsAndProducts()
        {
            return Json(
                _sportTypeService.GetSportsAndProducts(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                JsonRequestBehavior.AllowGet);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        [AjaxRequest]
        public ActionResult GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            return Json(
                _leagueService.GetLeaguesBySport(metaModel)
                .Select(x => new { Key = x.LeagueId, Value = x.LeagueName }));
        }

        [AjaxRequest]
        public ActionResult GetMatchesByLeague(int leagueid, DateTime fromDate, DateTime toDate)
        {
            var matches = _matchService.GetMatchesByLeague(leagueid, fromDate, toDate);
            matches.Insert(0, new Match() { MatchName = ViewResources.All, MatchId = 0 });

            return Json(matches.Select(x => new { Key = x.MatchId, Value = x.MatchName }));
        }

        private void AddNotificationMessageToViewBag()
        {
            var moveBet = _moveBetService.GetMoveBet();
            var now = DateTime.Now;
            var endDate = AppSettings.MoveBetDayEnd ?? moveBet.EstimatedTime;
            
            if (!endDate.HasValue || now > endDate)
            {
                return;
            }

            var startDate = AppSettings.MoveBetDayStart ?? moveBet.AvailableWinlostDate;
            var message = ViewResources.NotificationMessage;
            var startDateString = startDate.ToString("MM/dd/yyyy");
            var endDateString = endDate.Value.ToString("hh:mm:ss MM/dd/yyyy");
            ViewBag.NotificationMessage = string.Format(message, startDateString, endDateString);
        }
    }
}