// -----------------------------------------------------------------------
// <copyright file="Balance.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
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