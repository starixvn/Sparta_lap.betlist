// --------------------------------------------------------------------------
// <copyright file="BetListController.CancelledBetsBetList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for cancelled bets bet list.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>Return result of bet list.</returns>
        [HttpPost]
        public dynamic CancelledBetsBetList(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            const string UspKey = "Lrf_BetList_SelDeleted";
            metaModel.BetType = 13;

            return GetBetListResult(metaModel, UspKey);
        }
    }
}