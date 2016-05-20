// --------------------------------------------------------------------------
// <copyright file="BetListController.LastBetMonitoring.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Resources;
    using CacheRefHelper = Fanex.BetList.Core.TicketHelper.CacheRef;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        private static readonly IList<int> ConvertingDecimalOddsBetType = new List<int> { 1, 2, 3, 7, 8, 12, 20 };

        private readonly IMatchService _matchService = new MatchService();

        /// <summary>
        /// Get running match list by sport.
        /// </summary>
        /// <param name="sportId">Sport Id value.</param>
        /// <returns>Return match list and league list.</returns>
        [HttpGet]
        public dynamic GetRunningMatchList(int sportId)
        {
            var matchList = _matchService.GetAllRunningMatch(sportId);

            var leagueListNormal = matchList.Where(i => i.IsOutRight == false).Select(m => m.LeagueId).Distinct()
                                      .Select(l => new League
                                      {
                                          IsOutRight = false,
                                          LeagueId = l,
                                          LeagueName = CacheRef.GetLeagueName(l)
                                      }).ToList();
            var leagueListOutRight = matchList.Where(i => i.IsOutRight).Select(m => m.LeagueId).Distinct()

                                      .Select(l => new League
                                      {
                                          IsOutRight = true,
                                          LeagueId = l,
                                          LeagueName = CacheRef.GetLeagueName(l)
                                      }).ToList();

            var leagueList = leagueListNormal.Union(leagueListOutRight);

            return new
            {
                MatchList = matchList.OrderBy(i => i.MatchName),
                LeagueList = leagueList.OrderBy(i => i.LeagueName)
            };
        }

        /// <summary>
        /// Get data for last monitoring bet list.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains bet list result.</returns>
        [HttpPost]
        public dynamic LastMonitorBetListNew(LastBetsMonitoringMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var ticketResult = BetListService.GetLastBetsMonitoringData(metaModel);

            // Show empty content if data is empty
            return ticketResult == null || ticketResult.MainTicketList.Count == 0
                ? (dynamic)new
                {
                    NewRow = false
                }
                : new
                {
                    NewRow = true,
                    Rows = GetLastMonitorBetListContent(metaModel, ticketResult),
                    LastTransId = ticketResult.MainTicketList.Max(t => t.TransId),
                };
        }

        /// <summary>
        /// Build odds text.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="ticketResult">Ticket result.</param>
        /// <param name="ticket">LRF ticket object.</param>
        /// <param name="builder">Bet list builder.</param>
        private static void BuildOdds(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult, LrfTicket ticket, BetListHTMLBuilder builder)
        {
            const string OddStr = "<td data-name=\"Odds\" class=\"bl-odds\"><span class=\"underdog {1}\">{0}</span><br><span class=\"oddstype\">{2}</span></td>";
            if (metaModel == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(metaModel.OddsType)
                || !ConvertingDecimalOddsBetType.Contains(ticket.BetTypeId))
            {
                builder.AddOdds(
                    ticket,
                    ticketResult.TicketDataList,
                    "<td data-name=\"Odds\" class='bl-odds'>",
                    "</td>",
                    CacheRefHelper.GetOddsTypeName);
            }
            else
            {
                builder.Append(
                    string.Format(
                        OddStr,
                        Fanex.BetList.Core.Utils.ConvertByBetType.Odds(ticket.Odds),
                        ticket.Odds < 0 ? "negative" : string.Empty,
                        metaModel.OddsType));
            }
        }

        private static string MapPlatForm(string code)
        {
            switch (code.Trim().ToLower())
            {
                case "a":
                    return ViewResources.NewAsia;

                case "d":
                    return ViewResources.Asia;

                case "e":
                case "u":
                    return ViewResources.Euro;

                case "m":
                    return ViewResources.Wap.ToUpper();

                case "s":
                case "l":
                    return ViewResources.SmartSite;

                default:
                    return ViewResources.Others;
            }
        }

        /// <summary>
        /// Get status text of a ticket.
        /// </summary>
        /// <param name="ticket">LRF ticket object.</param>
        /// <returns>Status text.</returns>
        private static string GetLasBetMonitoringStatus(LrfTicket ticket)
        {
            const string PatternStatusBetlist = "<label style='color:{0}'>{1}</label>";
            if (ticket.BetTypeId == (int)BetType.MixParlay)
            {
                switch (ticket.LiveHomeScore)
                {
                    case 0:
                        return string.Format(PatternStatusBetlist, "#000", ViewResources.NoneLive);

                    case 1:
                        return string.Format(PatternStatusBetlist, "#cc0033", ViewResources.Live);

                    case 2:
                        return string.Format(PatternStatusBetlist, "#0033ff", ViewResources.MixLiveAndNoneLive);

                    default:
                        return string.Format(PatternStatusBetlist, "#000", ViewResources.NoneLive);
                }
            }

            return ticket.IsLive
                ? string.Format(PatternStatusBetlist, "#cc0033", ViewResources.Live)
                : string.Format(PatternStatusBetlist, "#000", ViewResources.NoneLive);
        }

        /// <summary>
        /// Render bet list content.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="ticketResult">Ticket result.</param>
        /// <returns>Bet list in html, using in last bet monitoring.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private string GetLastMonitorBetListContent(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult)
        {
            var builder = new BetListHTMLBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                TicketHelper.ClientDetailSystemParlayFunction = GetClientDetailSystemParlayFunction(metaModel, ticket);

                // Begin of row
                var row = string.Format("<tr class='content-row monitor-hightlight' id='trans-{0}' data-transid='{0}'>", ticket.TransId);
                builder.Append(row);

                // Platform
                builder.Append("<td data-name=\"Platform\" >");
                builder.Append(MapPlatForm(ticket.BetFrom));
                builder.Append("</td>");

                // External Id
                builder.Append("<td data-name=\"ExternalId\" class='special-color-");
                builder.Append(ticket.SpecialLevel.ToString(CultureInfo.InvariantCulture));
                builder.Append("'>");
                builder.Append(ticket.ExternalId);
                builder.Append("</td>");

                // Username
                builder.Append("<td data-name=\"SystemID\" class='special-color-");
                builder.Append(ticket.SpecialLevel.ToString(CultureInfo.InvariantCulture));
                builder.Append("'>");
                builder.Append(ticket.UserName);
                builder.Append("</td>");

                // Trans time
                builder.AddTrans(ticket, "<td data-name=\"TransTime\" class='special-color-0'>", "</td>");

                // Choice
                builder.AddChoice(
                    ticket,
                    TicketHelper,
                    ticketResult.TicketDataList,
                    "<td data-name=\"Choice\" class='bl-event'>",
                    "</td>",
                    false);

                // Odds
                BuildOdds(metaModel, ticketResult, ticket, builder);

                // Stake
                builder.AddStake(
                    ticket,
                    "<td data-name=\"Stake\" class='bl-stake'>",
                    "</td>");

                builder.Append("<td data-name=\"Status\" class='bl-status'>");
                builder.Append(GetLasBetMonitoringStatus(ticket));
                builder.Append("</td></tr>");
            }

            return builder.GetBetList();
        }

        /// <summary>
        /// GetClientDetailSystemParlayFunction for ticket helper.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <param name="ticket">LRF ticket object.</param>
        /// <returns>Return string.Empty if ticket is not combination mix parlay.</returns>
        private string GetClientDetailSystemParlayFunction(LastBetsMonitoringMetaModel metaModel, LrfTicket ticket)
        {
            if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
            {
                var baseCurrency = metaModel == null ? string.Empty : metaModel.BasedCurrency;
                if (!SystemConstants.BaseCurrencyList.Contains(baseCurrency.ToUpperInvariant()))
                {
                    baseCurrency = string.Empty;
                }

                const string FormatDetail = @"showCombMPDetail({0},{1},&quot;{2}&quot;,{3},&quot;{4}&quot;,&quot;{5}&quot;)";
                var betTeam = TicketHelper.GetBetTypeNameById(ticket.BetTypeId) + " - " + TicketHelper.GetCombinationMPOfferName(ticket.BetTeam);
                return string.Format(
                                FormatDetail,
                                ticket.TransId,
                                ticket.RefNo,
                                ticket.BetId,
                                0,
                                betTeam,
                                baseCurrency);
            }

            return string.Empty;
        }
    }
}