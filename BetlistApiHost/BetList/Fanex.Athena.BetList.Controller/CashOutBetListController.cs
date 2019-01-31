using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.Common.DataAccess.Interfaces;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Controller
{
    public class CashOutBetListController : BaseController
    {
        private readonly ICashOutBetListDataAccess _cashOutBetListDataAccess;

        private readonly ITicketHelper _ticketHelper;

        private readonly IBetListHTMLBuilder _betListHTMLBuilder;

        public CashOutBetListController(
            IBalanceUpDataAccess balanceUpDataAccess,
            ICashOutBetListDataAccess cashOutBetListDataAccess,
            ITicketHelper ticketHelper,
            IBetListHTMLBuilder betListHTMLBuilder)
        {
            _cashOutBetListDataAccess = cashOutBetListDataAccess;
            _ticketHelper = ticketHelper;
            _betListHTMLBuilder = betListHTMLBuilder;
        }

        [HttpPost]
        public JsonResult Detail(CashOutBetListDetailMetaModel metaModel)
        {
            var viewModel = _cashOutBetListDataAccess.GetDetail(metaModel, metaModel.UserCurrencyList);
            if (viewModel.OriginalTicket != null)
            {
                viewModel.IsHighlightOriginalTicket = metaModel.TransId == viewModel.OriginalTicket.TransId ||
                    IsCashOutBetTicket(metaModel.BetdaqId, metaModel.BetType);
            }

            viewModel.OriginalTicketText = BuilOriginalTicketText(viewModel.OriginalTicket, metaModel.HasPlayerComm);
            viewModel.CashOutTicketTextList = BuildCashOutTicketTextList(viewModel.CashOutTicketList, metaModel.TransId);

            viewModel.IsCashOut = metaModel.IsCashOut;
            return Json(viewModel);
        }

        private static bool IsCashOutBetTicket(long betdaqid, int bettype)
        {
            var cashOutBetTypes = new[] { 900, 912 };
            return betdaqid == 1 && !cashOutBetTypes.Contains(bettype);
        }

        private IList<CashOutTicketText> BuildCashOutTicketTextList(IEnumerable<CashOutTicket> dataList, long transId)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<CashOutTicketText>();
            }

            var ticketTextList = new List<CashOutTicketText>();
            _ticketHelper.HideStatusCashOutHistory = true;
            foreach (var ticket in dataList)
            {
                var betListTicket = new LrfTicket
                {
                    BetTypeId = ticket.BetType,
                    Status = ticket.Status
                };
                var statusHtml = _betListHTMLBuilder.CreateStatusBuilder(ticket.BetType).Render(betListTicket, _ticketHelper, null, true);
                statusHtml.StatusResult = null;
                var ticketText = new CashOutTicketText
                {
                    TransTime = ViewResources.RefNo + ":" + ticket.TransId + Environment.NewLine + "<div class='time'>" + ticket.TransDate + "</div>",
                    Stake = ticket.Stake.ToString("0.00"),
                    Price = ticket.Price.ToString("0.00"),
                    WinLoss = ticket.WinLoss.ToString("0.00"),
                    Status = statusHtml.ToString(),
                    IsHighlight = transId == ticket.TransId
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        private TicketText BuilOriginalTicketText(LrfTicket ticket, bool hasPlayerComm)
        {
            if (ticket == null)
            {
                return new TicketText();
            }

            var builder = new BetListHTMLBuilder();
            _ticketHelper.HideStatusCashOutHistory = true;

            var statusHtml = builder.CreateStatusBuilder(ticket.BetTypeId).Render(ticket, _ticketHelper, null, true);
            statusHtml.StatusResult = null;
            return new TicketText
            {
                ExternalId = ticket.ExternalId,
                SystemId = ticket.UserName,
                TransTime = builder.BuildTrans(ticket),
                Status = statusHtml.ToString(),
                Choice = builder.BuildChoice(ticket, _ticketHelper, null, false),
                Odds = builder.BuildOdds(ticket, null, _ticketHelper.GetValidOddsTypeNameById),
                Stake = builder.BuildStake(ticket),
                WinLoss = builder.BuildCustomerWL(ticket, hasPlayerComm)
            };
        }
    }
}