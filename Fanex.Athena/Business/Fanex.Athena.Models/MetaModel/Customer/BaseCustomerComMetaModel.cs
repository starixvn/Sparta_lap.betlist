// -----------------------------------------------------------------------
// <copyright file="BaseCustomerComMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// BaseCustomerComMetaModel class.
    /// </summary>
    public abstract class BaseCustomerComMetaModel : BaseMetaModel
    {
        private string _customerIds;

        public string Ip { get; set; }

        public string Language { get; set; }

        public IList<int> Customers { get; set; }

        public string CustomerIds
        {
            get
            {
                return _customerIds;
            }

            set
            {
                _customerIds = value;
                Customers = _customerIds.Split(',').Select(int.Parse).ToArray();
            }
        }

        public string CustomerToSQLParam
        {
            get
            {
                return string.Join(",", Customers);
            }
        }
    }
}