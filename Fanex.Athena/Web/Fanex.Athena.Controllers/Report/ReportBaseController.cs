namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Service.Interface.Configuration;
    using Models.DTO.Common;
    using Models.MetaModel.Report;
    using Models.ViewModel;
    using Service.Implement.Common;
    using Service.Implement.Configuration;
    using Service.Interface.Common;
    using Web.ActionFilter;

    public class ReportBaseController : BaseController
    {
        public const int PageSizeExcel = int.MaxValue - 1000;

        protected readonly IBalanceUpService BalanceUpService;
        protected readonly IProductService ProductService;
        protected readonly ISportTypeService SportTypeService;
        protected readonly ILeagueService LeagueService;
        protected readonly IMatchService MatchService;
        protected readonly IMoveBetService MoveBetService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportController" /> class.
        /// </summary>
        public ReportBaseController()
            : base()
        {
            this.BalanceUpService = new BalanceUpService();
            this.ProductService = new ProductService();
            SportTypeService = new SportTypeService();
            this.LeagueService = new LeagueService();
            this.MatchService = new MatchService();
            this.MoveBetService = new MoveBetService();
        }

        [HttpGet]
        [AjaxRequest]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        public ActionResult GetSportsAndProducts()
        {
            return Json(
                SportTypeService.GetSportsAndProducts(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                JsonRequestBehavior.AllowGet);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        [AjaxRequest]
        public ActionResult GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            return Json(
                LeagueService.GetLeaguesBySport(metaModel)
                .Select(x => new { Key = x.LeagueId, Value = x.LeagueName }));
        }

        [AjaxRequest]
        public ActionResult GetMatchesByLeague(int leagueid, DateTime fromDate, DateTime toDate)
        {
            var matches = MatchService.GetMatchesByLeague(leagueid, fromDate, toDate);
            matches.Insert(0, new Match() { MatchName = ViewResources.All, MatchId = 0 });

            return Json(matches.Select(x => new { Key = x.MatchId, Value = x.MatchName }));
        }

        protected void AddNotificationMessageToViewBag()
        {
            var moveBet = MoveBetService.GetMoveBet();

            if (!moveBet.EstimatedTime.HasValue || DateTime.Now > moveBet.EstimatedTime)
            {
                return;
            }

            var message = ViewResources.NotificationMessage;
            var startDateString = moveBet.AvailableWinlostDate.ToString("MM/dd/yyyy");
            var endDateString = moveBet.EstimatedTime.Value.ToString("HH:mm:ss MM/dd/yyyy");
            ViewBag.NotificationMessage = string.Format(message, startDateString, endDateString);
        }
    }
}