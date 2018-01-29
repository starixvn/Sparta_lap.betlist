// -----------------------------------------------------------------------
// <copyright file="AllBetBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class AllBetBetListController : BetListBaseController
    {
        public AllBetBetListController()
        {
        }

        public AllBetBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(
            languageService, 
            sessionService, 
            appSetting, 
            clientViewHelper, 
            authorizationService, 
            productService, 
            betListService, 
            balanceUpDataAccess, 
            cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);

            if (!model.IsExportExcel)
            {
                ViewBag.BetListTitle = GetTitle(model.Date);
                return View("~/Views/BetList/AGCasinoDetail/Main.cshtml", model);
            }

            if (IsValidCurrency(model.CurrencyId))
            {
                return ExcelDetail(model);
            }

            return EmptyExcel();
        }

        private static string GetTitle(DateTime date)
        {
            return ViewResources.ProductAB + " - " + ViewResources.DetailedBetlist + " - " + date.ToString("MM/dd/yyyy");
        }

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_LC_Detail";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var licenseeSession = SessionService.LicenseeSession;
            var showingOption = new ShowingOption
            {
                GrandTotalVisible = false,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(showingOption, licenseeSession.LicenseeId, hiddenColumns);
            var title = GetTitle(metaModel.Date);
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "AllBetDetail");
        }
    }
}