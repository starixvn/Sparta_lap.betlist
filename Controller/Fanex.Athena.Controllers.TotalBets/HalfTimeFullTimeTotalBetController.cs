// --------------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeTotalBetController.cs" company="">
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
    using Fanex.Athena.Model.TotalBet.DTO;
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

        public HalfTimeFullTimeTotalBetController()
        {
            _halfTimeFullTimeDataAccess = new HalfTimeFullTimeDataAccess();
        }

        public HalfTimeFullTimeTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFileSystem fileSystem,
            IHalfTimeFullTimeDataAccess halfTimeFullTimeDataAccess)
            : base(languageService, sessionService, appSetting, fileSystem)
        {
            _halfTimeFullTimeDataAccess = halfTimeFullTimeDataAccess;
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
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                HalfTimeFullTime totalData;
                var dataList = _halfTimeFullTimeDataAccess.GetHalfTimeFullTimeDataList(metaModel, out totalData);
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