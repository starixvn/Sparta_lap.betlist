// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.FirstGoalLastGoal.cs" company="Nexcel Solutions Vietnam">
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
    /// First Goal/Last Goal.
    /// </summary>
    public partial class TotalBetsController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult FirstGoalLastGoal()
        {
            var param = new FirstGoalLastGoalMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
            };

            SetConvetedUSDCurrencyName(param);
            return this.FirstGoalLastGoalView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult FirstGoalLastGoal(FirstGoalLastGoalMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.FirstGoalLastGoalView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of FGLG.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        private ActionResult FirstGoalLastGoalView(FirstGoalLastGoalMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _firstGoalLastGoalService.GetFirstGoalLastGoalDataList(metaModel);

                var model = new FirstGoalLastGoalViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList
                };

                return View("~/Views/TotalBets/FirstGoalLastGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}