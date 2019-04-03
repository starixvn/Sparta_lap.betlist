// -----------------------------------------------------------------------
// <copyright file="IProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IProductDataAccess
    {
        IList<Product> GetProductAccessList(int licenseeId, bool isActiveCustomerSummaryReport = false);
    }
}