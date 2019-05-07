// --------------------------------------------------------------------------
// <copyright file="HandicapOverUnderTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetHandicapOverUnderLive)]
    public class HandicapOverUnderTotalBetController : TotalBetBaseController
    {
        private readonly IHandicapOverUnderDataAccess _handicapOverUnderDataAccess;

        public HandicapOverUnderTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IHandicapOverUnderDataAccess handicapOverUnderDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _handicapOverUnderDataAccess = handicapOverUnderDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var param = new HandicapOverUnderMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return await this.GetView(param).ConfigureAwait(false);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(HandicapOverUnderMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return await this.GetView(metaModel).ConfigureAwait(false);
        }

        private async Task<ActionResult> GetView(HandicapOverUnderMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var result = await _handicapOverUnderDataAccess.GetHandicapOverUnderDataList(metaModel).ConfigureAwait(false);

                var model = new HandicapOverUnderViewModel
                {
                    Param = metaModel,
                    MainData = result.DataList.ToList(),
                    CurrencyList = currencies,
                    TotalData = result.TotalData
                };

                ViewBag.LicenseeView = AppSettings.LicenseeOfferNewForecast.Contains(licenseeSession.LicenseeId) ?
                    "ContentDataBodog.cshtml" :
                    "ContentData.cshtml";

                return View("~/Views/Desktop/TotalBets/HandicapOverUnder/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}