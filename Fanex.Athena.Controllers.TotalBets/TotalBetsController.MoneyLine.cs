// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.MoneyLine.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using Fanex.Athena.Model.TotalBet.DTO;
    using Fanex.Athena.Web.ActionFilter;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;

    /// <summary>
    /// Money line controller.
    /// </summary>
    public partial class TotalBetsController
    {
        /// <summary>
        /// Money-line pate.
        /// </summary>
        /// <returns>The report page.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult MoneyLine()
        {
            var param = new MoneyLineMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.MoneyLineView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult MoneyLine(MoneyLineMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.MoneyLineView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of MoneyLine.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult MoneyLineView(MoneyLineMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencies = GetUserCurrencies();

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                MoneyLine totalData;
                var dataList = _moneyLineService.GetMoneyLineDataList(metaModel, out totalData);

                var model = new MoneyLineViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencies,
                    TotalData = totalData
                };
                return View("~/Views/TotalBets/MoneyLine/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}