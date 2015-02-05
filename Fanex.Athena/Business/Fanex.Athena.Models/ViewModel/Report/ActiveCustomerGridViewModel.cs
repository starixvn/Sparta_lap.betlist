// -----------------------------------------------------------------------
// <copyright file="ActiveCustomerGridViewModel.cs" company="Nexcel">
// ActiveCustomerGridViewModel copyright.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System;

    /// <summary>
    /// ActiveCustomerGridViewModel class.
    /// </summary>
    public class ActiveCustomerGridViewModel
    {
        public DateTime TransactionDate { get; set; }

        public int TotalCustomer { get; set; }

        public int CustomerCountSB { get; set; }

        public int CustomerCountRB { get; set; }

        public int CustomerCountNG { get; set; }

        public int CustomerCountBA { get; set; }

        public int CustomerCountLC { get; set; }

        public int CustomerCountVS { get; set; }

        public int CustomerCountBG { get; set; }

        public int CustomerCountCS { get; set; }

        public int CustomerCountKN { get; set; }
    }
}