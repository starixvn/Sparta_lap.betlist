// -----------------------------------------------------------------------
// <copyright file="IProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    /// <summary>
    /// The i product data access.
    /// </summary>
    public interface IProductDataAccess
    {
        #region Public Methods and Operators

        IList<Product> GetProductAccessList(int licenseeId, bool isActiveCustomerSummaryReport = false);

        bool IsAllowedSaToteCurrency(int custId);

        #endregion Public Methods and Operators
    }
}