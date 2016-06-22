// --------------------------------------------------------------------------
// <copyright file="MixParlayBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Base.Attributes;

    #endregion Using

    /// <summary>
    /// Mix Parlay Detail.
    /// </summary>
    public class MixParlayBetListController : ApiBetListBaseController
    {
        [HttpGet]
        public dynamic Detail(long transId)
        {
            var ticketResult = BetListService.GetMixParlayDetailTicket(SessionService.LicenseeSession.LicenseeId, transId);
            var ticket = Ticket(ticketResult: ticketResult, showOdds: true);
            return ticket;
        }
    }
}