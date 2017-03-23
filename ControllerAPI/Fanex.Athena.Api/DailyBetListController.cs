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
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.DataDomain;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportDailyBetList)]
    public class DailyBetListController : ApiBetListBaseController
    {
        [HttpGet]
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
        {
            const string UspKey = "Lrf_BetList_SelWinlossDaily";

            var metaModel = new BetListMetaModel
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
                IsServerPaging = true
            };
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            return GetPagingBetListResult(metaModel, UspKey, page, rows);
        }
    }
}