// --------------------------------------------------------------------------
// <copyright file="BetListController.LiveReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>

// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using DNAide.Utils;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.MetaModel.Product;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        #region Local Paging

        /// <summary>
        /// Get data for live report bet list by match.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpPost]
        public dynamic LiveReportMatch(LiveReportBetListMetaModel metaModel)
        {
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            const string UspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";

            return LiveReportBetListResult(metaModel, UspKey);
        }

        /// <summary>
        /// Get data for live report outright bet list.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpPost]
        public dynamic LiveReportOutright(LiveReportBetListMetaModel metaModel)
        {
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                metaModel.UserId = SessionService.UserSession.UserId;
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
                return LiveReportBetListResult(metaModel, UspKey);
            }

            return null;
        }

        #endregion Local Paging

        #region Server Paging

        /// <summary>
        /// Get data for live report number game bet list.
        /// </summary>
        /// <param name="page">Page Index.</param>
        /// <param name="rows">Row count.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sumComm">Sum commission value.</param>
        /// <param name="sumLicenseeWl">Sum Licensee Win Loss value.</param>
        /// <param name="sumPlayerComm">Sum player commission value.</param>
        /// <param name="sumWinLost">Sum Win Loss value.</param>
        /// <param name="totalRows">Total row count.</param>
        /// <param name="baseCurrency">Base currency.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpGet]
        public dynamic LiveReportNumberGame(
            int page,
            int rows,
            int currencyId,
            DateTime fromDate,
            DateTime toDate,
            decimal sumComm,
            decimal sumLicenseeWl,
            decimal sumPlayerComm,
            decimal sumWinLost,
            int totalRows,
            string baseCurrency)
        {
            var currencyList = SessionService.UserSession.CurrencyList;

            if (currencyId == 0 || currencyList.Values.Contains(currencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
                var metaModel = new LiveReportBetListMetaModel
                {
                    CurrencyId = currencyId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SumComm = sumComm,
                    SumLicenseeWl = sumLicenseeWl,
                    SumPlayerComm = sumPlayerComm,
                    SumWinlost = sumWinLost,
                    TotalRows = totalRows,
                    BaseCurrency = baseCurrency,

                    UserId = SessionService.UserSession.UserId,
                    LicenseeId = SessionService.LicenseeSession.LicenseeId
                };

                return GetLiveReportPagingBetListResult(metaModel, UspKey, page, rows);
            }

            return null;
        }

        /// <summary>
        /// Get data for live report mix parlay bet list.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="rows">Row count.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="betType">Bet type value.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="showSb">Show sports book or not.</param>
        /// <param name="showBa">Show Ba or not.</param>
        /// <param name="sumComm">Sum commission value.</param>
        /// <param name="sumLicenseeWl">Sum Licensee Win Loss value.</param>
        /// <param name="sumPlayerComm">Sum player commission value.</param>
        /// <param name="sumWinLost">Sum Win Loss value.</param>
        /// <param name="totalRows">Total row count.</param>
        /// <param name="baseCurrency">Base currency.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpGet]
        public dynamic LiveReportMixParlay(
            int page,
            int rows,
            int currencyId,
            int betType,
            DateTime fromDate,
            DateTime toDate,
            bool showSb,
            bool showBa,
            decimal sumComm,
            decimal sumLicenseeWl,
            decimal sumPlayerComm,
            decimal sumWinLost,
            int totalRows,
            string baseCurrency)
        {
            var currencyList = SessionService.UserSession.CurrencyList;

            if (currencyId == 0 || currencyList.Values.Contains(currencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";
                var productOptions = new List<ProductOption>
            {
                new ProductOption
                {
                    Code = "SB",
                    Status = showSb
                },
                new ProductOption
                {
                    Code = "BA",
                    Status = showBa
                }
            };

                var metaModel = new LiveReportBetListMetaModel
                {
                    CurrencyId = currencyId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SumComm = sumComm,
                    SumLicenseeWl = sumLicenseeWl,
                    SumPlayerComm = sumPlayerComm,
                    SumWinlost = sumWinLost,
                    TotalRows = totalRows,
                    BaseCurrency = baseCurrency,

                    UserId = SessionService.UserSession.UserId,
                    LicenseeId = SessionService.LicenseeSession.LicenseeId
                };

                metaModel.ProductOptions = productOptions;
                metaModel.BetType = betType;

                return GetLiveReportPagingBetListResult(metaModel, UspKey, page, rows);
            }

            return null;
        }

        /// <summary>
        /// Get data for live report racing bet list.
        /// </summary>
        /// <param name="page">Page Index.</param>
        /// <param name="rows">Row count.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="sportType">Sport type id.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sumComm">Sum commission value.</param>
        /// <param name="sumLicenseeWl">Sum Licensee Win Loss value.</param>
        /// <param name="sumPlayerComm">Sum player commission value.</param>
        /// <param name="sumWinLost">Sum Win Loss value.</param>
        /// <param name="totalRows">Total row count.</param>
        /// <param name="baseCurrency">Base currency.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpGet]
        public dynamic LiveReportRacing(
            int page,
            int rows,
            int currencyId,
            int sportType,
            DateTime fromDate,
            DateTime toDate,
            decimal sumComm,
            decimal sumLicenseeWl,
            decimal sumPlayerComm,
            decimal sumWinLost,
            int totalRows,
            string baseCurrency)
        {
            const string UspKeyRacing = "Lrf_BetList_LiveReport_SelWinlossRacing";
            const string UspKeyVHRacing = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

            var currencyList = SessionService.UserSession.CurrencyList;

            if (currencyId == 0 || currencyList.Values.Contains(currencyId))
            {
                var metaModel = new LiveReportBetListMetaModel
                {
                    CurrencyId = currencyId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SumComm = sumComm,
                    SumLicenseeWl = sumLicenseeWl,
                    SumPlayerComm = sumPlayerComm,
                    SumWinlost = sumWinLost,
                    TotalRows = totalRows,
                    BaseCurrency = baseCurrency,

                    UserId = SessionService.UserSession.UserId,
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                    SportType = sportType
                };

                var racingBettypes = new List<int>()
                {
                    (int)SportType.VirtualHorseRacing,
                    (int)SportType.VirtualGreyhound,
                    (int)SportType.VirtualMotoRacing,
                    (int)SportType.VirtualSpeedway,
                    (int)SportType.VirtualCycling
                };
                var uspKey = racingBettypes.Contains(sportType) ? UspKeyVHRacing : UspKeyRacing;
                return GetLiveReportPagingBetListResult(metaModel, uspKey, page, rows);
            }

            return null;
        }

        #endregion Server Paging

        #region Utility

        /// <summary>
        /// Render live report bet list result.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="uspKey">Stored procedure key.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [NonAction]
        private dynamic LiveReportBetListResult(LiveReportBetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = _betListService.GetBetListData(metaModel, uspKey);

            return new
            {
                Main = Ticket(ticketResult),
                Total = new
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWl + ticketResult.SumComm
                }
            };
        }

        /// <summary>
        /// Render live report paging bet list result.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="uspKey">Stored procedure key.</param>
        /// <param name="pageIndex">Page Index.</param>
        /// <param name="pageSize">Page Size.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [NonAction]
        private dynamic GetLiveReportPagingBetListResult(LiveReportBetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUpInfo = _balanceUpService.BalanceUpInfo;

            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);

            var ticketResult = _betListService.GetBetListData(metaModel, uspKey);

            var totalPage = metaModel.TotalRows % metaModel.PageSize == 0
                ? metaModel.TotalRows / metaModel.PageSize
                : (metaModel.TotalRows / metaModel.PageSize) + 1;
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;

            var sumWinLoss = ticketResult.MainTicketList.Sum(x => x.Winlost + x.PlayerComm);
            var sumLicenseeWl = ticketResult.MainTicketList.Sum(x => x.LicenseeWl + x.Comm);
            var sumWinLossText = NumberFormat.DecFormat(sumWinLoss, 2, false);
            var sumLicenseeWlText = NumberFormat.DecFormat(sumLicenseeWl, 2, false);
            sumLicenseeWlText = sumLicenseeWl < 0
                ? string.Format("<span class='negative'>{0}</span>", sumLicenseeWlText)
                : sumLicenseeWlText;

            return new
            {
                page = pageIndex,
                total = totalPage,
                records = metaModel.TotalRows,
                rows = Ticket(ticketResult, metaModel, startIndex),
                userdata = new
                {
                    WinLoss = sumWinLossText,
                    LicenseeWl = sumLicenseeWlText
                },
                IsHasCustComm = licenseeSession.IsAllowedCustomerComm,
            };
        }

        #endregion Utility
    }
}