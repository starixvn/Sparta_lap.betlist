// --------------------------------------------------------------------------
// <copyright file="BetListController.RunningBetListOfMatch.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Linq;
    using System.Web.Http;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// List of racing bet type.
        /// </summary>
        private static readonly int[] RacingBetType = { 31, 32, 33, 41, 42, 43 };

        /// <summary>
        /// Get data for running bet list of a match.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        [HttpPost]
        public dynamic RunningBetListOfMatch(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            string uspKey = IsRacing(metaModel.BetType)
                ? "Lrf_BetList_SelAllByDateHR"
                : "Lrf_BetList_SelRunningByBettypeMatch";
            return GetBetListResult(metaModel, uspKey);
        }

        /// <summary>
        /// Check if a bet type is racing.
        /// </summary>
        /// <param name="betType">Bet type value.</param>
        /// <returns>The bet type is racing bet type or not.</returns>
        private static bool IsRacing(int betType)
        {
            return RacingBetType.Contains(betType);
        }
    }
}