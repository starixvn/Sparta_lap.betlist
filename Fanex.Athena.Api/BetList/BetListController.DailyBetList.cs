// --------------------------------------------------------------------------
// <copyright file="BetListController.DailyBetList.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for daily bet list.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="rows">Row count.</param>
        /// <param name="licenseeType">Licensee type.</param>
        /// <param name="currencyId">Currency id.</param>
        /// <param name="currencyName">Currency name.</param>
        /// <param name="baseCurrency">Base currency.</param>
        /// <param name="date">Search date.</param>
        /// <param name="productCode">Product code.</param>
        /// <param name="status">Status value.</param>
        /// <param name="sortBy">Sort by column.</param>
        /// <param name="sortOrder">Sort order.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpGet]
        public dynamic DailyBetList(
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
                SortOrder = sortOrder 
            };

            return GetPagingBetListResult(metaModel, UspKey, page, rows);
        }
    }
}