// -----------------------------------------------------------------------
// <copyright file="BetListController.SystemParlayDetail.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel.BetList;
    using Fanex.BetList.Core.Builder;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for System Parlay Detail.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult SystemParlayDetail(SystemParlayDetailMetaModel metaModel)
        {
            var balanceUpInfo = this._balanceUpService.BalanceUpInfo;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var systemParlayData = this._betListService.GetSystemParlayDetail(metaModel);

            var builder = new BetListHTMLBuilder();
            var systemParlayText = builder.BuildSystemParlayDetail(this._ticketHelper, systemParlayData);
            var viewModel = new SystemParlayDetailViewModel
            {
                DetailText = systemParlayText,
                Param = metaModel
            };

            return View("~/Views/BetList/SystemParlayDetail/Main.cshtml", viewModel);
        }
    }
}