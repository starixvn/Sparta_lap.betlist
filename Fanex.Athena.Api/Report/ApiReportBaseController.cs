namespace Fanex.Athena.Api.Report
{
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Models.MetaModel;

    public class ApiReportBaseController : BaseApiController
    {
        protected bool IsValidRequest(BaseMetaModel metaModel, int currencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            var validModel = (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= SystemConstants.MinBaseCurrencyType)
                             || currencyList.Values.Contains(metaModel.CurrencyId)
                             || (SessionService.LicenseeSession.LicenseeId == Licensees.KeenOcean && currencyId == CurrencyType.Cny);
            var validCurrency = IsValidCurrencyRequest(currencyId, currencyList);
            return validModel && validCurrency;
        }

        private bool IsValidCurrencyRequest(int currencyId, System.Collections.Generic.IDictionary<string, int> currencyList)
        {
            var validCurrency = (currencyId <= 0 && currencyId >= SystemConstants.MinBaseCurrencyType) ||
                                currencyList.Values.Contains(currencyId);

            return validCurrency || (SessionService.LicenseeSession.LicenseeId == Licensees.KeenOcean && currencyId == CurrencyType.Cny);
        }
    }
}
