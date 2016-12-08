// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Service.Core.Utility;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportPlatformWinLoss)]
    public class PlatformWinLossBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            if (!KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, licenseeId, metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.LicenseeId = licenseeId;
            metaModel.UserId = userSession.UserId;

            const string UspKey = "Lrf_BetList_PlatformWinLoss_Mem";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}