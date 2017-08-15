namespace Fanex.Athena.Model.Dashboard.MetaModel
{
    using System;
    using System.Linq;
    using DataDomain;

    public class DashboardBaseModel
    {
        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public string BaseCurrency { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SelectedWidgetString { get; set; }

        public string CurrencyIDList { get; set; }

        public virtual bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(BaseCurrency) || DashboardConfig.SupportedBaseCurrencies.Contains(BaseCurrency.ToUpperInvariant());
            }
        }
    }
}