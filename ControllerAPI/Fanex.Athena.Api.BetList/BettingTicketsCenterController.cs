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

    public class BettingTicketsCenterController : ApiController
    {
        private readonly ISessionService _sessionService;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IBettingTicketsCenterDataAccess _dataAccess;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly ITicketHelper _ticketHelper;

        private readonly IAuthorizationService _authorizationService;

        public BettingTicketsCenterController()
        {
            _sessionService = new SessionService();
            _balanceUpDataAccess = new BalanceUpDataAccess();
            _dataAccess = new BettingTicketsCenterDataAccess();
            _betlistHtmlBuilder = new BetListHTMLBuilder();
            _ticketHelper = new TicketHelper();
            _cacheRefAdapter = new CacheRefAdapter();
            _authorizationService = new AuthorizationService();
        }

        public BettingTicketsCenterController(
            ISessionService sessionService,
            IAuthorizationService authorizationService,
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
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public dynamic GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid && !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = _sessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetBetListForMultipleCustomers(metaModel);

            return new
            {
                Main = BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm,
                    SumStake = ticketResult.SumStake
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        [HttpPost]
        public dynamic GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid && !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = _sessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetBetListForSingleCustomer(metaModel);

            return new
            {
                Main = BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm,
                    SumStake = ticketResult.SumStake
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        [HttpPost]
        public dynamic GetBetListByRefno(RefNoBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid
                || !_authorizationService.HasPermissionOnData(_sessionService.LicenseeSession.LicenseeId, metaModel.CustId, 0))
            {
                return null;
            }

            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.AgentListXml = _sessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var ticketResult = _dataAccess.GetBetListByRefno(metaModel);

            return new
            {
                Main = BuildTicketText(ticketResult, metaModel)
            };
        }

        [HttpPost]
        public dynamic GetRunningBetList(BaseBettingTicketMetaModel metaModel)
        {
            if (!metaModel.IsValid 
                || !IsValidCurrencyId(metaModel.CurrencyId)
                || !_authorizationService.HasPermissionOnData(_sessionService.LicenseeSession.LicenseeId, metaModel.CustId, 0))
            {
                return null;
            }

            metaModel.AgentListXml = _sessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetRunningBetList(metaModel);

            return new
            {
                Main = BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumStake = ticketResult.SumStake
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        private bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == 0 || _sessionService.UserSession.CurrencyList.ContainsKey(currencyId);
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
                    Status = _betlistHtmlBuilder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, new BetListMetaModel()),
                    Platform = PlatformType.GetPlatformName(ticket.BetFrom),
                    Live = _betlistHtmlBuilder.BuildLiveStatus(ticket)
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }
    }
}