// --------------------------------------------------------------------------
// <copyright file="BetListController.MatchWl.cs" company="Nexcel Solutions Vietnam">
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
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for match win lost bet type bet list.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Bet list data for match win loss bet type.</returns>
        [HttpGet]
        public dynamic MatchWLMatchBetType(
            int page,
            int rows,
            [FromUri]BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
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
        public dynamic MatchWLOutright(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                metaModel.UserId = userSession.UserId;
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

                const string UspKey = "Lrf_BetList_MatchWL_Outright";
                return GetBetListResult(metaModel, UspKey);
            }

            return null;
        }

        /// <summary>
        /// Get match win lost mix parlay bet list.
        /// </summary>
        /// <param name="page">Page Index.</param>
        /// <param name="rows">Row count.</param>
        /// <param name="licenseeType">Licensee Type.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="baseCurrency">Base currency.</param>
        /// <param name="betType">Bet type value.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="showSB">Show sports book or not.</param>
        /// <param name="showBA">Show Ba or not.</param>
        /// <param name="sortBy">Sort by column.</param>
        /// <param name="sortOrder">Sort order.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        [HttpGet]
        public dynamic MatchWLMixParlay(
            int page,
            int rows,
            string licenseeType,
            int currencyId,
            string baseCurrency,
            int betType,
            DateTime fromDate,
            DateTime toDate,
            bool showSB,
            bool showBA,
            string sortBy = null,
            string sortOrder = null)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId == 0 || currencyList.Values.Contains(currencyId))
            {
                const string UspKey = "Lrf_BetList_MatchWL_Parlay";
                var productOptions = new HashSet<ProductOption>
                {
                    new ProductOption
                    {
                        Code = "SB",
                        Status = showSB
                    },
                    new ProductOption
                    {
                        Code = "BA",
                        Status = showBA
                    }
                };

                var metaModel = new BetListMetaModel
                {
                    LicenseeType = licenseeType,
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                    UserId = userSession.UserId,
                    CurrencyId = currencyId,
                    BetType = betType,
                    ProductOptions = productOptions,
                    BaseCurrency = baseCurrency ?? string.Empty,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    IsServerPaging = true
                };

                return GetPagingBetListResult(metaModel, UspKey, page, rows);
            }

            return null;
        }
    }
}