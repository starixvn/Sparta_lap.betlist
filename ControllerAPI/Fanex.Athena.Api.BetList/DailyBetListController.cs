// --------------------------------------------------------------------------
// <copyright file="DailyBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportDailyBetList)]
    public class DailyBetListController : ApiBetListBaseController
    {
        public DailyBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, betListApiClient, balanceUpDataAccess)
        {
        }

        [HttpGet]
#pragma warning disable S107 // Methods should not have too many parameters
        public async Task<dynamic> Index(
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
                SortOrder = sortOrder
            };
            return await GetPagingBetListResult(metaModel, UspKey, page, rows, "/BetList/GetDailyBetListData").ConfigureAwait(false);
        }
    }
}