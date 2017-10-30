// --------------------------------------------------------------------------
// <copyright file="HandicapOverUnderTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Linq;
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

    [FunctionAuthorization(PermissionKeys.TotalBetHandicapOverUnderLive)]
    public class HandicapOverUnderTotalBetController : TotalBetBaseController
    {
        private readonly IHandicapOverUnderDataAccess _handicapOverUnderDataAccess;

        public HandicapOverUnderTotalBetController()
        {
            _handicapOverUnderDataAccess = new HandicapOverUnderDataAccess();
        }

        public HandicapOverUnderTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFileSystem fileSystem,
            IHandicapOverUnderDataAccess handicapOverUnderDataAccess)
            : base(languageService, sessionService, appSetting, fileSystem)
        {
            _handicapOverUnderDataAccess = handicapOverUnderDataAccess;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new HandicapOverUnderMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(HandicapOverUnderMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(HandicapOverUnderMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                HandicapOverUnder totalData;
                var dataList = _handicapOverUnderDataAccess.GetHandicapOverUnderDataList(metaModel, out totalData);

                var model = new HandicapOverUnderViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies,
                    TotalData = totalData
                };
                /*set content view follow by licensee*/
                ViewBag.LicenseeView = "ContentData.cshtml";

                var items = AppSettings.GetValue("LicenseeOfferNewForecast");

                if (!string.IsNullOrEmpty(items))
                {
                    var licenseeOfferNewForecasts = items.Split(',');
                    if (licenseeOfferNewForecasts.Contains(licenseeSession.LicenseeId.ToString()))
                    {
                        ViewBag.LicenseeView = "ContentDataBodog.cshtml";
                    }
                }

                return View("~/Views/TotalBets/HandicapOverUnder/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}