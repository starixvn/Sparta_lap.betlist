// --------------------------------------------------------------------------
// <copyright file="OutrightTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.TotalBet;
    using Service.TotalBet.Interfaces;

    [FunctionAuthorization(PermissionKeys.TotalBetOutright)]
    public class OutrightTotalBetController : TotalBetBaseController
    {
        private readonly IOutrightService _outrightService;

        public OutrightTotalBetController()
        {
            _outrightService = new OutrightService();
        }

        public OutrightTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IOutrightService outrightService)
            : base(languageService, sessionService, appSetting)
        {
            _outrightService = outrightService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new OutrightMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(OutrightMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(OutrightMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var dataList = _outrightService.GetOutrightDataList(metaModel);
                var model = new OutrightViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies
                };

                return View("~/Views/TotalBets/Outright/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}