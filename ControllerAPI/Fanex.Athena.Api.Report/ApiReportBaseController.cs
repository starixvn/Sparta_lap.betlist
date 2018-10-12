namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.MetaModel;
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

        protected internal virtual bool IsValidRequest(BaseMetaModel metaModel, int currencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= SystemConstants.MinBaseCurrencyType)
                             || currencyList.ContainsKey(metaModel.CurrencyId)
                             || KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, SessionService.LicenseeSession.LicenseeId, currencyId);
        }

        protected internal virtual void CheckValidProducts(string functionKey, BaseReportMetaModel metaModel)
        {
            var passedProductCodes = string.IsNullOrEmpty(metaModel.ProductCodeList) ?
                metaModel.ProductOptions.Select(x => x.Code) : metaModel.ProductCodeList.Split(',');
            CheckValidProducts(functionKey, passedProductCodes);
        }

        protected internal virtual void CheckValidProducts(string functionKey, string productCodeList)
        {
            var passedProductCodes = productCodeList?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            CheckValidProducts(functionKey, passedProductCodes);
        }

        protected internal virtual void CheckValidProducts(string functionKey, IEnumerable<string> productCodes)
        {
            if (productCodes == null || !productCodes.Any())
            {
                throw new ArgumentNullException(nameof(productCodes));
            }

            var allowedProductCodes = ProductService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                functionKey).ProductList.Select(x => x.ProductCode);

            var invalidProduct = productCodes.FirstOrDefault(x => !allowedProductCodes.Contains(x));

            if (invalidProduct != null)
            {
                throw new ArgumentException($"Invalid product: {invalidProduct}");
            }
        }
    }
}