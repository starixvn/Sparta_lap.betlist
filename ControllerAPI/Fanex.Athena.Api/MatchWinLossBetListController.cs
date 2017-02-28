// --------------------------------------------------------------------------
// <copyright file="MatchWinLossBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Model.Core.DataDomain;
    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossBetListController : ApiBetListBaseController
    {
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
        public dynamic Outright(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.UserId = userSession.UserId;
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.IsServerPaging = true;

                const string UspKey = "Lrf_BetList_MatchWL_Outright";
                return GetBetListResult(metaModel, UspKey);
            }

            return null;
        }

        [HttpGet]
        public dynamic MixParlay(
            int page,
            int rows,
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

            if (currencyId == 0 || currencyList.ContainsKey(currencyId))
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