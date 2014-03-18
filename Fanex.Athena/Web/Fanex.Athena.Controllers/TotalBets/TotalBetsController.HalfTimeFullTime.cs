// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.HalfTimeFullTime.cs" company="Nexcel Solutions Vietnam">
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
    /// Half time/Full time.
    /// </summary>
    public partial class TotalBetsController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult HalfTimeFullTime()
        {
            var param = new HalfTimeFullTimeMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.HalfTimeFullTimeView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult HalfTimeFullTime(HalfTimeFullTimeMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.HalfTimeFullTimeView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of HTFL.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult HalfTimeFullTimeView(HalfTimeFullTimeMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _halfTimeFullTimeService.GetHalfTimeFullTimeDataList(metaModel);
                var model = new HalfTimeFullTimeViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList
                };
                return View("~/Views/TotalBets/HalfTimeFullTime/Main.cshtml", model);
            }

            return Redirect("~/AccessDenied.aspx");
        }

        #endregion Utility
    }
}