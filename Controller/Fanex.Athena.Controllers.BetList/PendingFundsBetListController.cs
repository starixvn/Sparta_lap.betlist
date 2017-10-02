// -----------------------------------------------------------------------
// <copyright file="PendingFundsBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
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

    [FunctionAuthorization(PermissionKeys.ReportCustomerPendingFunds)]
    public class PendingFundsBetListController : BetListBaseController
    {
        private static readonly List<int> HiddenColList =
            new List<int> { ExternalIdColumn, SystemIdColumn, WinLostColumn, BetEffectiveColumn, LicenseeWLColumn };

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            model.FromDate = DateTime.Today.AddYears(-10);
            model.ToDate = DateTime.Today.AddYears(10);

            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId != BaseCurrencyType.All && !currencyList.ContainsKey(model.CurrencyId))
                {
                    return EmptyExcel();
                }

                const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";
                model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                return Excel(model, UspKey);
            }

            return View("~/Views/BetList/PendingFunds/Main.cshtml", model);
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
                " - {0}{1} - {2}",
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName));

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, HiddenColList);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "CustomerPendingFundsBetList");
        }
    }
}