// -----------------------------------------------------------------------
// <copyright file="CustomerOutstandingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.MetaModel.Product;

    /// <summary>
    /// CustomerOutstandingMetaModel class.
    /// </summary>
    public class CustomerOutstandingMetaModel
    {
        public IList<ProductOption> ProductOptions { get; set; }

        public string Externalid { get; set; }

        public string Systemid { get; set; }

        public string Betlistname { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public bool IsConvert { get; set; }
    }
}