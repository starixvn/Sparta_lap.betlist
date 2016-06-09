// --------------------------------------------------------------------------
// <copyright file="DailyBetListController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Web.ActionFilter;

    #endregion Using

    public class DailyBetListController : ApiBetListBaseController
    {
        [HttpGet]
        [ApiFunctionAuthorization]
        public dynamic Index(
            int page,
            int rows,
            string licenseeType,
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
                LicenseeType = licenseeType,
                CurrencyId = currencyId,
                CurrencyName = currencyName,
                BaseCurrency = baseCurrency ?? string.Empty,
                Date = date.Date,
                FromDate = date.Date,
                ToDate = date.Date,
                UserId = SessionService.UserSession.UserId,
                ProductCode = productCode,
                DailyBetListStatus = status,
                SortBy = sortBy,
                SortOrder = sortOrder,
                IsServerPaging = true
            };

            return GetPagingBetListResult(metaModel, UspKey, page, rows);
        }
    }
}