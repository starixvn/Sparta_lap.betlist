// -----------------------------------------------------------------------
// <copyright file="BaseCustomerComMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.MetaModel;

    /// <summary>
    /// BaseCustomerComMetaModel class.
    /// </summary>
    public abstract class BaseCustomerComMetaModel : BaseMetaModel
    {
        public string Ip { get; set; }

        public string Language { get; set; }

        public IList<int> Customers
        {
            get
            {
                return string.IsNullOrWhiteSpace(CustomerIds) ? new List<int>() : CustomerIds.Split(',').Select(int.Parse).ToList();
            }
        }

        public string CustomerIds { get; set; }

        public string CustomerToSQLParam
        {
            get
            {
                return string.Join(",", Customers);
            }
        }
    }
}