// --------------------------------------------------------------------------
// <copyright file="DailyBetListController.cs" company="">
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

    [ApiFunctionAuthorization(PermissionKeys.ReportDailyBetList)]
    public class DailyBetListController : ApiBetListBaseController
    {
        public DailyBetListController()
        {
        }

        [CLSCompliant(false)]
        public DailyBetListController(
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
#pragma warning disable S107 // Methods should not have too many parameters
        public dynamic Index(
            int page,
            int rows,
            int currencyId,
            string currencyName,
            string baseCurrency,
            DateTime date,
            string productCode,
            string status,
            string sortBy = null,
            string sortOrder = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            const string UspKey = "Lrf_BetList_SelWinlossDaily";

            var metaModel = new DailyBetListMetaModel
            {
                CurrencyId = currencyId,
                CurrencyName = currencyName,
                BaseCurrency = baseCurrency ?? string.Empty,
                Date = date.Date,
                FromDate = date.Date,
                ToDate = date.Date,
                ProductCode = productCode,
                DailyBetListStatus = status,
                SortBy = sortBy,
                SortOrder = sortOrder,
                IsServerPaging = true,
                AgentListXml = SessionService.GenerateAgentListXml(currencyId)
            };
            return GetPagingBetListResult(metaModel, UspKey, page, rows);
        }
    }
}