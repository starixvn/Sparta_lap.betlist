// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;

    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.BetList.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportPlatformWinLoss)]
    public class PlatformWinLossBetListController : ApiBetListBaseController
    {
        public PlatformWinLossBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IAuthorizationService authorizationService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, authorizationService, betListApiClient, balanceUpDataAccess)
        {
        }

        [HttpPost]
        public async Task<dynamic> Index(BetListMetaModel metaModel)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            if (!KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(SessionService.UserSession.CurrencyList, licenseeId, metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_PlatformWinLoss_Mem";
            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }
    }
}