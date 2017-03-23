// --------------------------------------------------------------------------
// <copyright file="TotalGoalTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetTotalGoal)]
    public class TotalGoalTotalBetController : TotalBetBaseController
    {
        private readonly ITotalGoalService _totalGoalService;

        public TotalGoalTotalBetController()
        {
            _totalGoalService = new TotalGoalService();
        }

        public TotalGoalTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ITotalGoalService totalGoalService)
            : base(languageService, sessionService, appSetting)
        {
            _totalGoalService = totalGoalService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new TotalGoalMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(TotalGoalMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        #region Utility

        private ActionResult GetView(TotalGoalMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                TotalGoal totalData;
                var dataList = _totalGoalService.GetTotalGoalDataList(metaModel, out totalData);
                var model = new TotalGoalViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/TotalGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}