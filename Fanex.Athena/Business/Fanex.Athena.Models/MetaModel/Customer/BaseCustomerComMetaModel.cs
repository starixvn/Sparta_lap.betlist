// -----------------------------------------------------------------------
// <copyright file="BaseCustomerComMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer
{
    using System.Collections.Generic;

    /// <summary>
    /// BaseCustomerComMetaModel class.
    /// </summary>
    public abstract class BaseCustomerComMetaModel : BaseMetaModel
    {
        public string Ip { get; set; }

        public string Language { get; set; }

        public IList<int> Customers { get; set; }

        public string CustomerToSQLParam
        {
            get
            {
                return string.Join(",", Customers);
            }
        }
    }
}