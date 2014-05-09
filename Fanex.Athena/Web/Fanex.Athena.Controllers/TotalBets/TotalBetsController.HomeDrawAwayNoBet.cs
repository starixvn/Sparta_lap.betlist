// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.HomeDrawAwayNoBet.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.TotalBets;
    using Models.ViewModel.TotalBets;

    /// <summary>
    /// Home/Draw/Away No Bet.
    /// </summary>
    public partial class TotalBetsController
    {
        /// <summary>
        /// Home-Draw-Away-No-bet page.
        /// </summary>
        /// <returns>The report page.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult HomeDrawAwayNoBet()
        {
            var param = new HomeDrawAwayNoBetMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);
            return this.HomwDrawAwayNoBetView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult HomeDrawAwayNoBet(HomeDrawAwayNoBetMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.HomwDrawAwayNoBetView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of HomeDrawAwayNoBet.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult HomwDrawAwayNoBetView(HomeDrawAwayNoBetMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _homeDrawAwayNoBetService.GetHomeDrawAwayNoBetDataList(metaModel);
                var model = new HomeDrawAwayNoBetViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList
                };

                return View("~/Views/TotalBets/HomeDrawAwayNoBet/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}