// --------------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetTotalBetController.cs" company="Nexcel Solutions Vietnam">
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
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.TotalBet;
    using Service.TotalBet.Interfaces;

    public class HomeDrawAwayNoBetTotalBetController : TotalBetBaseController
    {
        private readonly IHomeDrawAwayNoBetService _homeDrawAwayNoBetService;

        public HomeDrawAwayNoBetTotalBetController()
        {
            _homeDrawAwayNoBetService = new HomeDrawAwayNoBetService();
        }

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var param = new HomeDrawAwayNoBetMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);
            return this.GetView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult Index(HomeDrawAwayNoBetMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(HomeDrawAwayNoBetMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencies = GetUserCurrencies();

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                var dataList = _homeDrawAwayNoBetService.GetHomeDrawAwayNoBetDataList(metaModel);
                var model = new HomeDrawAwayNoBetViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencies
                };

                return View("~/Views/TotalBets/HomeDrawAwayNoBet/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}