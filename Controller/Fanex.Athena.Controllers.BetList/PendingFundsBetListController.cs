// -----------------------------------------------------------------------
// <copyright file="PendingFundsBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportCustomerPendingFunds)]
    public class PendingFundsBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColList =
            new[] { ExternalIdColumn, SystemIdColumn, WinLostColumn, BetEffectiveColumn, LicenseeWLColumn };

        public PendingFundsBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            model.FromDate = DateTime.Today.AddYears(-10);
            model.ToDate = DateTime.Today.AddYears(10);
            return ProcessDefaultRequest(model, "~/Views/Desktop/BetList/PendingFunds/Main.cshtml", "Lrf_BetList_SelRunningByDate_Mem");
        }

        protected override bool IsValidExportExcel(BetListMetaModel model)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId);
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var title = "Running " + ViewResources.BetList
                + string.Format(
                " - {0}{1} - {2}",
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                GetViewInCurrency(metaModel));

            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColList, title, "CustomerPendingFundsBetList");
        }
    }
}