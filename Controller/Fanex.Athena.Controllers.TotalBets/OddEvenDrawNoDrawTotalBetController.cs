// --------------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.TotalBet.DTO;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.TotalBet;
    using Service.TotalBet.Interfaces;

    [FunctionAuthorization(PermissionKeys.TotalBetOddEven1x2DND)]
    public class OddEvenDrawNoDrawTotalBetController : TotalBetBaseController
    {
        private readonly IOddEvenDrawNoDrawService _oddEvenDrawNoDrawService;

        public OddEvenDrawNoDrawTotalBetController()
        {
            _oddEvenDrawNoDrawService = new OddEvenDrawNoDrawService();
        }

        public OddEvenDrawNoDrawTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IOddEvenDrawNoDrawService oddEvenDrawNoDrawService)
            : base(languageService, sessionService, appSetting)
        {
            _oddEvenDrawNoDrawService = oddEvenDrawNoDrawService;
        }

        [HttpGet]
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
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.UserId = userSession.UserId;
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                OddEvenDrawNoDraw totalData;
                var dataList = _oddEvenDrawNoDrawService.GetOddEvenDrawNoDrawDataList(metaModel, out totalData);
                var model = new OddEvenDrawNoDrawViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
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