// --------------------------------------------------------------------------
// <copyright file="LiveReportBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>

// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportBetListController : ApiBetListBaseController
    {
        public LiveReportBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter,
            IBetListHTMLBuilder betlistHtmlBuilder)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter, betlistHtmlBuilder)
        {
        }

        [HttpPost]
        public dynamic Match(LiveReportBetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
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
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
                return GetData(metaModel, UspKey);
            }

            return null;
        }

        [HttpGet]
        public dynamic NumberGame([FromUri] LiveReportBetListMetaModel metaModel, int page, int rows)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                return GetPagingData(metaModel, UspKey, page, rows);
            }

            return null;
        }

        [HttpGet]
        public dynamic MixParlay([FromUri] LiveReportBetListMetaModel metaModel, int page, int rows)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";

                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

                return GetPagingData(metaModel, UspKey, page, rows);
            }

            return null;
        }

        [HttpGet]
        public dynamic VirtualSports([FromUri] LiveReportBetListMetaModel metaModel, int page, int rows)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                const string UspName = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                return GetPagingData(metaModel, UspName, page, rows);
            }

            return null;
        }

        #region Utility

        private dynamic GetData(LiveReportBetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = BetListService.GetBetListData(metaModel, UserCurrencyList, uspKey);

            var result = new
            {
                Main = Ticket(ticketResult),
                Total = new
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm
                }
            };
            ClearData(ticketResult);
            return result;
        }

        private dynamic GetPagingData(LiveReportBetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
        {
            var licenseeSession = SessionService.LicenseeSession;

            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;

            var ticketResult = BetListService.GetBetListData(metaModel, UserCurrencyList, uspKey);

            var totalPage = metaModel.TotalRows % metaModel.PageSize == 0
                ? metaModel.TotalRows / metaModel.PageSize
                : (metaModel.TotalRows / metaModel.PageSize) + 1;
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;

            var sumLicenseeWl = metaModel.SumLicenseeWL + metaModel.SumComm;
            var sumWinLossText = NumberHelper.DecFormat(metaModel.SumWinlost + metaModel.SumPlayerComm, 2);
            var sumLicenseeWlText = NumberHelper.DecFormat(sumLicenseeWl, 2);
            sumLicenseeWlText = sumLicenseeWl < 0
                ? string.Format("<span class='negative-number'>{0}</span>", sumLicenseeWlText)
                : sumLicenseeWlText;

            var result = new
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

            ClearData(ticketResult);

            return result;
        }

        #endregion Utility
    }
}