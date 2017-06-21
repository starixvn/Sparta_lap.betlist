namespace Fanex.Athena.Model.Dashboard.DataDomain
{
    using System.Collections.Generic;

    public static class DashboardConfig
    {
        public static readonly IEnumerable<string> SupportedBaseCurrencies = new string[]
        {
            "EUR", "USD", "GBP", "MYR", "CNY", "THB", "IDR"
        };
    }
}