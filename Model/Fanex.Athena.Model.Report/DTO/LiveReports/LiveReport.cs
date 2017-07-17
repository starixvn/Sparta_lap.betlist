// -----------------------------------------------------------------------
// <copyright file="LiveReport.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO.LiveReports
{
    public class LiveReport : ICustomerWinLoss
    {
        public string ProductName { get; set; }

        public decimal TurnOver { get; set; }

        public int BetCount { get; set; }

        public decimal LicenseeWinLoss { get; set; }

        public decimal LicenseeComm { get; set; }

        public decimal AmountDue { get; set; }

        public string ProductCode { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }
    }
}