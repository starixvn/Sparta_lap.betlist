// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn };

        public CustomerWinLossBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, clientViewHelper, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            return ProcessDefaultRequest(CreateDefaultMetaModel(metaModel), "~/Views/BetList/CustomerWl/Main.cshtml", "Lrf_BetList_SelWinLossByDate_Mem");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            var title = GetCustomerBetListTitle(metaModel, GetViewInCurrency(metaModel));

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColumns, title, "CustomerWinLossBetList");
        }
    }
}