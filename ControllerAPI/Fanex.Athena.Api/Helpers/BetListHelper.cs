namespace Fanex.Athena.Api.BetList.Helpers
{
    using System;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.BetList.Core.Entities;

    public static class BetListHelper
    {
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
    }
}