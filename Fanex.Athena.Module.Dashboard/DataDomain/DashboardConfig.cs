namespace Fanex.Athena.Module.Dashboard.DataDomain
{
    using System.Collections.Generic;

    public static class DashboardConfig
    {
        public static readonly IEnumerable<string> SupportedBaseCurrencies = new[]
        {
            "EUR", "USD", "GBP", "MYR", "CNY", "THB", "IDR"
        };
    }
}