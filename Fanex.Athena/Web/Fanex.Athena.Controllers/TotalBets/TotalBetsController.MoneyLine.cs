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
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.TotalBets;
    using Models.ViewModel.TotalBets;

    /// <summary>
    /// Money line controller.
    /// </summary>
    public partial class TotalBetsController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult MoneyLine()
        {
            var param = new MoneyLineMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };
            param.BaseCurrency = param.BaseUsdLicensees.Contains(param.LicenseeId.ToString()) ? "usd" : "eur";

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
            metaModel.UserId = SessionService.UserSession.UserId;
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _moneyLineService.GetMoneyLineDataList(metaModel);

                var model = new MoneyLineViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList
                };
                return View("~/Views/TotalBets/MoneyLine/Main.cshtml", model);
            }

            return Redirect("~/AccessDenied.aspx");
        }

        #endregion Utility
    }
}