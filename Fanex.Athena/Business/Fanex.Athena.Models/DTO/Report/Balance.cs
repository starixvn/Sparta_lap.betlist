// -----------------------------------------------------------------------
// <copyright file="Balance.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System.Collections.Generic;

    /// <summary>
    /// Balance class.
    /// </summary>
    public class Balance
    {
        public List<BalanceDetails> BalanceList { get; set; }

        #region Customer Information

        public decimal CustomerPendingFunds { get; set; }

        public int TotalCustomerActive { get; set; }

        public int TotalCustomerClosed { get; set; }

        public int TotalCustomerSuspended { get; set; }

        public string UserName { get; set; }

        #endregion Customer Information
    }
}