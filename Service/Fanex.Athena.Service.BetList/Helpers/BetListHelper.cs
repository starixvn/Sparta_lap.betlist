using System.Linq;

namespace Fanex.Athena.Service.BetList.Helpers
{
    using System;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.BetList.Core.Entities;
    using Model.Core.DataDomain;

    public static class BetListHelper
    {
        private const int MinColossusBetBetType = 18000;

        private const int MaxColossusBetBetType = 18007;

        [CLSCompliant(false)]
        public static string GenerateClientDetailSystemParlayFunction(
            ITicketHelper tickerHelper,
            string baseCurrency,
            LrfTicket ticket,
            long custid)
        {
            const string FormatDetail = @"showCombMPDetail({0},{1},{2},{3},&quot;{4}&quot;,&quot;{5}&quot;)";
            var comboName = tickerHelper.GetCombinationMPOfferName(ticket.BetTeam);
            var betTeam = tickerHelper.GetBetTypeNameById(ticket.BetTypeId);
            if (!string.IsNullOrEmpty(comboName))
            {
                betTeam += " - " + comboName;
            }

            return string.Format(
                FormatDetail,
                ticket.TransId,
                ticket.RefNo,
                ticket.BetId,
                custid,
                betTeam,
                baseCurrency);
        }

        public static bool IsBoughtTicket(int bettype)
        {
            return bettype == (int)BetType.BetTradeBought || bettype == (int)BetType.CashOutBought;
        }

        public static bool IsColossusBet(int bettype)
        {
            return bettype >= MinColossusBetBetType && bettype <= MaxColossusBetBetType;
        }
    }
}