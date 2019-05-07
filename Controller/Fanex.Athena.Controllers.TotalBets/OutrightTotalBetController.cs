// --------------------------------------------------------------------------
// <copyright file="OutrightTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetOutright)]
    public class OutrightTotalBetController : TotalBetBaseController
    {
        private readonly IOutrightDataAccess _outrightDataAccess;

        public OutrightTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IOutrightDataAccess outrightDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _outrightDataAccess = outrightDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var param = new OutrightMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return await this.GetView(param).ConfigureAwait(false);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(OutrightMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return await this.GetView(metaModel).ConfigureAwait(false);
        }

        private async Task<ActionResult> GetView(OutrightMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var dataList = await _outrightDataAccess.GetOutrightDataList(metaModel).ConfigureAwait(false);
                var model = new OutrightViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies
                };

                return View("~/Views/Desktop/TotalBets/Outright/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}