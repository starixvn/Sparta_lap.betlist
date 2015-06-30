// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.NumberGame.cs" company="Nexcel Solutions Vietnam">
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
    /// Number game.
    /// </summary>
    public partial class TotalBetsController
    {
        /// <summary>
        /// Number-game page.
        /// </summary>
        /// <returns>The report page.</returns>
        [FunctionAuthorization]
        public ActionResult NumberGame()
        {
            var param = new NumberGameMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.NumberGameView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult NumberGame(NumberGameMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.NumberGameView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of Number game.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult NumberGameView(NumberGameMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                NumberGame totalData;
                var dataList = _numberGameService.GetNumberGameDataList(metaModel, out totalData);
                var model = new NumberGameViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencyList,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/NumberGame/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}