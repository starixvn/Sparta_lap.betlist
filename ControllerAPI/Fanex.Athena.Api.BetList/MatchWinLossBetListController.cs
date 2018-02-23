// --------------------------------------------------------------------------
// <copyright file="MatchWinLossBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Entities;
    using Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossBetListController : ApiBetListBaseController
    {
        public MatchWinLossBetListController()
        {
        }

        [CLSCompliant(false)]
        public MatchWinLossBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter)
        {
        }

        [HttpGet]
        public dynamic MatchBetType(
            int page,
            int rows,
            [FromUri]BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.IsServerPaging = true;

            var uspKey = IsRacing(metaModel.BetType)
                ? "Lrf_BetList_MatchWL_Racing"
                : "Lrf_BetList_MatchWL_Bettype";

            return GetPagingBetListResult(metaModel, uspKey, page, rows);
        }

        /// <summary>
        /// Get match win lost outright bet list.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Bet list data for match win loss outright.</returns>
        [HttpPost]
        public dynamic Outright(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                metaModel.IsServerPaging = true;

                const string UspKey = "Lrf_BetList_MatchWL_Outright";
                return GetBetListResult(metaModel, UspKey);
            }

            return null;
        }

        [HttpGet]
#pragma warning disable S107 // Methods should not have too many parameters
        public dynamic MixParlay(
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
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.ContainsKey(currencyId))
            {
                const string UspKey = "Lrf_BetList_MatchWL_Parlay";
                var metaModel = new BetListMetaModel
                {
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                    UserId = userSession.UserId,
                    CurrencyId = currencyId,
                    BetType = betType,
                    BaseCurrency = baseCurrency ?? string.Empty,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    IsServerPaging = true,
                    AgentListXml = SessionService.GenerateAgentListXml(currencyId),
                    ProductCodeList = productCodeList
                };

                return GetPagingBetListResult(metaModel, UspKey, page, rows);
            }

            return null;
        }
    }
}