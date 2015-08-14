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
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.TotalBets;
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
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencies = GetUserCurrencies(Licensees.KeenOcean);

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                FirstGoalLastGoal totalData;
                var dataList = _firstGoalLastGoalService.GetFirstGoalLastGoalDataList(metaModel, out totalData);

                var model = new FirstGoalLastGoalViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/FirstGoalLastGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}