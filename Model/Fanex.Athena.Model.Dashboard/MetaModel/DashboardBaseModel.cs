namespace Fanex.Athena.Model.Dashboard.MetaModel
{
    using System;

    public class DashboardBaseModel
    {
        public int CurrencyId { get; set; }

        public string BaseCurrency { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SelectedWidgetString { get; set; }
    }
}