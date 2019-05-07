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

    public class BaseCustomerComMetaModel : BaseMetaModel
    {
        public string Ip { get; set; }

        public string Language { get; set; }

        public IList<int> Customers
        {
            get
            {
                var customers = string.IsNullOrWhiteSpace(CustomerIds) ? new List<int>() : CustomerIds.Split(',').Select(int.Parse).ToList();
                return customers;
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

        public string UserName { get; set; }
    }
}