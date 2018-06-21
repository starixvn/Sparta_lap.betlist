namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base;
    using Constant.DataDomain;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.BetList;
    using Excel.BetList.Model;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Resources;
    using Service.BetList.Extensions;
    using Service.BetList.Helpers;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class BettingTicketCenterController : BaseApiController
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IBettingTicketCenterDataAccess _dataAccess;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly ITicketHelper _ticketHelper;

        private readonly IAuthorizationService _authorizationService;

        public BettingTicketCenterController()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
            _dataAccess = new BettingTicketCenterDataAccess();
            _betlistHtmlBuilder = new BetListHTMLBuilder();
            _ticketHelper = new TicketHelper();
            _cacheRefAdapter = new CacheRefAdapter();
            _authorizationService = new AuthorizationService();
        }

#pragma warning disable S107 // Methods should not have too many parameters
        public BettingTicketCenterController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IAuthorizationService authorizationService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IBettingTicketCenterDataAccess dataAccess,
            IBetListHTMLBuilder betlistHtmlBuilder,
            ICacheRefAdapter cacheRefAdapter,
            ITicketHelper ticketHelper)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(sessionService, productService, langService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _dataAccess = dataAccess;
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _cacheRefAdapter = cacheRefAdapter;
            _ticketHelper = ticketHelper;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public dynamic GetByMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid && !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetBetListForMultipleCustomers(metaModel);

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

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetBetListForSingleCustomer(metaModel);

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
            if (!metaModel.IsValid
                || !_authorizationService.HasPermissionOnData(SessionService.LicenseeSession.LicenseeId, metaModel.CustId, 0))
            {
                return null;
            }

            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var ticketResult = _dataAccess.GetBetListByRefno(metaModel);

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    ShowTotal = false,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    Title = $"{ViewResources.BettingTicketCenter}, {ViewResources.BaseCurrency}: {metaModel.BaseCurrency.ToUpperInvariant()}"
                };
                return Export(excelModel);
            }

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel)
            };
        }

        [HttpPost]
        public dynamic GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            if (!metaModel.IsValid 
                || !IsValidCurrencyId(metaModel.CurrencyId)
                || !_authorizationService.HasPermissionOnData(SessionService.LicenseeSession.LicenseeId, metaModel.CustId, 0))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetRunningBetList(metaModel);

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

        protected internal virtual BettingTicketCenterExcel CreateExcel(BettingTicketCenterExcelModel excelModel)
        {
            return new BettingTicketCenterExcel(excelModel);
        }

        private bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == BaseCurrencyType.All || SessionService.UserSession.CurrencyList.ContainsKey(currencyId);
        }

        private HttpResponseMessage Export(BettingTicketCenterExcelModel excelModel)
        {
            var excel = CreateExcel(excelModel);

            return ExcelStream(excel.Export(), string.Empty);
        }

        private List<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel, int startIndex = 1)
        {
            var showPlayerComm = SessionService.LicenseeSession.IsAllowedCustomerComm;
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