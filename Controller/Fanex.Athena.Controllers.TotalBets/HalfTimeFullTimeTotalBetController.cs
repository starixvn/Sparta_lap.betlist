// --------------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetHalfTimeFullTime)]
    public class HalfTimeFullTimeTotalBetController : TotalBetBaseController
    {
        private readonly IHalfTimeFullTimeDataAccess _halfTimeFullTimeDataAccess;

        public HalfTimeFullTimeTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IHalfTimeFullTimeDataAccess halfTimeFullTimeDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _halfTimeFullTimeDataAccess = halfTimeFullTimeDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var param = new HalfTimeFullTimeMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return await this.GetView(param).ConfigureAwait(false);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(HalfTimeFullTimeMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return await this.GetView(metaModel).ConfigureAwait(false);
        }

        private async Task<ActionResult> GetView(HalfTimeFullTimeMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var result = await _halfTimeFullTimeDataAccess.GetHalfTimeFullTimeDataList(metaModel).ConfigureAwait(false);
                var totalData = result.TotalData;
                var model = new HalfTimeFullTimeViewModel
                {
                    Param = metaModel,
                    MainData = result.DataList.ToList(),
                    CurrencyList = currencies,
                    TotalRunningStake = totalData.HomeHome + totalData.HomeDraw + totalData.HomeAway + totalData.DrawHome +
                                        totalData.DrawDraw + totalData.DrawAway + totalData.AwayHome + totalData.AwayAway
                };
                return View("~/Views/Desktop/TotalBets/HalfTimeFullTime/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}