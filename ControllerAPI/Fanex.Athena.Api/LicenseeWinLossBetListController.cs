namespace Fanex.Athena.Api.BetList
{
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Service.Core.Utility;

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss, PermissionKeys.ReportWinLossByProduct, PermissionKeys.ReportColossusBetsWinLoss)]
    public class LicenseeWinLossBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var currencyList = userSession.CurrencyList;

            if (!KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, licenseeId, metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.LicenseeId = licenseeId;
            metaModel.UserId = userSession.UserId;

            const string UspKey = "Lrf_BetList_SelWinloss_Mem";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}