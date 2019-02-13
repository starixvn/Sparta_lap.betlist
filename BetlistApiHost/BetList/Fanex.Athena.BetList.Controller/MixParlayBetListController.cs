using System;
using System.Linq;
using System.Web.Mvc;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Utility.Interfaces;

namespace Fanex.Athena.BetList.Controller
{
    public class MixParlayBetListController : BaseController
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IBetListService _betListService;

        public MixParlayBetListController(ICacheRefAdapter cacheRefAdapter, IBetListService betListService)
        {
            _cacheRefAdapter = cacheRefAdapter;
            _betListService = betListService;
        }

        [HttpPost]
        public JsonResult Detail(MixParlayDetailMetaModel metaModel)
        {
            var ticketResult = _betListService.GetMixParlayDetailTicket(metaModel);
            var result = _betListService.BuildTicketTextList(ticketResult: ticketResult, showOdds: true);
            ClearData(ticketResult);
            return Json(result);
        }

        [HttpPost]
        public JsonResult NewParlayDetail(NewParlayDetailMetaModel metaModel)
        {
            var texts = (string[])_cacheRefAdapter.GetCombinationTypeCombo(metaModel.BetTeam, metaModel.BetCheck, metaModel.Handicap1, metaModel.Handicap2).Clone();

            var ticketResult = _betListService.GetMixParlayDetailTicket(metaModel);
            var tickets = ticketResult.MainTicketList.Where(x => x.Stake != 0).ToList();

            var textRenderPattern = "<p class='system-parlay-ticket-infor'>"
                                    + "<span class='system-parlay-combo-type'>{0}</span> x "
                                    + "<span class='system-parlay-unit-stake'>{1} {2}</span>"
                                    + "</p>";
            if (tickets.Any())
            {
                if (metaModel.Handicap1 == 0)
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
                Data = _betListService.BuildTicketTextList(ticketResult: ticketResult, showOdds: true),
                Texts = texts
            };
            ClearData(ticketResult);
            return Json(result);
        }
    }
}