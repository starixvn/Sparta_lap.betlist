// --------------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawTotalBetController.cs" company="Nexcel Solutions Vietnam">
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
    using Service.TotalBet;
    using Service.TotalBet.Interfaces;

    public class OddEvenDrawNoDrawTotalBetController : TotalBetBaseController
    {
        private readonly IOddEvenDrawNoDrawService _oddEvenDrawNoDrawService;

        public OddEvenDrawNoDrawTotalBetController()
        {
            _oddEvenDrawNoDrawService = new OddEvenDrawNoDrawService();
        }

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var param = new OddEvenDrawNoDrawMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);
            return this.GetView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        [CustomValidateAntiForgery]
        public ActionResult Index(OddEvenDrawNoDrawMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        #region Utility

        /// <summary>
        /// Get the view of OddEvenDrawNoDraw.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>The action result.</returns>
        private ActionResult GetView(OddEvenDrawNoDrawMetaModel metaModel)
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