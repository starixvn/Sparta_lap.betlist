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
    using System;
    using System.Linq;
    using System.Web.Http;
    using Base.Attributes;

    public class MixParlayBetListController : ApiBetListBaseController
    {
        [HttpGet]
        public dynamic Detail(long transId)
        {
            var ticketResult = BetListService.GetMixParlayDetailTicket(SessionService.LicenseeSession.LicenseeId, transId);
            var ticket = Ticket(ticketResult: ticketResult, showOdds: true);
            return ticket;
        }

        [HttpGet]
        public dynamic NewParlayDetail(long transId, string betTeam, string betCheck, decimal handicap1, decimal handicap2)
        {
            var texts = CacheRef.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2);

            var ticketResult = BetListService.GetMixParlayDetailTicket(transId);
            var tickets = ticketResult.MainTicketList.Where(x => x.Stake != 0).ToList();

            var textRenderPattern = "<p class='system-parlay-ticket-infor'>"
                                    + "<span class='system-parlay-combo-type'>{0}</span> x "
                                    + "<span class='system-parlay-unit-stake'>{1} {2}</span>"
                                    + "</p>";
            var ticketTextRender = string.Empty;
            if (tickets.Count > 0)
            {
                if (handicap1 == 0)
                {
                    // System Parlay.
                    var ticket = tickets.FirstOrDefault();
                    texts[0] = string.Format(textRenderPattern, texts[0], ticket.Currency, ticket.Stake.ToString("N0"));
                }
                else
                {
                    // Combo parlay.
                    tickets = tickets.OrderBy(x => x.MatchId).ToList();
                    var length = Math.Min(texts.Count(), tickets.Count());
                    for (int i = 0; i < length; i++)
                    {
                        var text = texts[i];
                        var ticket = tickets[i];
                        texts[i] = string.Format(textRenderPattern, text, ticket.Currency, ticket.Stake.ToString("N0"));
                    }
                }
            }

            return new
            {
                Data = Ticket(ticketResult: ticketResult, showOdds: true),
                Texts = texts
            };
        }
    }
}