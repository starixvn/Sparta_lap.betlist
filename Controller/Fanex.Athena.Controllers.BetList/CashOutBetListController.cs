namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.BetList.Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class CashOutBetListController : BetListBaseController
    {
        private readonly ICashOutBetListDataAccess _cashOutBetListDataAccess;

        public CashOutBetListController()
        {
            _cashOutBetListDataAccess = new CashOutBetListDataAccess();
        }

        public CashOutBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            ICashOutBetListDataAccess cashOutBetListDataAccess)
        : base(
            languageService, 
            sessionService, 
            appSetting, 
            clientViewHelper, 
            authorizationService, 
            productService, 
            betListService,             
            balanceUpDataAccess, 
            cacheRefAdapter)
        {
            _cashOutBetListDataAccess = cashOutBetListDataAccess;
        }

        [HttpGet]
        public ActionResult Detail(CashOutBetListDetailMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            var viewModel = _cashOutBetListDataAccess.GetDetail(metaModel);
            if (viewModel.OriginalTicket != null)
            {
                viewModel.IsHighlightOriginalTicket = metaModel.TransId == viewModel.OriginalTicket.TransId ||
                    IsCashOutBetTicket(metaModel.BetdaqId, metaModel.BetType);
            }

            viewModel.OriginalTicketText = BuilOriginalTicketText(viewModel.OriginalTicket);
            viewModel.CashOutTicketTextList = BuildCashOutTicketTextList(viewModel.CashOutTicketList, metaModel.TransId);

            viewModel.IsCashOut = metaModel.IsCashOut;
            return this.View("~/Views/BetList/CashOutDetail/Main.cshtml", viewModel);
        }

        protected internal virtual BetListHTMLBuilder CreateBetListHTMLBuilder()
        {
            return new BetListHTMLBuilder();
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
                var statusHtml = builder.CreateStatusBuilder(ticket.BetType).Render(betListTicket, ticketHelper, null, true);
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

        private TicketText BuilOriginalTicketText(LrfTicket ticket)
        {
            if (ticket == null)
            {
                return new TicketText();
            }

            var licenseeSession = SessionService.LicenseeSession;
            var showPlayerComm = licenseeSession.IsAllowedCustomerComm;
            var builder = CreateBetListHTMLBuilder();
            var ticketHelper = new TicketHelper
            {
                HideStatusCashOutHistory = true
            };

            var statusHtml = builder.CreateStatusBuilder(ticket.BetTypeId).Render(ticket, ticketHelper, null, true);
            statusHtml.StatusResult = null;
            return new TicketText
            {
                ExternalId = ticket.ExternalId,
                SystemId = ticket.UserName,
                TransTime = builder.BuildTrans(ticket),
                Status = statusHtml.ToString(),
                Choice = builder.BuildChoice(ticket, ticketHelper, null, false),
                Odds = builder.BuildOdds(ticket, null, CacheRefAdapter.GetOddsTypeName),
                Stake = builder.BuildStake(ticket),
                WinLoss = builder.BuildCustomerWL(ticket, showPlayerComm)
            };
        }
    }
}