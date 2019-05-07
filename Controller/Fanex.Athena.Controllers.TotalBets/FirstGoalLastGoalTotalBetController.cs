// --------------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.TotalBet.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.TotalBetFirstGoalLastGoal)]
    public class FirstGoalLastGoalTotalBetController : TotalBetBaseController
    {
        private readonly IFirstGoalLastGoalDataAccess _firstGoalLastGoalDataAccess;

        public FirstGoalLastGoalTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFirstGoalLastGoalDataAccess firstGoalLastGoalDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _firstGoalLastGoalDataAccess = firstGoalLastGoalDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var param = new FirstGoalLastGoalMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
            };

            SetConvetedUSDCurrencyName(param);
            return await this.GetView(param).ConfigureAwait(false);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(FirstGoalLastGoalMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return await this.GetView(metaModel).ConfigureAwait(false);
        }

        private async Task<ActionResult> GetView(FirstGoalLastGoalMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var result = await _firstGoalLastGoalDataAccess.GetFirstGoalLastGoalDataList(metaModel).ConfigureAwait(false);

                var model = new FirstGoalLastGoalViewModel
                {
                    Param = metaModel,
                    MainData = result.DataList.ToList(),
                    CurrencyList = currencies,
                    TotalData = result.TotalData
                };

                return View("~/Views/Desktop/TotalBets/FirstGoalLastGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}