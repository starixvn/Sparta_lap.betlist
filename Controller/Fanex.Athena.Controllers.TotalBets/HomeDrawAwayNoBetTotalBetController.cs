// --------------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
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

        public HomeDrawAwayNoBetTotalBetController()
        {
            _homeDrawAwayNoBetDataAccess = new HomeDrawAwayNoBetDataAccess();
        }

        public HomeDrawAwayNoBetTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFileSystem fileSystem,
            IHomeDrawAwayNoBetDataAccess homeDrawAwayNoBetDataAccess)
            : base(languageService, sessionService, appSetting, fileSystem)
        {
            _homeDrawAwayNoBetDataAccess = homeDrawAwayNoBetDataAccess;
        }

        [HttpGet]
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
        [CustomValidateAntiForgery]
        public ActionResult Index(HomeDrawAwayNoBetMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(HomeDrawAwayNoBetMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var dataList = _homeDrawAwayNoBetDataAccess.GetHomeDrawAwayNoBetDataList(metaModel);
                var model = new HomeDrawAwayNoBetViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies
                };

                return View("~/Views/TotalBets/HomeDrawAwayNoBet/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}