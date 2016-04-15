// -----------------------------------------------------------------------
// <copyright file="BetListController.LastBetsMonitoring .cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.BetList;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
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
            this._sportTypeService = new SportTypeService();
            var metaModel = new BetListMetaModel { LicenseeId = SessionService.LicenseeSession.LicenseeId };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new LastBetsMonitoringViewModel
            {
                CurrencyList = GetUserCurrencies(),
                SportsList = GetSports(),
                IsOnlyShowAllCurrencies = SessionService.LicenseeSession.LicenseeId == Licensees.KeenOcean,
                Param = metaModel
            };

            return View("~/Views/BetList/LastBetsMonitoringNew/Main.cshtml", model);
        }

        private IEnumerable<SelectListItem> GetSports()
        {
            const string ParlayId = "999";
            var sports = this._sportTypeService.GetAllSports();
            var sportListItems = sports.Select(item => new SelectListItem { Value = item.Key.ToString(), Text = item.Value }).ToList();
            sportListItems.Insert(0, new SelectListItem { Value = "0", Text = ViewResources.All, Selected = true });
            sportListItems.Add(new SelectListItem { Value = ParlayId, Text = ViewResources.Parlay });

            return sportListItems;
        }
    }
}