using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;

namespace Fanex.Athena.BetList.Service
{
    public class BettingTicketCenterService : IBettingTicketCenterService
    {
        private readonly IBettingTicketCenterDataAccess _dataAccess;

        private readonly IBettingTicketCenterMultiCustomerDataAccess _multiCustomerDataAccess;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        [CLSCompliant(false)]
        public BettingTicketCenterService(
            IBettingTicketCenterDataAccess dataAccess,
            IBettingTicketCenterMultiCustomerDataAccess multiCustomerDataAccess,
            IBetListHTMLBuilder betlistHtmlBuilder
            )
        {
            _dataAccess = dataAccess;
            _multiCustomerDataAccess = multiCustomerDataAccess;
            _betlistHtmlBuilder = betlistHtmlBuilder;
        }

        public async Task<GroupTicketResult> GetBetListForMultipleCustomersAsync(
            MultipleCustomersBetListMetaModel metaModel,
            int rowPerGroup,
            IDictionary<int, string> currencyList)
        {
            return await _multiCustomerDataAccess.GetBetListForMultipleCustomersAsync(metaModel, rowPerGroup, currencyList).ConfigureAwait(false);
        }

        public async Task<TicketResult> GetBetListForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _multiCustomerDataAccess.GetBetListForMultipleCustomersAsync(metaModel, currencyList).ConfigureAwait(false);
        }

        public async Task<int> GetTotalRowForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel)
        {
            return await _multiCustomerDataAccess.GetTotalRowForMultipleCustomersAsync(metaModel).ConfigureAwait(false);
        }

        public async Task<TicketResult> GetBetListForSingleCustomerAsync(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _dataAccess.GetBetListForSingleCustomerAsync(metaModel, currencyList);
        }

        public async Task<TicketResult> GetBetListByRefnoAsync(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _dataAccess.GetBetListByRefnoAsync(metaModel, currencyList).ConfigureAwait(false);
        }

        public async Task<TicketResult> GetRunningBetListAsync(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            return await _dataAccess.GetRunningBetListAsync(metaModel, currencyList).ConfigureAwait(false);
        }

        public string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm)
        {
            var ticket = new LrfTicket { Winlost = ticketResult.SumWinLoss, PlayerComm = ticketResult.SumPlayerComm };
            return _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm);
        }

        public string GetTitle(BaseBettingTicketMetaModel metaModel)
        {
            var date = metaModel.FromDate == DateTime.MinValue
                ? string.Empty
                : $", {metaModel.FromDate.ToString(FormatKey.DateFormat)}-->{metaModel.ToDate.ToString(FormatKey.DateFormat)}";

            var baseCurrencyName = metaModel.IsConvertAll ? metaModel.BaseCurrencyName : string.Empty;
            var baseCurrencyText = !string.IsNullOrEmpty(metaModel.BaseCurrency) ? metaModel.BaseCurrency.ToUpperInvariant() : baseCurrencyName;

            var baseCurrency = string.IsNullOrEmpty(baseCurrencyText)
                ? string.Empty
                : $", {ViewResources.BaseCurrency}: {baseCurrencyText}";

            return ViewResources.BettingTicketCenter + date + baseCurrency;
        }
    }
}