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
    using Extension.Utility.Interfaces;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Caching;

    #endregion

    public class ProductService : IProductService
    {
        private const int CacheTimeoutMinute = 15;

        private readonly IProductDataAccess _productDataAccess;

        private readonly IAppSettings _appSettings;

        private readonly IXDocumentWrapper _xmlWrapper;

        private readonly IMemoryCacheService _cache;

        public ProductService()
        {
            _appSettings = new AppSettings();
            _productDataAccess = new ProductDataAccess();
            _xmlWrapper = new XDocumentWrapper();
            _cache = new MemoryCacheService();
        }

        public ProductService(IAppSettings appSettings, IProductDataAccess productDataAccess, IXDocumentWrapper xmlWrapper)
        {
            _appSettings = appSettings;
            _productDataAccess = productDataAccess;
            _xmlWrapper = xmlWrapper;
        }

        #region Public Methods and Operators

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

            var productList = allowedProducts.Any() ? originalProducts.Where(i => allowedProducts.Contains(i.ProductCode)).ToList()
                : originalProducts;
            return new ProductAccess(productList);
        }

        public ProductAccess GetSingleWalletProductAccess(int licenseeId, string languageCode, string functionConfigKey = null)
        {
            var allowedProducts = GetAllowedProducts(functionConfigKey);

            var originalProducts = this.GetProductList(licenseeId, languageCode);

            var productList = allowedProducts.Any() ? originalProducts.Where(i => i.IsOfferedSingleWallet && allowedProducts.Contains(i.ProductCode)).ToList()
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
                products = _productDataAccess.GetProductAccessList(licenseeId, true);
                var allowedProducts = GetAllowedProducts(FilterFunctionName);
                if (allowedProducts.Any())
                {
                    products = products.Where(i => i.IsOfferedSingleWallet && allowedProducts.Contains(i.ProductCode)).ToList();
                }

                cache.Set(
                    cacheKey,
                    products,
                    DateTime.Now.AddMinutes(CacheTimeoutMinute));
            }

            return new ProductAccess(products);
        }

        #endregion

        private string[] GetAllowedProducts(string functionName = null)
        {
            const string FunctionAllowedProductsKey = "FunctionAllowedProducts";

            if (string.IsNullOrWhiteSpace(functionName))
            {
                functionName = "_Default";
            }

            var functions = _cache.Get<IEnumerable<XElement>>(FunctionAllowedProductsKey);
            if (functions == null)
            {
                var filePath = HostingEnvironment.MapPath(_appSettings.GetValue("FunctionAllowedProductFile"));
                functions = _xmlWrapper.Load(filePath).Descendants(nameof(Function));
                if (functions != null && functions.Any())
                {
                    _cache.Set(FunctionAllowedProductsKey, functions);
                }
            }

            var config = functions.FirstOrDefault(x => x.Attribute("Name").Value.Equals(functionName, StringComparison.OrdinalIgnoreCase));
            return config == null ? new string[] { } : config.Attribute(nameof(Product)).Value.Split(',')
                    .Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }
}