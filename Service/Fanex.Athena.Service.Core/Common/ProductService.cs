// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductService.cs" company="">
//  The product service.
// </copyright>
// <summary>
//   The product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Hosting;
    using System.Xml.Linq;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Caching;

    #endregion

    /// <summary>
    ///     The product service.
    /// </summary>
    public class ProductService : IProductService
    {
        private const int CacheTimeoutMinute = 15;

        private readonly IProductDataAccess _productDataAccess;

        private readonly IAppSettings _appSettings;

        private readonly IServerPathMapper _serverPathMapper;

        public ProductService()
        {
            _appSettings = new AppSettings();
            _productDataAccess = new ProductDataAccess();
            _serverPathMapper = new ServerPathMapper();
        }

        public ProductService(IAppSettings appSettings, IProductDataAccess productDataAccess, IServerPathMapper serverPathMapper)
        {
            _appSettings = appSettings;
            _productDataAccess = productDataAccess;
            _serverPathMapper = serverPathMapper;
        }

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
            var cache = Fanex.Caching.CacheService.CreateInstance(CacheType.MemoryCache);
            var cacheKey = CacheKey.MemorycacheCommonProduct + licenseeId + languageCode;
            var allowedProducts = cache.Get<IList<Product>>(cacheKey);
            
            if (allowedProducts != null)
            {
                return allowedProducts;
            }

            allowedProducts = this._productDataAccess.GetProductAccessList(licenseeId);
            if (allowedProducts != null && allowedProducts.Count > 0)
            {
                cache.Set(
                    cacheKey,
                    allowedProducts,
                    DateTime.Now.AddMinutes(CacheTimeoutMinute));
            }

            return allowedProducts;
        }

        public ProductAccess GetProductAccess(int licenseeId, string languageCode, string functionConfigKey = null)
        {
            var allowedProducts = GetAllowedProducts(functionConfigKey);

            var originalProducts = this.GetProductList(licenseeId, languageCode);

            var productList = allowedProducts != null ? originalProducts.Where(i => allowedProducts.Contains(i.ProductCode)).ToList()
                : originalProducts;
            return new ProductAccess(productList);
        }

        public ProductAccess GetProductAccessForActiveCustomerSummary(int licenseeId, string languageCode)
        {
            const string FilterFunctionName = "ActiveCustomerSummary";

            var cache = Fanex.Caching.CacheService.CreateInstance(CacheType.MemoryCache);
            var cacheKey = CacheKey.MemorycacheActiveCustomerSummaryProduct + licenseeId + languageCode;
            var products = cache.Get<IList<Product>>(cacheKey);

            if (products == null)
            {
                products = this._productDataAccess.GetProductAccessList(licenseeId, true);
                var allowedProducts = GetAllowedProducts(FilterFunctionName);
                if (allowedProducts.Any())
                {
                    products = products.Where(i => allowedProducts.Contains(i.ProductCode)).ToList();
                }

                cache.Set(
                    cacheKey,
                    products,
                    DateTime.Now.AddMinutes(CacheTimeoutMinute));
            }

            return new ProductAccess(products);
        }

        /// <summary>
        /// Check allowable currency for SA tote.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <returns>True if this customer's allowed.</returns>
        public bool IsAllowedSaToteCurrency(int custId)
        {
            return this._productDataAccess.IsAllowedSaToteCurrency(custId);
        }
        
        #endregion

        /// <summary>
        /// Get Allowed Products.
        /// </summary>
        /// <param name="functionName">Function name.</param>
        /// <returns>Product code array.</returns>
        private string[] GetAllowedProducts(string functionName = null)
        {
            const string FunctionAllowedProductsKey = "FunctionAllowedProducts";

            if (string.IsNullOrWhiteSpace(functionName))
            {
                functionName = "_Default";
            }

            var functions = CacheService.Get<IEnumerable<XElement>>(FunctionAllowedProductsKey);
            if (functions == null)
            {
                var filePath = _serverPathMapper.MapPath(_appSettings.GetValue("FunctionAllowedProductFile"));
                functions = XDocument.Load(filePath).Descendants("Function");
                if (functions != null && functions.Count() > 0)
                {
                    CacheService.Set(FunctionAllowedProductsKey, functions);
                }
            }

            var config = functions
                    .Where(x => x.Attribute("Name").Value.Equals(functionName, System.StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            if (config == null)
            {
                return new string[] { };
            }
            else
            {
                return config.Attribute("Product").Value.Split(',')
                    .Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }
    }
}