// -----------------------------------------------------------------------
// <copyright file="CustomerOutstandingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.MetaModel.Product;

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