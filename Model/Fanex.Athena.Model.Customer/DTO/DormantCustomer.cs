// -----------------------------------------------------------------------
// <copyright file="DormantCustomer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Fanex.Athena.Model.Customer.DTO
{
    public class DormantCustomer
    {
        public int Index { get; set; }

        public int CustId { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public decimal Balance { get; set; }

        public DateTime ArchivedDate { get; set; }
    }
}