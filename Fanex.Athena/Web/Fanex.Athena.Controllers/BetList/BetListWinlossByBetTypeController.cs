namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ResultExtension;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BetListWinlossByBetTypeController : BetListBaseController
    {
        public ActionResult WinlossByBetType(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            if (!metaModel.IsExportExcel)
            {
                return this.View("~/Views/BetList/WinlossByBetType/Main.cshtml", metaModel);
            }

            var currencyList = this.SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId != BaseCurrencyType.All && 
                currencyList.Values.Contains(metaModel.CurrencyId) && 
                metaModel.FromDate.Date == metaModel.ToDate.Date)
            {
                return this.WinlossByBetTypeExcel(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        [NonAction]
        public ActionResult WinlossByBetTypeExcel(BetListMetaModel metaModel)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

            // determine currency viewed in
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            var title = string.Format(
                "{0} - {1} - {2} → {3} - {4}",
                ViewResources.BetList,
                CacheRef.GetBettype(metaModel.BetType),
                metaModel.FromDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ToDate.ToString(FormatKey.ShortFormatDate),
                viewinCurrency);

            var excel = new BetListExcel(new ShowingOption(), metaModel.LicenseeId == 1, new List<int> { BetEffectiveColumn });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "WinLossByBetTypeBetList");
        }
    }
}