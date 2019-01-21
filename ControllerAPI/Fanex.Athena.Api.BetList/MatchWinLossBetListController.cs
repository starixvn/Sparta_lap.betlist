// --------------------------------------------------------------------------
// <copyright file="MatchWinLossBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossBetListController : ApiBetListBaseController
    {
        public MatchWinLossBetListController(
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

        [HttpGet]
        public async Task<dynamic> MatchBetType(
            int page,
            int rows,
            [FromUri]BetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_MatchWL_Bettype";

            return await GetPagingBetListResult(metaModel, UspKey, page, rows).ConfigureAwait(false);
        }

        /// <summary>
        /// Get match win lost outright bet list.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Bet list data for match win loss outright.</returns>
        [HttpPost]
        public async Task<dynamic> Outright(BetListMetaModel metaModel)
        {
            if (IsValidCurrencyId(metaModel.CurrencyId))
            {
                metaModel.IsServerPaging = true;

                const string UspKey = "Lrf_BetList_MatchWL_Outright";
                return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
            }

            return null;
        }

        [HttpGet]
#pragma warning disable S107 // Methods should not have too many parameters
        public async Task<dynamic> MixParlay(
            int page,
            int rows,
            int currencyId,
            string baseCurrency,
            int betType,
            DateTime fromDate,
            DateTime toDate,
            string productCodeList,
            string sortBy = null,
            string sortOrder = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (IsValidCurrencyId(currencyId))
            {
                const string UspKey = "Lrf_BetList_MatchWL_Parlay";
                var metaModel = new BetListMetaModel
                {
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                    UserId = SessionService.UserSession.UserId,
                    CurrencyId = currencyId,
                    BetType = betType,
                    BaseCurrency = baseCurrency ?? string.Empty,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    ProductCodeList = productCodeList
                };

                return await GetPagingBetListResult(metaModel, UspKey, page, rows).ConfigureAwait(false);
            }

            return null;
        }
    }
}