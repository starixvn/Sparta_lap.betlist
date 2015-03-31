// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductService.cs" company="Nexcel">
// The i product service.
// </copyright>
// <summary>
//   The i product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    ///     The i product service.
    /// </summary>
    public interface IProductService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Get page access for the licensee.
        /// </summary>
        /// <param name="licenseeId">Licensee Id.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="allowedProducts">Allowed products.</param>
        /// <returns>
        ///     Product-Access object.
        /// </returns>
        ProductAccess GetProductAccess(int licenseeId, string languageCode, IEnumerable<string> allowedProducts = null);

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

        /// <summary>
        ///     Check if the member's currency is allowed in SA Tote.
        /// </summary>
        /// <param name="custId">Id of member.</param>
        /// <returns>Return true if member's currency is allowed in SA tote.</returns>
        bool IsAllowedSaToteCurrency(int custId);

        #endregion Public Methods and Operators
    }
}