// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductService.cs" company="Nexcel">
//  The product service.
// </copyright>
// <summary>
//   The product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Common;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Service.Exception;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Caching;
    using Fanex.Logging;

    #endregion

    /// <summary>
    ///     The product service.
    /// </summary>
    public class ProductService : IProductService
    {
        #region Constants and Fields

        /// <summary>
        ///     The cache timeout minute.
        /// </summary>
        private const int CacheTimeoutMinute = 15;

        /// <summary>
        ///     The _product data access.
        /// </summary>
        private readonly IProductDataAccess _productDataAccess = new ProductDataAccess();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The get product list.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>
        /// The product list.
        /// </returns>
        public IList<Product> GetProductList(int licenseeId, string languageCode)
        {
            try
            {
                var cache = CacheService.CreateInstance(CacheType.MemoryCache);
                var cacheKey = CacheKey.MemorycacheCommonProduct + licenseeId + languageCode;
                var allowedProducts = cache.Get<IList<Product>>(cacheKey);

                if (allowedProducts != null)
                {
                    return allowedProducts;
                }

                allowedProducts = this._productDataAccess.GetProductAccessList(licenseeId);
                cache.Set(
                    cacheKey,
                    allowedProducts,
                    DateTime.Now.AddMinutes(CacheTimeoutMinute));

                return allowedProducts;
            }
            catch (ServiceException ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "ProductService");
            }
        }

        /// <summary>
        /// Get product access of licensee.
        /// </summary>
        /// <param name="licenseeId">The licensee identity.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="ignoreProducts">Ignored products.</param>
        /// <returns>The accessed product.</returns>
        public ProductAccess GetProductAccess(int licenseeId, string languageCode, IEnumerable<string> ignoreProducts = null)
        {
            var originalProducts = this.GetProductList(licenseeId, languageCode);
            var productList = ignoreProducts != null ? originalProducts.Where(i => !ignoreProducts.Contains(i.ProductCode)).ToList()
                : originalProducts;

            return new ProductAccess(productList);
        }

        /// <summary>
        /// Check allowable currency for SA tote.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <returns>True if this customer's allowed.</returns>
        public bool IsAllowedSaToteCurrency(int custId)
        {
            try
            {
                return this._productDataAccess.IsAllowedSaToteCurrency(custId);
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}