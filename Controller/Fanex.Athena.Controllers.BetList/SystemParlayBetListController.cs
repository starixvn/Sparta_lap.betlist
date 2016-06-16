// -----------------------------------------------------------------------
// <copyright file="SystemParlayBetListController.cs" company="Nexcel">
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
    using Web.ActionFilter;

    #endregion Using

    public class SystemParlayBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Detail(SystemParlayDetailMetaModel metaModel)
        {
            var balanceUpInfo = this.BalanceUpService.BalanceUpInfo;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var systemParlayData = this.BetListService.GetSystemParlayDetail(metaModel);

            var builder = new BetListHTMLBuilder();
            var systemParlayText = builder.BuildSystemParlayDetail(this.TicketHelper, systemParlayData);
            var viewModel = new SystemParlayDetailViewModel
            {
                DetailText = systemParlayText,
                Param = metaModel
            };

            return View("~/Views/BetList/SystemParlayDetail/Main.cshtml", viewModel);
        }
    }
}