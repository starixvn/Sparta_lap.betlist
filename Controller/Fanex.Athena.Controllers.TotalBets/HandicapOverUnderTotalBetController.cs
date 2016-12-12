// --------------------------------------------------------------------------
// <copyright file="HandicapOverUnderTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Linq;
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

    [FunctionAuthorization(PermissionKeys.TotalBetHandicapOverUnderLive)]
    public class HandicapOverUnderTotalBetController : TotalBetBaseController
    {
        private readonly IHandicapOverUnderService _handicapOverUnderService;

        public HandicapOverUnderTotalBetController()
        {
            _handicapOverUnderService = new HandicapOverUnderService();
        }

        public HandicapOverUnderTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IHandicapOverUnderService handicapOverUnderService)
            : base(languageService, sessionService, appSetting)
        {
            _handicapOverUnderService = handicapOverUnderService;
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
            metaModel.UserId = userSession.UserId;
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.Values.Contains(metaModel.CurrencyId))
            {
                HandicapOverUnder totalData;
                var dataList = _handicapOverUnderService.GetHandicapOverUnderDataList(metaModel, out totalData);

                var model = new HandicapOverUnderViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = licenseeSession.LicenseeType,
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