// -----------------------------------------------------------------------
// <copyright file="IProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IProductDataAccess
    {
        Task<IList<Product>> GetProductAccessList(int licenseeId, bool isActiveCustomerSummaryReport = false);
    }
}