// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.OddEvenDrawNoDraw.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.TotalBets;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.TotalBets;
    using Models.ViewModel.TotalBets;

    /// <summary>
    /// Odd/Even + 1X2 + Draw/NoDraw.
    /// </summary>
    public partial class TotalBetsController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult OddEvenDrawNoDraw()
        {
            var param = new OddEvenDrawNoDrawMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);
            return this.OddEvenDrawNoDrawView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult OddEvenDrawNoDraw(OddEvenDrawNoDrawMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.OddEvenDrawNoDrawView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of OddEvenDrawNoDraw.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult OddEvenDrawNoDrawView(OddEvenDrawNoDrawMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencies = GetUserCurrencies();

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                OddEvenDrawNoDraw totalData;
                var dataList = _oddEvenDrawNoDrawService.GetOddEvenDrawNoDrawDataList(metaModel, out totalData);
                var model = new OddEvenDrawNoDrawViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/OddEvenDrawNoDraw/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}