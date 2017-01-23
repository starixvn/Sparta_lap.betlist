// --------------------------------------------------------------------------
// <copyright file="MoneyLineTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetMoneyLine)]
    public class MoneyLineTotalBetController : TotalBetBaseController
    {
        private readonly IMoneyLineService _moneyLineService;

        public MoneyLineTotalBetController()
        {
            _moneyLineService = new MoneyLineService();
        }

        public MoneyLineTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IMoneyLineService moneyLineService)
            : base(languageService, sessionService, appSetting)
        {
            _moneyLineService = moneyLineService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new MoneyLineMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(MoneyLineMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(MoneyLineMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.UserId = userSession.UserId;
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                MoneyLine totalData;
                var dataList = _moneyLineService.GetMoneyLineDataList(metaModel, out totalData);

                var model = new MoneyLineViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    CurrencyList = currencies,
                    TotalData = totalData
                };
                return View("~/Views/TotalBets/MoneyLine/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}