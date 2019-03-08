using System.Collections.Generic;

namespace Fanex.Athena.Api.BetList.Helpers
{
    public static class BetListHelper
    {
        public static bool IsValidCurrencyId(int[] currencyList, IDictionary<int, string> userCurrencyList)
        {
            if (currencyList == null)
            {
                return false;
            }

            foreach (var currency in currencyList)
            {
                if (!userCurrencyList.ContainsKey(currency))
                {
                    return false;
                }
            }

            return true;
        }
    }
}