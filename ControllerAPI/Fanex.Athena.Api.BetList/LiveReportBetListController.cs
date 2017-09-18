// --------------------------------------------------------------------------
// <copyright file="LiveReportBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>

// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Match(LiveReportBetListMetaModel metaModel)
        {
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            const string UspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";

            return GetData(metaModel, UspKey);
        }

        [HttpPost]
        public dynamic Outright(LiveReportBetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId == 0 || SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
                return GetData(metaModel, UspKey);
            }

            return null;
        }

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

            if (currencyId == 0 || currencyList.ContainsKey(currencyId))
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
                    LiveIndicator = liveIndicator
                };
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
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
            decimal sumComm,
            decimal sumLicenseeWL,
            decimal sumPlayerComm,
            decimal sumWinLost,
            int totalRows,
            string baseCurrency,
            int liveIndicator,
            string productCodeList,
            string sortBy = null,
            string sortOrder = null)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.ContainsKey(currencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";

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
                    LiveIndicator = liveIndicator,
                    AgentListXml = SessionService.GenerateAgentListXml(currencyId),
                    BetType = betType,
                    ProductCodeList = productCodeList
                };

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
            const string UspName = "Lrf_BetList_LiveReport_SelWinlossRacing";

            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.ContainsKey(currencyId))
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
                    SportType = sportType
                };

                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                return GetPagingData(metaModel, UspName, page, rows);
            }

            return null;
        }

        [HttpGet]
        public dynamic VirtualSports(
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
            const string UspName = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.ContainsKey(currencyId))
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
                    SportType = sportType
                };

                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                return GetPagingData(metaModel, UspName, page, rows);
            }

            return null;
        }

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
            var balanceUp = GetBalanceUp();

            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);

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