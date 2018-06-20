namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Constant.DataDomain;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.BetList;
    using Service.BetList.Extensions;
    using Service.BetList.Helpers;
    using Service.BetList.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class BettingTicketsCenterController : ApiController
    {
        private readonly ISessionService _sessionService;

        private readonly IBettingTicketsCenterService _service;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly ITicketHelper _ticketHelper;

        public BettingTicketsCenterController()
        {
            _sessionService = new SessionService();
            _service = new BettingTicketsCenterService();
            _betlistHtmlBuilder = new BetListHTMLBuilder();
            _ticketHelper = new TicketHelper();
            _cacheRefAdapter = new CacheRefAdapter();
        }

        public BettingTicketsCenterController(
            ISessionService sessionService,
            IBettingTicketsCenterService service,
            IBetListHTMLBuilder betlistHtmlBuilder,
            ICacheRefAdapter cacheRefAdapter,
            ITicketHelper ticketHelper)
        {
            _sessionService = sessionService;
            _service = service;
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _cacheRefAdapter = cacheRefAdapter;
            _ticketHelper = ticketHelper;
        }

        [HttpPost]
        public dynamic GetByMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid && !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            UpdateMetaModel(metaModel);
            var ticketResult = _service.GetBetListForMultipleCustomers(metaModel);

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel),
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
        public dynamic GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid && !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            UpdateMetaModel(metaModel);
            var ticketResult = _service.GetBetListForSingleCustomer(metaModel);

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel),
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
        public dynamic GetByRefno(RefNoBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return null;
            }

            UpdateMetaModel(metaModel);
            var ticketResult = _service.GetBetListByRefno(metaModel);

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel)
            };
        }

        [HttpPost]
        public dynamic GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            UpdateMetaModel(metaModel);
            var ticketResult = _service.GetRunningBetList(metaModel);

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumStake = ticketResult.SumStake
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        private void UpdateMetaModel(BaseBettingTicketMetaModel metaModel)
        {
            metaModel.AgentListXml = _sessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.LicenseeId = _sessionService.LicenseeSession.LicenseeId;
        }

        private bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == BaseCurrencyType.All ||
                KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(
                    _sessionService.UserSession.CurrencyList, _sessionService.LicenseeSession.LicenseeId, currencyId);
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