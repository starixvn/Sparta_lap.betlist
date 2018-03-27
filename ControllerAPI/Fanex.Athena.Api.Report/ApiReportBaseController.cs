namespace Fanex.Athena.Api.Report
{
    using System;
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

        protected ApiReportBaseController()
        {
        }

        protected bool IsValidRequest(BaseMetaModel metaModel, int currencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= SystemConstants.MinBaseCurrencyType)
                             || currencyList.ContainsKey(metaModel.CurrencyId)
                             || KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, SessionService.LicenseeSession.LicenseeId, currencyId);
        }

        protected void CheckValidProducts(string functionKey, BaseReportMetaModel metaModel)
        {
            var allowedProductCodes = ProductService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                functionKey).ProductList.Select(x => x.ProductCode);

            var passedProductCodes = string.IsNullOrEmpty(metaModel.ProductCodeList) ?
                metaModel.ProductOptions.Select(x => x.Code) : metaModel.ProductCodeList.Split(',');

            var invalidProduct = passedProductCodes.FirstOrDefault(x => !allowedProductCodes.Contains(x));
            if (invalidProduct != null)
            {
                throw new ArgumentException($"Invalid product: {invalidProduct}");
            }
        }
    }
}