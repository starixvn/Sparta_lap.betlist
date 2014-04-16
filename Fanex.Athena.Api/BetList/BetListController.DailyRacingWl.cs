// --------------------------------------------------------------------------
// <copyright file="BetListController.DailyRacingWl.cs" company="Nexcel Solutions Vietnam">
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
        /// Get data for daily racing win lost bet list.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpPost]
        public dynamic DailyRacingWl(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            const string UspKey = "Lrf_BetList_SelWinlostRacing";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}