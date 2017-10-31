// --------------------------------------------------------------------------
// <copyright file="OutrightTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetOutright)]
    public class OutrightTotalBetController : TotalBetBaseController
    {
        private readonly IOutrightDataAccess _outrightDataAccess;

        public OutrightTotalBetController()
        {
            _outrightDataAccess = new OutrightDataAccess();
        }

        public OutrightTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFileSystem fileSystem,
            IOutrightDataAccess outrightDataAccess)
            : base(languageService, sessionService, appSetting, fileSystem)
        {
            _outrightDataAccess = outrightDataAccess;
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
                var dataList = _outrightDataAccess.GetOutrightDataList(metaModel);
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