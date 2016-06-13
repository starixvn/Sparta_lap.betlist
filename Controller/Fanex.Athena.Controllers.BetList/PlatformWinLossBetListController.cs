// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossBetListController.cs" company="Nexcel">
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
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    public class PlatformWinLossBetListController : BetListBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                if (IsValidCurrency(metaModel.CurrencyId))
                {
                    return this.Excel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/PlatformWinLoss/Main.cshtml", metaModel);
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_PlatformWinLoss_Mem";
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            var platformText = PlatformType.GetPlatformName(metaModel.PlatformType);

            var title = ViewResources.Customer + " " + ViewResources.BetList
                + string.Format(
                " - {0} --> {1} - {2}({3}) - {4} - {5}",
                metaModel.FromDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ToDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ExternalId,
                metaModel.SystemId,
                ViewResources.Platform + ": " + platformText,
                viewinCurrency);

            var colList = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, colList);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "PlatformWinLossBetList");
        }
    }
}