// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.HandicapOU.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.TotalBets;
    using Models.ViewModel.TotalBets;

    /// <summary>
    /// Handicap Over Under/Live.
    /// </summary>
    public partial class TotalBetsController
    {
        #region Action

        /// <summary>
        /// Handicap-Over-Under page.
        /// </summary>
        /// <returns>The report page.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult HandicapOverUnder()
        {
            var param = new HandicapOverUnderMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.HandicapOverUnderView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult HandicapOverUnder(HandicapOverUnderMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.HandicapOverUnderView(metaModel);
        }

        #endregion Action

        #region Utility

        /// <summary>
        /// Get the view of Handicap OU.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult HandicapOverUnderView(HandicapOverUnderMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _handicapOverUnderService.GetHandicapOverUnderDataList(metaModel);

                var model = new HandicapOverUnderViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList
                };
                /*set content view follow by licensee*/
                ViewBag.LicenseeView = "ContentData.cshtml";

                var items = Fanex.Athena.Extension.Utility.AppSettings.GetValue("LicenseeOfferNewForecast");

                if (!string.IsNullOrEmpty(items))
                {
                    var licenseeOfferNewForecasts = items.Split(',');
                    if (licenseeOfferNewForecasts.Contains(SessionService.LicenseeSession.LicenseeId.ToString()))
                    {
                        ViewBag.LicenseeView = "ContentDataBodog.cshtml";
                    }
                }

                return View("~/Views/TotalBets/HandicapOverUnder/Main.cshtml", model);
            }

            return Redirect("~/AccessDenied.aspx");
        }

        #endregion Utility
    }
}