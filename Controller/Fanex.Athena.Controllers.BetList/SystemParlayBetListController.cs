// -----------------------------------------------------------------------
// <copyright file="SystemParlayBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Web.ActionFilter;

    #endregion Using

    public class SystemParlayBetListController : BetListBaseController
    {
        private readonly ITicketHelper _ticketHelper;
        private readonly BetListHTMLBuilder _betListBuilder;

        public SystemParlayBetListController()
        {
            this._ticketHelper = new TicketHelper();
            _betListBuilder = new BetListHTMLBuilder();
        }

        [HttpGet]
        public ActionResult Detail(SystemParlayDetailMetaModel metaModel)
        {
            var balanceUpInfo = this.BalanceUpService.BalanceUpInfo;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var systemParlayData = this.BetListService.GetSystemParlayDetail(metaModel);

            var systemParlayText = _betListBuilder.BuildSystemParlayDetail(this._ticketHelper, systemParlayData);
            var viewModel = new SystemParlayDetailViewModel
            {
                DetailText = systemParlayText
            };

            return View("~/Views/BetList/SystemParlayDetail/Main.cshtml", viewModel);
        }
    }
}