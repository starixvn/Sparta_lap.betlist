// -----------------------------------------------------------------------
// <copyright file="BetListController.LastBetsMonitoring .cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using Fanex.Athena.Models.ViewModel.BetList;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Web.ActionFilter;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        private ISportTypeService _sportTypeService;

        /// <summary>
        /// Action Get for last bets monitoring.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult LastBetsMonitoring()
        {
            this._sportTypeService = new SportTypeService(this._balanceUpService.BalanceUpInfo);
            var metaModel = new Models.MetaModel.BetList.BetListMetaModel { LicenseeId = SessionService.LicenseeSession.LicenseeId };
          
            SetConvetedUSDCurrencyName(metaModel);
           
            var model = new LastBetsMonitoringViewModel
            {
                CurrencyList = SessionService.UserSession.CurrencyList,
                SportsList = this._sportTypeService.GetAllSports(),
                Param = metaModel,
            };

            return View("~/Views/BetList/LastBetsMonitoringNew/Main.cshtml", model);
        }
    }
}