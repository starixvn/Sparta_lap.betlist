// --------------------------------------------------------------------------
// <copyright file="NumberGameTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using Fanex.Athena.Model.TotalBet.DTO;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.TotalBet;
    using Service.TotalBet.Interfaces;

    [FunctionAuthorization(PermissionKeys.TotalBetNumberGame)]
    public class NumberGameTotalBetController : TotalBetBaseController
    {
        private readonly INumberGameService _numberGameService;

        public NumberGameTotalBetController()
        {
            _numberGameService = new NumberGameService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new NumberGameMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(param);

            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(NumberGameMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(NumberGameMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                NumberGame totalData;
                var dataList = _numberGameService.GetNumberGameDataList(metaModel, out totalData);
                var model = new NumberGameViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = currencyList,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/NumberGame/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}