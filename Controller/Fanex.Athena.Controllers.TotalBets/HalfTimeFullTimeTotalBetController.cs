// --------------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetHalfTimeFullTime)]
    public class HalfTimeFullTimeTotalBetController : TotalBetBaseController
    {
        private readonly IHalfTimeFullTimeService _halfTimeFullTimeService;

        public HalfTimeFullTimeTotalBetController()
        {
            _halfTimeFullTimeService = new HalfTimeFullTimeService();
        }

        public HalfTimeFullTimeTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IHalfTimeFullTimeService halfTimeFullTimeService)
            : base(languageService, sessionService, appSetting)
        {
            _halfTimeFullTimeService = halfTimeFullTimeService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new HalfTimeFullTimeMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(HalfTimeFullTimeMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(HalfTimeFullTimeMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.UserId = userSession.UserId;
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                HalfTimeFullTime totalData;
                var dataList = _halfTimeFullTimeService.GetHalfTimeFullTimeDataList(metaModel, out totalData);
                var model = new HalfTimeFullTimeViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies,
                    TotalRunningStake = totalData.HomeHome + totalData.HomeDraw + totalData.HomeAway + totalData.DrawHome +
                                        totalData.DrawDraw + totalData.DrawAway + totalData.AwayHome + totalData.AwayAway
                };
                return View("~/Views/TotalBets/HalfTimeFullTime/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}