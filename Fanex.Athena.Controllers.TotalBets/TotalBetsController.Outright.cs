// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.Outright.cs" company="Nexcel Solutions Vietnam">
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
    /// Outright controller.
    /// </summary>
    public partial class TotalBetsController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Outright()
        {
            var param = new OutrightMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.OutrightView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult Outright(OutrightMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.OutrightView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of Outright.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult OutrightView(OutrightMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencies = GetUserCurrencies();

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _outrightService.GetOutrightDataList(metaModel);
                var model = new OutrightViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencies
                };

                return View("~/Views/TotalBets/Outright/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}