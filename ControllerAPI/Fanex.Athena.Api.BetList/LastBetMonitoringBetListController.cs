namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Resources;
    using Service.BetList;
    using Service.BetList.Helpers;
    using Service.BetList.Interfaces;
    using BetListCacheRef = Fanex.BetList.Core.TicketHelper.CacheRef;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportLastBetsMonitoring)]
    public class LastBetMonitoringBetListController : BaseApiController
    {
        private static readonly IEnumerable<int> ConvertingDecimalOddsBetType = new int[] { 1, 2, 3, 7, 8, 12, 20 };

        private readonly IMatchService _matchService;

        private readonly ILastBetsMonitoringBetListService _lastBetsMonitoringBetListService;

        private readonly ITicketHelper _ticketHelper;

        public LastBetMonitoringBetListController()
        {
            _matchService = new MatchService();
            _lastBetsMonitoringBetListService = new LastBetsMonitoringBetListService();
            _ticketHelper = new TicketHelper();
        }

        [HttpGet]
        public dynamic GetRunningMatchList(int sportId)
        {
            var matchList = _matchService.GetAllRunningMatch(sportId);

            var leagueListNormal = matchList.Where(i => !i.IsOutRight).Select(m => m.LeagueId).Distinct()
                                      .Select(l => new League
                                      {
                                          IsOutRight = false,
                                          LeagueId = l,
                                          LeagueName = Model.Core.Helper.CacheRef.GetLeagueName(l)
                                      }).ToList();
            var leagueListOutRight = matchList.Where(i => i.IsOutRight).Select(m => m.LeagueId).Distinct()

                                      .Select(l => new League
                                      {
                                          IsOutRight = true,
                                          LeagueId = l,
                                          LeagueName = Model.Core.Helper.CacheRef.GetLeagueName(l)
                                      }).ToList();

            var leagueList = leagueListNormal.Union(leagueListOutRight);

            return new
            {
                MatchList = matchList.OrderBy(i => i.MatchName),
                LeagueList = leagueList.OrderBy(i => i.LeagueName)
            };
        }

        [HttpPost]
        public dynamic Index(LastBetsMonitoringMetaModel metaModel)
        {
            if (string.IsNullOrWhiteSpace(metaModel.Currency))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            }
            else
            {
                var currencyIdList = metaModel.Currency.Split(',').Select(int.Parse);
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(currencyIdList);
            }

            var ticketResult = _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel);

            if (ticketResult.MainTicketList.Any())
            {
                var rows = GetLastMonitorBetListContent(metaModel, ticketResult);
                return new
                {
                    NewRow = true,
                    Rows = rows,
                    LastTransId = ticketResult.MainTicketList.Max(t => t.TransId),
                };
            }
            else
            {
                return new
                {
                    NewRow = false
                };
            }
        }

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
                    BetListCacheRef.GetOddsTypeName);
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
                case "z":
                    return ViewResources.China;

                case "d":
                case "g":
                case "x":
                    return ViewResources.Asia;

                case "e":
                case "u":
                case "y":
                    return ViewResources.Euro;

                case "m":
                    return ViewResources.Wap.ToUpper();

                case "s":
                case "l":
                    return ViewResources.SmartSite;

                case "r":
                    return ViewResources.Tablet;

                default:
                    return ViewResources.Others;
            }
        }

        private static string GetLasBetMonitoringStatus(ITicket ticket)
        {
            const string PatternStatusBetlist = "<label style='color:{0}'>{1}</label>";
            if (ticket.BetTypeId == (int)BetType.MixParlay)
            {
                switch (ticket.LiveHomeScore)
                {
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

        private string GetLastMonitorBetListContent(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult)
        {
            var builder = new BetListHTMLBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                _ticketHelper.ClientDetailSystemParlayFunction = GetClientDetailSystemParlayFunction(metaModel, ticket);

                var row = $"<tr class='content-row monitor-hightlight' id='trans-{ticket.TransId}' data-transid='{ticket.TransId}'>";
                builder.Append(row);

                builder.Append($"<td data-name='Platform'>{MapPlatForm(ticket.BetFrom)}</td>");
                builder.Append($"<td data-name='ExternalId' class='special-color-{ticket.SpecialLevel}'>{ticket.ExternalId}</td>");
                builder.Append($"<td data-name='SystemId' class='special-color-{ticket.SpecialLevel}'>{ticket.UserName}</td>");
                builder.AddTrans(ticket, "<td data-name='TransTime' class='special-color-0'>", "</td>");
                builder.AddChoice(
                    ticket,
                    _ticketHelper,
                    ticketResult.TicketDataList,
                    "<td data-name='Choice' class='bl-event'>",
                    "</td>",
                    false);
                BuildOdds(metaModel, ticketResult, ticket, builder);
                builder.AddStake(ticket, "<td data-name='Stake' class='bl-stake'>", "</td>");
                builder.Append($"<td data-name='Status' class='bl-status'>{GetLasBetMonitoringStatus(ticket)}</td></tr>");
            }

            return builder.GetBetList();
        }

        private string GetClientDetailSystemParlayFunction(LastBetsMonitoringMetaModel metaModel, LrfTicket ticket)
        {
            if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
            {
                var baseCurrency = metaModel == null ? string.Empty : metaModel.BasedCurrency;
                if (!SystemConstants.BaseCurrencyList.Contains(baseCurrency.ToUpperInvariant()))
                {
                    baseCurrency = string.Empty;
                }

                return BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, 0);
            }

            return string.Empty;
        }
    }
}