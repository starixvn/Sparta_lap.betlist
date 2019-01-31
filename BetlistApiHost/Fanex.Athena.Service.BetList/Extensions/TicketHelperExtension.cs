using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.Service.BetList.Extensions
{
    public static class TicketHelperExtension
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public static string GetValidOddsTypeNameById(this ITicketHelper ticketHelper, object oddsTypeId)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            if (oddsTypeId == null || oddsTypeId.ToString().Trim() == string.Empty || oddsTypeId.ToString() == "0")
            {
                return string.Empty;
            }

            return ticketHelper.GetOddsTypeNameById(oddsTypeId);
        }
    }
}