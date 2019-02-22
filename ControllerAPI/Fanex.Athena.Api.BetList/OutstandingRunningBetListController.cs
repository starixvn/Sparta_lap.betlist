namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;

    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class OutstandingRunningBetListController : ApiBetListBaseController
    {
        private const string BetListCustomerOutstandingRunningName = "BetList_CustomerOutstandingRunning";

        public OutstandingRunningBetListController(
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
            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.ProductCodeList = RemoveInvalidProducts(metaModel.ProductCodeList, BetListCustomerOutstandingRunningName);

            const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);
            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }
    }
}