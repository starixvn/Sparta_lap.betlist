// --------------------------------------------------------------------------
// <copyright file="BetListController.MixParlay.cs" company="Nexcel Solutions Vietnam">
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

    #endregion Using

    /// <summary>
    /// Mix Parlay Detail.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for running mix parlay bet list.
        /// </summary>
        /// <param name="transId">Transaction Id.</param>
        /// <returns>Dynamic objects, contain Mix Parlay result.</returns>
        [HttpGet]
        public dynamic MixParlayDetail(long transId)
        {
            var ticketResult = _betListService.GetMixParlayDetailTicket(transId);
            var ticket = Ticket(ticketResult: ticketResult, showOdds: true);
            return ticket;
        }
    }
}