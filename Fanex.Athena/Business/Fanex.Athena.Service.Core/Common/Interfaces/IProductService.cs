// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductService.cs" company="Nexcel">
// The i product service.
// </copyright>
// <summary>
//   The i product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    ///     The i product service.
    /// </summary>
    public interface IProductService
    {
        #region Public Methods and Operators

        ProductAccess GetProductAccess(int licenseeId, string languageCode, string functionConfigKey = null);

        /// <summary>
        ///     The get product list.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>
        /// The products.
        /// </returns>
        IList<Product> GetProductList(int licenseeId, string languageCode);

        ProductAccess GetProductAccessForActiveCustomerSummary(int licenseeId, string languageCode);

        /// <summary>
        ///     Check if the member's currency is allowed in SA Tote.
        /// </summary>
        /// <param name="custId">Id of member.</param>
        /// <returns>Return true if member's currency is allowed in SA tote.</returns>
        bool IsAllowedSaToteCurrency(int custId);

        #endregion Public Methods and Operators
    }
}