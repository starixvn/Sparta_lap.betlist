// --------------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalTotalBetController.cs" company="Nexcel Solutions Vietnam">
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

    public class FirstGoalLastGoalTotalBetController : TotalBetBaseController
    {
        private readonly IFirstGoalLastGoalService _firstGoalLastGoalService;

        public FirstGoalLastGoalTotalBetController()
        {
            _firstGoalLastGoalService = new FirstGoalLastGoalService();
        }

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var param = new FirstGoalLastGoalMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
            };

            SetConvetedUSDCurrencyName(param);
            return this.GetView(param);
        }

        [HttpPost]
        [FunctionAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FirstGoalLastGoalMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(FirstGoalLastGoalMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencies = GetUserCurrencies();

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                FirstGoalLastGoal totalData;
                var dataList = _firstGoalLastGoalService.GetFirstGoalLastGoalDataList(metaModel, out totalData);

                var model = new FirstGoalLastGoalViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/FirstGoalLastGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}