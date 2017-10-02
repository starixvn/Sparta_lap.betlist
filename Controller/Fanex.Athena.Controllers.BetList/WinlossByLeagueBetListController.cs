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
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportWinLossByLeague)]
    public class WinlossByLeagueBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index(WinLossByLeagueBetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel) as WinLossByLeagueBetListMetaModel;

            if (!model.IsExportExcel)
            {
                return View("~/Views/BetList/WinlossByLeague/Main.cshtml", model);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
            {
                model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                return Excel(model);
            }

            return EmptyExcel();
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_WinlostByLeague_Mem";
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);

            // determine currency viewed in
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            var title = string.Format(
                "{0} - {1} → {2} - {3}{4} - {5}",
                ViewResources.CustomerBetList,
                metaModel.FromDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ToDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                viewinCurrency);

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

            var excel = new BetListExcel(new ShowingOption { CustomerCommVisible = true }, metaModel.LicenseeId, hiddenColumns);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "WinLossByLeagueBetList");
        }
    }
}