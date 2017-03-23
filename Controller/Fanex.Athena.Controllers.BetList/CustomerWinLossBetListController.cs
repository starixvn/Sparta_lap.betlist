// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
                {
                    return Excel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/CustomerWl/Main.cshtml", metaModel);
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinLossByDate_Mem";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            var currencyTitle = string.Format(
                ViewResources.Viewin,
                metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

            var title = ViewResources.Customer + " " + ViewResources.BetList + string.Format(
                " - {0} --> {1} - {2}({3}) - {4}",
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                metaModel.ExternalId,
                metaModel.SystemId,
                currencyTitle);

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn };

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, hiddenColumns);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "CustomerWinLossBetList");
        }
    }
}