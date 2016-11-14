namespace Fanex.Athena.Api.Report
{
    using Base;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.MetaModel;
    using Service.Core.Utility;

    public class ApiReportBaseController : BaseApiController
    {
        protected bool IsValidRequest(BaseMetaModel metaModel, int currencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= SystemConstants.MinBaseCurrencyType)
                             || KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, SessionService.LicenseeSession.LicenseeId, currencyId);

        }
    }
}
