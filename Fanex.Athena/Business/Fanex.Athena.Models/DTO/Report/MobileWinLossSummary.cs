// -----------------------------------------------------------------------
// <copyright file="MobileWinLossMember.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System;

    /// <summary>
    /// MobileWinLoss class.
    /// </summary>
    public class MobileWinLossSummary : ICustomerWinLoss
    {
        public MobileWinLossSummary()
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
        
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }
    }
}