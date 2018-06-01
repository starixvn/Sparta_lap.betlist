namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Constant.DataDomain;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.BetList.Extensions;
    using Service.BetList.Helpers;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class BettingTicketsCenterReportController : ApiController
    {
        private readonly ISessionService _sessionService;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IBettingTicketsCenterDataAccess _dataAccess;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly ITicketHelper _ticketHelper;

        public BettingTicketsCenterReportController()
        {
            _sessionService = new SessionService();
            _balanceUpDataAccess = new BalanceUpDataAccess();
            _dataAccess = new BettingTicketsCenterDataAccess();
            _betlistHtmlBuilder = new BetListHTMLBuilder();
            _ticketHelper = new TicketHelper();
            _cacheRefAdapter = new CacheRefAdapter();
        }

        public BettingTicketsCenterReportController(
            ISessionService sessionService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IBettingTicketsCenterDataAccess dataAccess,
            IBetListHTMLBuilder betlistHtmlBuilder,
            ICacheRefAdapter cacheRefAdapter,
            ITicketHelper ticketHelper)
        {
            _sessionService = sessionService;
            _balanceUpDataAccess = balanceUpDataAccess;
            _dataAccess = dataAccess;
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _cacheRefAdapter = cacheRefAdapter;
            _ticketHelper = ticketHelper;
        }

        [HttpPost]
        public dynamic GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.AgentListXml = _sessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var ticketResult = _dataAccess.GetBetListForMultipleCustomers(metaModel);

            return new
            {
                Main = BuildTicketText(ticketResult, metaModel)
            };
        }

        [HttpPost]
        public dynamic GetBetListForSingleCustomer()
        {
            var ticketResult = _dataAccess.GetBetListForSingleCustomer();

            return new
            {
                Main = ticketResult
            };
        }

        [HttpPost]
        public dynamic GetBetListByRefno()
        {
            var ticketResult = _dataAccess.GetBetListByRefno();

            return new
            {
                Main = ticketResult
            };
        }

        [HttpPost]
        public dynamic GetRunningBetList()
        {
            var ticketResult = _dataAccess.GetRunningBetList();

            return new
            {
                Main = ticketResult
            };
        }

        private List<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel, int startIndex = 1)
        {
            var showPlayerComm = _sessionService.LicenseeSession.IsAllowedCustomerComm;
            var ticketTextList = new List<TicketText>();

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
                {
                    var baseCurrency = metaModel.IsConvert ? metaModel.BaseCurrency : ticket.Currency;
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, ticket.CustId);
                }

                BetListMetaModel betListMetaModel = ticket.BetTypeId == (int)BetType.CasinoGames
                    ? CreateParamForCasinoGamesStatus(ticket, metaModel.IsConvert ? metaModel.BaseCurrency : string.Empty)
                    : null;

                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = _betlistHtmlBuilder.BuildTrans(ticket),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = _betlistHtmlBuilder.BuildOdds(ticket, ticketResult.TicketDataList, _cacheRefAdapter.GetOddsTypeName),
                    Stake = _betlistHtmlBuilder.BuildStake(ticket),
                    LicenseeWL = _betlistHtmlBuilder.BuildLicenseeWL(ticket),
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, showPlayerComm),
                    Status = _betlistHtmlBuilder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, betListMetaModel),
                    Platform = PlatformType.GetPlatformName(ticket.BetFrom),
                    Live = _betlistHtmlBuilder.BuildLiveStatus(ticket)
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        private BetListMetaModel CreateParamForCasinoGamesStatus(LrfTicket ticket, string baseCurrency)
        {
            return new BetListMetaModel
            {
                FromDate = ticket.WinlostDate,
                ToDate = ticket.WinlostDate,
                ExternalId = ticket.ExternalId,
                SystemId = ticket.UserName,
                CustId = (int)ticket.CustId,
                CurrencyId = ticket.CurrencyId,
                CurrencyName = ticket.Currency,
                BaseCurrency = baseCurrency
            };
        }
    }
}