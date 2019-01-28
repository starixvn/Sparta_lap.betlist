// --------------------------------------------------------------------------
// <copyright file="MixParlayBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.DTO;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class MixParlayBetListController : ApiBetListBaseController
    {
        public MixParlayBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter,
            IBetListHTMLBuilder betlistHtmlBuilder)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter, betlistHtmlBuilder)
        {
        }

        [HttpGet]
        public dynamic Detail(long transId, int custId)
        {
            var ticketResult = BetListService.GetMixParlayDetailTicket(
                UserCurrencyList,
                SessionService.LicenseeSession.LicenseeId,
                transId,
                custId);
            var result = BuildParlayDetailTickets(ticketResult: ticketResult, showOdds: true);
            ClearData(ticketResult);
            return result;
        }

        [HttpGet]
        public dynamic NewParlayDetail(long transId, string betTeam, string betCheck, decimal handicap1, decimal handicap2, int custId)
        {
            var texts = (string[])CacheRefAdapter.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2).Clone();

            var ticketResult = BetListService.GetMixParlayDetailTicket(
                UserCurrencyList,
                SessionService.LicenseeSession.LicenseeId,
                transId,
                custId);
            var tickets = ticketResult.MainTicketList.Where(x => x.Stake != 0).ToList();

            var textRenderPattern = "<p class='system-parlay-ticket-infor'>"
                                    + "<span class='system-parlay-combo-type'>{0}</span> x "
                                    + "<span class='system-parlay-unit-stake'>{1} {2}</span>"
                                    + "</p>";
            if (tickets.Any())
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
                    var length = Math.Min(texts.Count(), tickets.Count);
                    for (int i = 0; i < length; i++)
                    {
                        var text = texts[i];
                        var ticket = tickets[i];
                        texts[i] = string.Format(textRenderPattern, text, ticket.Currency, ticket.Stake.ToString("N0"));
                    }
                }
            }

            var result = new
            {
                Data = BuildParlayDetailTickets(ticketResult: ticketResult, showOdds: true),
                Texts = texts
            };
            ClearData(ticketResult);
            return result;
        }

        protected internal virtual IList<string> BuildParlayDetailTickets(TicketResult ticketResult, bool showOdds = false)
        {
            var ticketTextList = new List<string>();

            TicketHelper.IsShowOddsWithChoice = showOdds;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                var statusTemplate = BetListHTMLBuilder.CreateStatusBuilder(ticket.BetTypeId).Render(ticket, TicketHelper, ticketResult.TicketDataList, isShowVNIP: false);                statusTemplate.ShowIP = null;
                statusTemplate.StatusResult = null;

                var choice = BetListHTMLBuilder.BuildChoice(ticket, TicketHelper, ticketResult.TicketDataList, false);
                var transDateAndStatus = BetListHTMLBuilder.BuildTransEventDateAndStatusResult(ticket, ticketResult.TicketDataList);
                
                ticketTextList.Add(choice + transDateAndStatus);
            }

            return ticketTextList;
        }
    }
}