// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Globalization;
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
            var model = CreateDefaultMetaModel(metaModel);

            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    return Excel(model);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/CustomerWl/Main.cshtml", model);
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinLossByDate_Mem";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var currencyTitle = string.Format(
                ViewResources.Viewin,
                metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper(CultureInfo.CurrentCulture) : metaModel.CurrencyName);

            var title = ViewResources.Customer + " " + ViewResources.BetList + string.Format(
                " - {0} --> {1} - {2}{3} - {4}",
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                currencyTitle);

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn };

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, hiddenColumns);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "CustomerWinLossBetList");
        }
    }
}