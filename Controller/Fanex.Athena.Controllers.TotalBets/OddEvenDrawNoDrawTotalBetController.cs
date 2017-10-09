// --------------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.TotalBet.DTO;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.TotalBetOddEven1x2DND)]
    public class OddEvenDrawNoDrawTotalBetController : TotalBetBaseController
    {
        private readonly IOddEvenDrawNoDrawDataAccess _oddEvenDrawNoDrawDataAccess;

        public OddEvenDrawNoDrawTotalBetController()
        {
            _oddEvenDrawNoDrawDataAccess = new OddEvenDrawNoDrawDataAccess();
        }

        public OddEvenDrawNoDrawTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IOddEvenDrawNoDrawDataAccess oddEvenDrawNoDrawDataAccess)
            : base(languageService, sessionService, appSetting)
        {
            _oddEvenDrawNoDrawDataAccess = oddEvenDrawNoDrawDataAccess;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new OddEvenDrawNoDrawMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);
            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(OddEvenDrawNoDrawMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        #region Utility

        private ActionResult GetView(OddEvenDrawNoDrawMetaModel metaModel)
        {
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                OddEvenDrawNoDraw totalData;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var dataList = _oddEvenDrawNoDrawDataAccess.GetOddEvenDrawNoDrawDataList(metaModel, out totalData);
                var model = new OddEvenDrawNoDrawViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/OddEvenDrawNoDraw/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }

        #endregion Utility
    }
}