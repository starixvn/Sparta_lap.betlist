using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Service
{
    public class BettingTicketCenterService : IBettingTicketCenterService
    {
        private readonly IBettingTicketCenterDataAccess _dataAccess;

        private readonly IBettingTicketCenterMultiCustomerDataAccess _multiCustomerDataAccess;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ITicketHelper _ticketHelper;

#pragma warning disable CS3001 // Argument type is not CLS-compliant

        public BettingTicketCenterService(
            IBettingTicketCenterDataAccess dataAccess,
            IBettingTicketCenterMultiCustomerDataAccess multiCustomerDataAccess,
            IBetListHTMLBuilder betlistHtmlBuilder,
            ITicketHelper ticketHelper)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            _dataAccess = dataAccess;
            _multiCustomerDataAccess = multiCustomerDataAccess;
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _ticketHelper = ticketHelper;
        }

        public async Task<GroupTicketResult> GetBetListForMultipleCustomers(
            MultipleCustomersBetListMetaModel metaModel,
            int rowPerGroup,
            IDictionary<int, string> currencyList)
        {
            return await _multiCustomerDataAccess.GetBetListForMultipleCustomers(metaModel, rowPerGroup, currencyList);
        }

        public async Task<TicketResult> GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _multiCustomerDataAccess.GetBetListForMultipleCustomers(metaModel, currencyList);
        }

        public int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            return _multiCustomerDataAccess.GetTotalRowForMultipleCustomers(metaModel);
        }

        public async Task<TicketResult> GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _dataAccess.GetBetListForSingleCustomer(metaModel, currencyList);
        }

        public async Task<TicketResult> GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _dataAccess.GetBetListByRefno(metaModel, currencyList);
        }

        public async Task<TicketResult> GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _dataAccess.GetRunningBetList(metaModel, currencyList);
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
                    TransTime = _betlistHtmlBuilder.BuildTransTime(ticket),
                    RefNo = _betlistHtmlBuilder.BuildRefNo(ticket),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = _betlistHtmlBuilder.BuildOddsValue(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById),
                    OddsType = _betlistHtmlBuilder.BuildOddsTypeName(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById),
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