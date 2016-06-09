// --------------------------------------------------------------------------
// <copyright file="LiveReportBetListController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web.Http;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.MetaModel.Product;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveReportBetListController : ApiBetListBaseController
    {
        #region Local Paging

        [HttpPost]
        public dynamic Match(LiveReportBetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            const string UspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";

            return GetData(metaModel, UspKey);
        }

        [HttpPost]
        public dynamic Outright(LiveReportBetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                metaModel.UserId = userSession.UserId;
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
                return GetData(metaModel, UspKey);
            }

            return null;
        }

        #endregion Local Paging

        #region Server Paging

        [HttpGet]
        public dynamic NumberGame(
            int page,
            int rows,
            int currencyId,
            DateTime fromDate,
            DateTime toDate,
            decimal sumComm,
            decimal sumLicenseeWL,
            decimal sumPlayerComm,
            decimal sumWinLost,
            int totalRows,
            string baseCurrency,
            int liveIndicator,
            string sortBy = null,
            string sortOrder = null)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.Values.Contains(currencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
                var metaModel = new LiveReportBetListMetaModel
                {
                    CurrencyId = currencyId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SumComm = sumComm,
                    SumLicenseeWL = sumLicenseeWL,
                    SumPlayerComm = sumPlayerComm,
                    SumWinlost = sumWinLost,
                    TotalRows = totalRows,
                    BaseCurrency = baseCurrency,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    UserId = userSession.UserId,
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                    LiveIndicator = liveIndicator
                };

                return GetPagingData(metaModel, UspKey, page, rows);
            }

            return null;
        }

        [HttpGet]
        public dynamic MixParlay(
            int page,
            int rows,
            int currencyId,
            int betType,
            DateTime fromDate,
            DateTime toDate,
            bool showSB,
            bool showBA,
            decimal sumComm,
            decimal sumLicenseeWL,
            decimal sumPlayerComm,
            decimal sumWinLost,
            int totalRows,
            string baseCurrency,
            int liveIndicator,
            string sortBy = null,
            string sortOrder = null)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.Values.Contains(currencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";
                var productOptions = new HashSet<ProductOption>
            {
                new ProductOption
                {
                    Code = "SB",
                    Status = showSB
                },
                new ProductOption
                {
                    Code = "BA",
                    Status = showBA
                }
            };

                var metaModel = new LiveReportBetListMetaModel
                {
                    CurrencyId = currencyId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SumComm = sumComm,
                    SumLicenseeWL = sumLicenseeWL,
                    SumPlayerComm = sumPlayerComm,
                    SumWinlost = sumWinLost,
                    TotalRows = totalRows,
                    BaseCurrency = baseCurrency,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    UserId = userSession.UserId,
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                    LiveIndicator = liveIndicator
                };

                metaModel.ProductOptions = productOptions;
                metaModel.BetType = betType;

                return GetPagingData(metaModel, UspKey, page, rows);
            }

            return null;
        }

        [HttpGet]
        public dynamic Racing(
            int page,
            int rows,
            int currencyId,
            int sportType,
            DateTime fromDate,
            DateTime toDate,
            decimal sumComm,
            decimal sumLicenseeWL,
            decimal sumPlayerComm,
            decimal sumWinLost,
            int totalRows,
            string baseCurrency,
            string sortBy = null,
            string sortOrder = null)
        {
            const string UspKeyRacing = "Lrf_BetList_LiveReport_SelWinlossRacing";
            const string UspKeyVHRacing = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.Values.Contains(currencyId))
            {
                var metaModel = new LiveReportBetListMetaModel
                {
                    CurrencyId = currencyId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SumComm = sumComm,
                    SumLicenseeWL = sumLicenseeWL,
                    SumPlayerComm = sumPlayerComm,
                    SumWinlost = sumWinLost,
                    TotalRows = totalRows,
                    BaseCurrency = baseCurrency,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    UserId = userSession.UserId,
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
                return GetPagingData(metaModel, uspKey, page, rows);
            }

            return null;
        }

        #endregion Server Paging

        #region Utility

        private dynamic GetData(LiveReportBetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            return new
            {
                Main = Ticket(ticketResult),
                Total = new
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm
                }
            };
        }

        private dynamic GetPagingData(LiveReportBetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUpInfo = BalanceUpService.BalanceUpInfo;

            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);

            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            var totalPage = metaModel.TotalRows % metaModel.PageSize == 0
                ? metaModel.TotalRows / metaModel.PageSize
                : (metaModel.TotalRows / metaModel.PageSize) + 1;
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;

            var sumWinLossText = NumberHelper.DecFormat(metaModel.SumWinlost, 2);
            var sumLicenseeWlText = NumberHelper.DecFormat(metaModel.SumLicenseeWL + metaModel.SumComm, 2);
            sumLicenseeWlText = metaModel.SumLicenseeWL < 0
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
                    LicenseeWL = sumLicenseeWlText
                },
                IsHasCustComm = licenseeSession.IsAllowedCustomerComm,
            };
        }

        #endregion Utility
    }
}