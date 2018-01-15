// -----------------------------------------------------------------------
// <copyright file="RacingCommissionBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportRacingCommission)]
    public class RacingCommissionBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            return ProcessDefaultRequest(metaModel, "~/Views/BetList/RacingCommission/Main.cshtml");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinLossRacingByDate_Mem";

            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            var currencyTitle = string.Format(
                ViewResources.Viewin,
                metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName);

            var title = ViewResources.Customer + " " + ViewResources.BetList + string.Format(
                " - {0} --> {1} - {2}{3} - {4}",
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                currencyTitle);

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "CustomerWinLossBetList");
        }
    }
}