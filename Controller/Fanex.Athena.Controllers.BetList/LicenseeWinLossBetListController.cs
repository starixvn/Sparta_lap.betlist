// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss, PermissionKeys.ReportWinLossByProduct, PermissionKeys.ReportColossusBetsWinLoss)]
    public class LicenseeWinLossBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            return ProcessDefaultRequest(metaModel, "~/Views/BetList/LicenseeWl/Main.cshtml");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinloss_Mem";
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            // determine currency viewed in
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);

            var title = ViewResources.Customer + " " + ViewResources.BetList
                + string.Format(
                " - {0} --> {1} - {2}{3} - {4}",
                metaModel.FromDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ToDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                viewinCurrency);

            var colList = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, colList);

            var workbook = excel.Export(ticketResult, title);

            var fileName = string.IsNullOrWhiteSpace(metaModel.ExcelFileName) ? "LicenseeWinLossBetList" : metaModel.ExcelFileName;
            return new ExcelResult(workbook, fileName);
        }
    }
}