// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.Racing.cs" company="Nexcel Solutions Vietnam">
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
    /// Horse Racing.
    /// </summary>
    public partial class TotalBetsController
    {
        [FunctionAuthorization]
        public ActionResult HorseRacing()
        {
            var param = new RacingMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.HorseRacingView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult HorseRacing(RacingMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.HorseRacingView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of Racing.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult HorseRacingView(RacingMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _racingService.GetRacingDataList(metaModel);
                var model = new RacingViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList
                };

                return View("~/Views/TotalBets/Racing/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}