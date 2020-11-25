using System;
using System.Collections.Generic;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Service
{
    public class TicketBuilderService : ITicketBuilderService
    {
        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;
        private readonly ITicketHelper _ticketHelper;

        [CLSCompliant(false)]
        public TicketBuilderService(
            IBetListHTMLBuilder betlistHtmlBuilder,
            ITicketHelper ticketHelper)
        {
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _ticketHelper = ticketHelper;
        }

        public IList<TicketText> BuildTicketText(TicketResult ticketResult, ICommonValueMetaModel metaModel, bool isAllowedCustomerComm)
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
                    Stake = _betlistHtmlBuilder.BuildStake(ticket, _ticketHelper),
                    LicenseeWL = _betlistHtmlBuilder.BuildLicenseeWL(ticket),
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm),
                    Status = _betlistHtmlBuilder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, new BetListMetaModel()),
                    Platform = PlatformType.GetPlatformName(ticket.BetFrom),
                    Live = _betlistHtmlBuilder.BuildLiveStatus(ticket),
                    LicenseeTxId = _betlistHtmlBuilder.BuildLicenseeTxId(ticket)
                };

                if (ticket.BetTypeId == (int)BetType.PPContribution)
                {
                    BuildPPContributionTicket(ticketText);
                }

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        public List<TicketText> BuildTicketText(
            TicketResult ticketResult,
            BetListMetaModel metaModel = null,
            int startIndex = 1,
            bool showOdds = false,
            bool isAllowedCustomerComm = false,
            bool isMobileView = false)
        {
            var baseCurrency = metaModel == null ? string.Empty : metaModel.BaseCurrency;
            var ticketTextList = new List<TicketText>();

            _ticketHelper.IsShowOddsWithChoice = showOdds;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay && metaModel != null)
                {
                    var custid = ticket.CustId != 0 ? ticket.CustId : metaModel.CustId;
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, custid);
                }

                var odds = BuildOddsText(ticket, isMobileView);
                var oddsTypeName = BuildOddsTypeNameText(ticket, isMobileView);

                var ticketText = new TicketText
                {
                    RefNo = BuildRefNoText(ticket, isMobileView),
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = BuildTransTimeText(ticket, isMobileView),
                    Status = _betlistHtmlBuilder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, metaModel),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = odds,
                    OddsType = oddsTypeName,
                    Stake = _betlistHtmlBuilder.BuildStake(ticket, _ticketHelper),
                    LicenseeWL = _betlistHtmlBuilder.BuildLicenseeWL(ticket),
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm),
                    BetEffective = _betlistHtmlBuilder.BuildBetEffective(ticket)
                };

                if (ticket.BetTypeId == (int)BetType.PPContribution)
                {
                    BuildPPContributionTicket(ticketText);
                }

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        public string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm)
        {
            var ticket = new LrfTicket { Winlost = ticketResult.SumWinLoss, PlayerComm = ticketResult.SumPlayerComm };
            return _betlistHtmlBuilder.BuildCustomerWL(ticket, isAllowedCustomerComm);
        }

        private static void BuildPPContributionTicket(TicketText ticketText)
        {
            const string HyphenTextHTML = "<div class='underdog'>-</div>";

            ticketText.LicenseeWL = HyphenTextHTML;
            ticketText.WinLoss = HyphenTextHTML;
        }

        private string BuildOddsText(LrfTicket ticket, bool isMobileView)
        {
            return isMobileView
                   ? _betlistHtmlBuilder.BuildOddsValue(ticket, null, _ticketHelper.GetValidOddsTypeNameById)
                   : _betlistHtmlBuilder.BuildOdds(ticket, null, _ticketHelper.GetValidOddsTypeNameById);
        }

        private string BuildOddsTypeNameText(LrfTicket ticket, bool isMobileView)
        {
            return isMobileView
                ? _betlistHtmlBuilder.BuildOddsTypeName(ticket, null, _ticketHelper.GetValidOddsTypeNameById)
                : null;
        }

        private string BuildTransTimeText(LrfTicket ticket, bool isMobileView)
        {
            return isMobileView ? _betlistHtmlBuilder.BuildTransTime(ticket) : _betlistHtmlBuilder.BuildTrans(ticket);
        }

        private string BuildRefNoText(LrfTicket ticket, bool isMobileView)
        {
            return isMobileView ? _betlistHtmlBuilder.BuildRefNo(ticket) : null;
        }
    }
}
