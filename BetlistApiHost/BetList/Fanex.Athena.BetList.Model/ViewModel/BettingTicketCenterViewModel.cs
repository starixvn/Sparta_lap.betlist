namespace Fanex.Athena.BetList.Model.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Athena.Model.Core.MetaModel.Product;

    [Serializable]
    public class BettingTicketCenterViewModel
    {
        public BettingTicketCenterViewModel()
        {
            var today = DateTime.Today;
            FromDate = today;
            ToDate = today;
            MaxDate = today;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MaxDate { get; set; }

        public DateTime MinDate { get; set; }

        public bool IsAllowedCustomerComm { get; set; }

        public IEnumerable<ProductOption> ProductOptionList { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }

        public IDictionary<int, string> StatusList { get; set; }

        public IDictionary<int, string> PlatformList { get; set; }

        public IDictionary<int, string> LiveStatusList { get; set; }
    }
}