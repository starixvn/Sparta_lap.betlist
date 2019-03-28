namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Base;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;

    public class ApiReportBaseController : BaseApiController
    {
        protected ApiReportBaseController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
        }

        protected internal virtual bool IsValidRequest(int currencyId, int drillDownCurrencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return (currencyId <= 0 && currencyId >= SystemConstants.MinBaseCurrencyType)
                             || currencyList.ContainsKey(currencyId)
                             || KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, SessionService.LicenseeSession.LicenseeId, drillDownCurrencyId);
        }

        protected internal virtual async Task CheckValidProducts(string functionKey, BaseReportMetaModel metaModel)
        {
            var passedProductCodes = string.IsNullOrEmpty(metaModel.ProductCodeList) ?
                metaModel.ProductOptions.Select(x => x.Code) : metaModel.ProductCodeList.Split(',');
            await CheckValidProducts(functionKey, passedProductCodes).ConfigureAwait(false);
        }

        protected internal virtual async Task CheckValidProducts(string functionKey, string productCodeList)
        {
            var passedProductCodes = productCodeList?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            await CheckValidProducts(functionKey, passedProductCodes).ConfigureAwait(false);
        }

        protected internal virtual async Task CheckValidProducts(string functionKey, IEnumerable<string> productCodes)
        {
            if (productCodes == null || !productCodes.Any())
            {
                throw new ArgumentNullException(nameof(productCodes));
            }

            var productAccess = await ProductService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                functionKey).ConfigureAwait(false);
            var allowedProductCodes = productAccess.ProductList.Select(x => x.ProductCode);

            var invalidProduct = productCodes.FirstOrDefault(x => !allowedProductCodes.Contains(x));

            if (invalidProduct != null)
            {
                throw new ArgumentException($"Invalid product: {invalidProduct}");
            }
        }
    }
}