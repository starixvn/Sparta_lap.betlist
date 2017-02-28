// -----------------------------------------------------------------------
// <copyright file="MatchWinLossBetListController.cs" company="">
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
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult MatchBetType(BetListMetaModel metaModel)
        {
            // include LicenseeId, UserId in CreateDefaultMetaModel
            metaModel = CreateDefaultMetaModel(metaModel);
            metaModel.IsServerPaging = true;

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
                {
                    var uspKey = IsRacing(metaModel.BetType)
                        ? "Lrf_BetList_MatchWL_Racing"
                        : "Lrf_BetList_MatchWL_Bettype";
                    return Excel(metaModel, uspKey);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/MatchWl/MatchBetType.cshtml", metaModel);
        }

        [HttpGet]
        public ActionResult MixParlay(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            metaModel.IsServerPaging = true;

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_MatchWL_Parlay";
                    metaModel.IsServerPaging = true;
                    return Excel(metaModel, UspKey);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/MatchWl/MixParlay.cshtml", metaModel);
        }

        [HttpGet]
        public ActionResult Outright(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            metaModel.IsServerPaging = true;

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_MatchWL_Outright";
                    return Excel(metaModel, UspKey);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/MatchWl/Outright.cshtml", metaModel);
        }

        private ActionResult Excel(BetListMetaModel metaModel, string uspKey)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var ticketResult = this.BetListService.GetBetListData(metaModel, uspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var matchName = metaModel.HomeId > 0 && metaModel.AwayId > 0
                  ? CacheRef.GetTeamName(metaModel.HomeId) + " vs " + CacheRef.GetTeamName(metaModel.AwayId) + " - "
                  : string.Empty;
            var currencyTitle = string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

            var title = ViewResources.BetList
                + string.Format(
                " - {0}{1} - {2}",
                matchName + (metaModel.BetType > 0 ? CacheRef.GetBettype(metaModel.BetType) + " - " : string.Empty),
                metaModel.FromDate.ToShortDateString(),
                currencyTitle);

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, new List<int> { BetEffectiveColumn });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "MatchWinLossBetList");
        }
    }
}