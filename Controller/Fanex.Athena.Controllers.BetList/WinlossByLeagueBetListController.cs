// -----------------------------------------------------------------------
// <copyright file="WinlossByLeagueBetListController.cs" company="Nexdev">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportWinLossByLeague)]
    public partial class WinlossByLeagueBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index(WinLossByLeagueBetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel) as WinLossByLeagueBetListMetaModel;

            if (!metaModel.IsExportExcel)
            {
                return this.View("~/Views/BetList/WinlossByLeague/Main.cshtml", metaModel);
            }

            var currencyList = this.SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                return this.Excel(metaModel);
            }

            return this.EmptyExcel();
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_WinlostByLeague_Mem";
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

            // determine currency viewed in
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            var title = string.Format(
                "{0} - {1} → {2} - {3}({4}) - {5}",
                ViewResources.CustomerBetList,
                metaModel.FromDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ToDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ExternalId,
                metaModel.SystemId,
                viewinCurrency);

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

            var excel = new BetListExcel(new ShowingOption { CustomerCommVisible = true }, metaModel.LicenseeId == 1, hiddenColumns);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "WinLossByLeagueBetList");
        }
    }
}