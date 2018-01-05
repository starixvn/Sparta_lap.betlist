namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class ExchangeWinLoss
    {
        public DateTime Date { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustId { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public int TotalTransaction { get; set; }

        public decimal CustomerWL { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerWLTotal
        {
            get
            {
                return CustomerWL + CustomerComm;
            }
        }

        public decimal LicenseeWL { get; set; }

        public decimal LicenseeNetWL { get; set; }

        public decimal AmountDueTo { get; set; }

        public decimal LicenseeComm { get; set; }

        public decimal LicenseeWLTotal
        {
            get
            {
                return LicenseeNetWL + LicenseeComm;
            }
        }
    }
}