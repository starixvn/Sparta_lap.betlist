// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.TotalGoal.cs" company="Nexcel Solutions Vietnam">
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
    /// Total Goal controller.
    /// </summary>
    public partial class TotalBetsController
    {
        [FunctionAuthorization]
        public ActionResult TotalGoal()
        {
            var param = new TotalGoalMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.TotalGoalView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult TotalGoal(TotalGoalMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.TotalGoalView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of Total Goal.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult TotalGoalView(TotalGoalMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencies = GetUserCurrencies(Licensees.KeenOcean);
            metaModel.UserId = userSession.UserId;

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                TotalGoal totalData;
                var dataList = _totalGoalService.GetTotalGoalDataList(metaModel, out totalData);
                var model = new TotalGoalViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/TotalGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}