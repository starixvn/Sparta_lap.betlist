// --------------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
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

    [FunctionAuthorization(PermissionKeys.TotalBetHomeDrawAwayNoBet)]
    public class HomeDrawAwayNoBetTotalBetController : TotalBetBaseController
    {
        private readonly IHomeDrawAwayNoBetDataAccess _homeDrawAwayNoBetDataAccess;

        public HomeDrawAwayNoBetTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IHomeDrawAwayNoBetDataAccess homeDrawAwayNoBetDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _homeDrawAwayNoBetDataAccess = homeDrawAwayNoBetDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var param = new HomeDrawAwayNoBetMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);
            return await this.GetView(param).ConfigureAwait(false);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(HomeDrawAwayNoBetMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return await this.GetView(metaModel).ConfigureAwait(false);
        }

        private async Task<ActionResult> GetView(HomeDrawAwayNoBetMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var dataList = await _homeDrawAwayNoBetDataAccess.GetHomeDrawAwayNoBetDataList(metaModel).ConfigureAwait(false);
                var model = new HomeDrawAwayNoBetViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies
                };

                return View("~/Views/Desktop/TotalBets/HomeDrawAwayNoBet/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}