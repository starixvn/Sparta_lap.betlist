// -----------------------------------------------------------------------
// <copyright file="ActiveCustomer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ActiveCustomer class.
    /// </summary>
    [Serializable]
    public class ActiveCustomer
    {
        public DateTime TransactionDate { get; set; }

        public int TotalCustomer { get; set; }

        public IDictionary<string, int> CustomerCount { get; set; }
    }
}