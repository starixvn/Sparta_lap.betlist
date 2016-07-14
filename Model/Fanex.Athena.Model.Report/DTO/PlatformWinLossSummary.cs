// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossSummary.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    public class PlatformWinLossSummary : ICustomerWinLoss
    {
        public PlatformWinLossSummary()
        {
        }

        public byte Level { get; set; }

        public string Parent { get; set; }

        public int PlatformType { get; set; }

        public bool IsLeaf { get; set; }

        public string Name { get; set; }

        public decimal Turnover { get; set; }

        public decimal TurnoverPercent { get; set; }

        public int TotalTransactions { get; set; }

        public decimal LicenseeWL { get; set; }
        
        public string ProductCode { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }
    }
}