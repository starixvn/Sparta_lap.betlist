// -----------------------------------------------------------------------
// <copyright file="BetListController.MatchWl.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Linq;
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
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for match win lost bet type bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult MatchWlMatchBetType(BetListMetaModel metaModel)
        {
            // include LicenseeId, UserId in CreateDefaultMetaModel
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    var uspKey = IsRacing(metaModel.BetType)
                        ? "Lrf_BetList_MatchWL_Racing"
                        : "Lrf_BetList_MatchWL_Bettype";
                    return MatchWlExcel(metaModel, uspKey);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/MatchWl/MatchBetType.cshtml", metaModel);
        }

        /// <summary>
        /// Action Get for match win lost parlay.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult MatchWlMixParlay(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_MatchWL_Parlay";
                    metaModel.IsServerPaging = true;
                    return MatchWlExcel(metaModel, UspKey, true);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/MatchWl/MixParlay.cshtml", metaModel);
        }

        /// <summary>
        /// Action Get for match win lost outright bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult MatchWlOutright(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_MatchWL_Outright";
                    return MatchWlExcel(metaModel, UspKey);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/MatchWl/Outright.cshtml", metaModel);
        }

        /// <summary>
        /// Export excel for match win lost.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="uspKey">The store procedure key.</param>
        /// <param name="onePageOnly">Only one page.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        public ActionResult MatchWlExcel(BetListMetaModel metaModel, string uspKey, bool onePageOnly = false)
        {
            var ticketResult = this._betListService.GetBetListData(metaModel, uspKey);
            if (onePageOnly)
            {
                // Sum value by page
                ticketResult.SumWinLoss = ticketResult.MainTicketList.Sum(x => x.Winlost);
                ticketResult.SumLicenseeWl = ticketResult.MainTicketList.Sum(x => x.LicenseeWl);
                ticketResult.SumComm = ticketResult.MainTicketList.Sum(x => x.Comm);
                ticketResult.SumPlayerComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);
            }

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var matchName = metaModel.HomeId > 0 && metaModel.AwayId > 0
                  ? CacheRef.GetTeamName(metaModel.HomeId) + " vs " + CacheRef.GetTeamName(metaModel.AwayId) + "- "
                  : string.Empty;
            var currencyTitle = string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

            var title = ViewResources.BetList
                + string.Format(
                " - {0} - {1} - {2}",
                matchName + CacheRef.GetBettype(metaModel.BetType),
                metaModel.FromDate.ToShortDateString(),
                currencyTitle);

            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, null);

            var workbook = excel.Export(ticketResult, title, startIndex);

            return new ExcelResult(workbook, "LicenseeWinLossBetList");
        }
    }
}