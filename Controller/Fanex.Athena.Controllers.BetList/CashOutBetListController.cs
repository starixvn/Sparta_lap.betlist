namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.BetList.Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using Model.Core.DataDomain;
    using Resources;
    using Service.BetList;
    using Service.BetList.Interfaces;

    public class CashOutBetListController : BetListBaseController
    {
        private readonly string _title =
            ViewResources.ProductCO + " - " + ViewResources.BetList + " - " + ViewResources.DetailTickets;

        private readonly ICashOutBetListService _cashOutBetListService;

        public CashOutBetListController()
        {
            _cashOutBetListService = new CashOutBetListService();
        }

        public CashOutBetListController(ICashOutBetListService cashOutBetListService)
        {
            _cashOutBetListService = cashOutBetListService;
        }

        [HttpGet]
        public ActionResult Detail(CashOutBetListDetailMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            var viewModel = _cashOutBetListService.GetDetail(metaModel);

            viewModel.TicketTextList = BuilTicketTextList(viewModel.DataList);
            viewModel.CashOutTicketTextList = BuildCashOutTicketTextList(viewModel.CashOutTicketList);

            viewModel.IsBoughtTicket = metaModel.BetdaqId > 0 || metaModel.BetType == (int)BetType.CashOutBought;
            return this.View("~/Views/BetList/CashOutDetail/Main.cshtml", viewModel);
        }

        private IList<CashOutTicketText> BuildCashOutTicketTextList(IEnumerable<CashOutTicket> dataList)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var showPlayerComm = licenseeSession.IsAllowedCustomerComm;
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<CashOutTicketText>();
            var ticketHelper = new TicketHelper
            {
                HideStatusCashOutHistory = true
            };
            foreach (var ticket in dataList)
            {
                var betListTicket = new LrfTicket
                {
                    BetTypeId = ticket.BetType,
                    Status = ticket.Status
                };
                var statusHtml = builder.CreateStatusBuilder(ticket.BetType).Render(betListTicket, ticketHelper, null, false);
                statusHtml.StatusResult = null;
                var ticketText = new CashOutTicketText
                {
                    TransTime = ViewResources.RefNo + ":" + ticket.RefNo + Environment.NewLine + "<div class='time'>" + ticket.TransDate + "</div>",
                    Stake = ticket.Stake.ToString("0.00"),
                    Price = ticket.Price.ToString("0.00"),
                    WinLoss = ticket.WinLoss.ToString("0.00"),
                    Status = statusHtml.ToString()
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        private IList<TicketText> BuilTicketTextList(IEnumerable<LrfTicket> dataList)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var showPlayerComm = licenseeSession.IsAllowedCustomerComm;
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            var ticketHelper = new TicketHelper
            {
                HideStatusCashOutHistory = true
            };
            foreach (var ticket in dataList)
            {
                var statusHtml = builder.CreateStatusBuilder(ticket.BetTypeId).Render(ticket, ticketHelper, null, false);
                statusHtml.StatusResult = null;
                var ticketText = new TicketText
                {
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = builder.BuildTrans(ticket),
                    Status = statusHtml.ToString(),
                    Choice = builder.BuildChoice(ticket, ticketHelper, null, false),
                    Odds = builder.BuildOdds(ticket, null, CacheRef.GetOddsTypeName),
                    Stake = builder.BuildStake(ticket),
                    WinLoss = builder.BuildCustomerWL(ticket, showPlayerComm)
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }
    }
}