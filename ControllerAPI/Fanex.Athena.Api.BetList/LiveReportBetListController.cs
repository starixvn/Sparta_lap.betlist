// --------------------------------------------------------------------------
// <copyright file="LiveReportBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>

// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Threading.Tasks;
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
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportBetListController : ApiBetListBaseController
    {
        public LiveReportBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter,
            IBetListHTMLBuilder betlistHtmlBuilder)
        : base(sessionService, productService, languageService, bridgeClientService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter, betlistHtmlBuilder)
        {
        }

        [HttpPost]
        public async Task<dynamic> Match(LiveReportBetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";

            return await GetData(metaModel, UspKey).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> Outright(LiveReportBetListMetaModel metaModel)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
                return await GetData(metaModel, UspKey).ConfigureAwait(false);
            }

            return null;
        }

        [HttpGet]
        public async Task<dynamic> NumberGame([FromUri] LiveReportBetListMetaModel metaModel, int page, int rows)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
                
                return await GetPagingData(metaModel, UspKey, page, rows).ConfigureAwait(false);
            }

            return null;
        }

        [HttpGet]
        public async Task<dynamic> MixParlay([FromUri] LiveReportBetListMetaModel metaModel, int page, int rows)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";

                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

                return await GetPagingData(metaModel, UspKey, page, rows).ConfigureAwait(false);
            }

            return null;
        }

        [HttpGet]
        public async Task<dynamic> VirtualSports([FromUri] LiveReportBetListMetaModel metaModel, int page, int rows)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                const string UspName = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

               
                return await GetPagingData(metaModel, UspName, page, rows).ConfigureAwait(false);
            }

            return null;
        }

        #region Utility

        private async Task<dynamic> GetData(LiveReportBetListMetaModel metaModel, string uspKey)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var currencyList = SessionService.UserSession.CurrencyList;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.IsShowPlayerCom = licenseeSession.IsAllowedCustomerComm;
            metaModel.CurrencyList = currencyList;
            metaModel.UspKey = uspKey;
            var betlistAPIUrl = new Uri($"http://localhost:55050/LiveReportBetList/GetData");
            return await BridgeClientService.PostAsJsonAsync<LiveReportBetListMetaModel, dynamic>(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        private async Task<dynamic> GetPagingData(LiveReportBetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var currencyList = SessionService.UserSession.CurrencyList;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.IsShowPlayerCom = licenseeSession.IsAllowedCustomerComm;
            metaModel.CurrencyList = currencyList;
            metaModel.UspKey = uspKey;
            var betlistAPIUrl = new Uri($"http://localhost:55050/LiveReportBetList/GetPagingData");
            return await BridgeClientService.PostAsJsonAsync<LiveReportBetListMetaModel, dynamic>(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        #endregion Utility
    }
}