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

        public int CustomerCountAGC { get; set; }

        public int CustomerCountPTCS { get; set; }

        public int CustomerCountMGCLC { get; set; }

        public int CustomerCountMGCRNG { get; set; }

        public int CustomerCountBT { get; set; }

        public int CustomerCountVB { get; set; }
    }
}