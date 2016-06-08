// -----------------------------------------------------------------------
// <copyright file="PendingFundsBetListController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    #endregion Using

    public class PendingFundsBetListController : BetListBaseController
    {
        private static readonly List<int> HiddenColList =
            new List<int> { ExternalIdColumn, SystemIdColumn, WinLostColumn, BetEffectiveColumn, LicenseeWLColumn };

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            metaModel.FromDate = DateTime.Today.AddYears(-10);
            metaModel.ToDate = DateTime.Today.AddYears(10);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId != BaseCurrencyType.All && !currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
                }

                const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";

                return this.Excel(metaModel, UspKey);
            }

            return View("~/Views/BetList/PendingFunds/Main.cshtml", metaModel);
        }

        private ActionResult Excel(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = this.BetListService.GetBetListData(metaModel, uspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var title = "Running " + ViewResources.BetList
                + string.Format(
                " - {0}({1}) - {2}",
                metaModel.ExternalId,
                metaModel.SystemId,
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, HiddenColList);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "CustomerPendingFundsBetList");
        }
    }
}