using System;
using System.Collections.Generic;
using Fanex.Athena.Constant.DataDomain;
using Fanex.Athena.DataAccess.BetList.Interfaces;
using Fanex.Athena.Model.BetList.DTO;
using Fanex.Athena.Model.BetList.MetaModel;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Resources;
using Fanex.Athena.Service.BetList.Extensions;
using Fanex.Athena.Service.BetList.Helpers;
using Fanex.Athena.Service.BetList.Interfaces;
using Fanex.Athena.VendorAdapter.Interfaces;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.Service.BetList
{
    public class BettingTicketCenterService : IBettingTicketCenterService
    {
        private readonly IBettingTicketCenterDataAccess _dataAccess;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly ITicketHelper _ticketHelper;

#pragma warning disable CS3001 // Argument type is not CLS-compliant

        public BettingTicketCenterService(
            IBettingTicketCenterDataAccess dataAccess,
            IBetListHTMLBuilder betlistHtmlBuilder,
            ICacheRefAdapter cacheRefAdapter,
            ITicketHelper ticketHelper)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            _dataAccess = dataAccess;
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _cacheRefAdapter = cacheRefAdapter;
            _ticketHelper = ticketHelper;
        }
        public GroupTicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup, IDictionary<int, string> currencyList)
        {

            return _dataAccess.GetBetListForMultipleCustomers(metaModel, rowPerGroup, currencyList);
        }

        public TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return _dataAccess.GetBetListForMultipleCustomers(metaModel, currencyList);
        }

        public int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            return _dataAccess.GetTotalRowForMultipleCustomers(metaModel);
        }

        public TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return _dataAccess.GetBetListForSingleCustomer(metaModel, currencyList);
        }

        public TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return _dataAccess.GetBetListByRefno(metaModel, currencyList);
        }

        public TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return _dataAccess.GetRunningBetList(metaModel, currencyList);
        }

        public bool IsValidCurrencyId(int currencyId, IDictionary<int, string> currencyList)
        {
            return currencyId == BaseCurrencyType.All || currencyList.ContainsKey(currencyId);
        }

        public string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm)
        {
            var ticket = new LrfTicket { Winlost = ticketResult.SumWinLoss, PlayerComm = ticketResult.SumPlayerComm };
            return _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm);
        }

        public IList<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel, bool isAllowedCustomerComm)
        {
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            var ticketTextList = new List<TicketText>();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
                {
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(
                        _ticketHelper,
                        metaModel.BaseCurrency,
                        ticket,
                        ticket.CustId);
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
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm),
                    Status = _betlistHtmlBuilder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, new BetListMetaModel()),
                    Platform = PlatformType.GetPlatformName(ticket.BetFrom),
                    Live = _betlistHtmlBuilder.BuildLiveStatus(ticket)
                };

                ticketTextList.Add(ticketText);
            }
            return ticketTextList;
        }

        public string GetTitle(BaseBettingTicketMetaModel metaModel)
        {
            var date = metaModel.FromDate == DateTime.MinValue
                ? string.Empty
                : ", " + metaModel.FromDate.ToString(FormatKey.DateFormat) + "-->" + metaModel.ToDate.ToString(FormatKey.DateFormat);
            var currency = string.IsNullOrWhiteSpace(metaModel.CurrencyName) ? string.Empty : ", " + ViewResources.Currency + ": " + metaModel.CurrencyName;
            var baseCurrency = string.IsNullOrWhiteSpace(metaModel.BaseCurrency)
                ? string.Empty
                : ", " + ViewResources.BaseCurrency + ": " + metaModel.BaseCurrency.ToUpperInvariant();

            return ViewResources.BettingTicketCenter + date + currency + baseCurrency;
        }
    }
}