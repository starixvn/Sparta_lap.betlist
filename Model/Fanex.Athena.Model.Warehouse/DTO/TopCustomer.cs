// -----------------------------------------------------------------------
// <copyright file="TopCustomer.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Warehouse.DTO
{
    using System;

    /// <summary>
    /// TopCustomer class.
    /// </summary>
    [Serializable]
    public class TopCustomer
    {
        public TopCustomer()
        {
            Member = string.Empty;
            Currency = string.Empty;
            MemberWinLoss = 0;
            MemberWinLossCom = 0;
            Turnover = 0;
            BetCount = 0;
            LastLoginDate = null;
            ErrorCode = null;
            Message = null;
        }

        public string Member { get; set; }

        public string Currency { get; set; }

        public decimal BAComm { get; set; }

        public decimal LicenseeWinLoss
        {
            get { return -1 * MemberWinLoss; }
        }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberWinLossCom { get; set; }

        public decimal Turnover { get; set; }

        public int BetCount { get; set; }

        public object LastLoginDate { get; set; }

        public object ErrorCode { get; set; }

        public object Message { get; set; }
    }
}