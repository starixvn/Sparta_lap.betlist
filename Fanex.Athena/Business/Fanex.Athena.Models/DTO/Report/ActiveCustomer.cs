// -----------------------------------------------------------------------
// <copyright file="ActiveCustomer.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ActiveCustomer class.
    /// </summary>
    [Serializable]
    public class ActiveCustomer
    {
        public string TransactionDate { get; set; }

        public int TotalCustomer { get; set; }

        public IDictionary<string, int> CustomerCountPerProduct { get; set; }
    }
}