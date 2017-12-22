// --------------------------------------------------------------------------
// <copyright file="MoneyLineTotalBetController.cs" company="">
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

    [FunctionAuthorization(PermissionKeys.TotalBetMoneyLine)]
    public class MoneyLineTotalBetController : TotalBetBaseController
    {
        private readonly IMoneyLineDataAccess _moneyLineDataAccess;

        public MoneyLineTotalBetController()
        {
            _moneyLineDataAccess = new MoneyLineDataAccess();
        }

        public MoneyLineTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IMoneyLineDataAccess moneyLineDataAccess)
            : base(languageService, sessionService, appSetting)
        {
            _moneyLineDataAccess = moneyLineDataAccess;
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
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                MoneyLine totalData;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var dataList = _moneyLineDataAccess.GetMoneyLineDataList(metaModel, out totalData);

                var model = new MoneyLineViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies,
                    TotalData = totalData
                };
                return View("~/Views/TotalBets/MoneyLine/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}