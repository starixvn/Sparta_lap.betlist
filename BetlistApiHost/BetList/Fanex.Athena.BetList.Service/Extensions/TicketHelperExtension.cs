namespace Fanex.Athena.BetList.Service.Extensions
{
    using System;
    using Fanex.BetList.Core.Entities;

    public static class TicketHelperExtension
    {
        [CLSCompliant(false)]
        public static string GetValidOddsTypeNameById(this ITicketHelper ticketHelper, object oddsTypeId)
        {
            if (oddsTypeId == null || oddsTypeId.ToString().Trim() == string.Empty || oddsTypeId.ToString() == "0")
            {
                return string.Empty;
            }

            return ticketHelper.GetOddsTypeNameById(oddsTypeId);
        }
    }
}