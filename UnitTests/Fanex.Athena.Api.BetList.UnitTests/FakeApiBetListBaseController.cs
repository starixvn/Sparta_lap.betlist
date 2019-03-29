namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Threading.Tasks;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.BetList.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    internal class FakeApiBetListBaseController : ApiBetListBaseController
    {
        internal FakeApiBetListBaseController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IAuthorizationService authorizationService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, authorizationService, betListApiClient, balanceUpDataAccess)
        {
        }

        internal new async Task CheckDataPermission(int custId, long transId)
        {
            await base.CheckDataPermission(custId, transId).ConfigureAwait(false);
        }

        internal new Task<dynamic> GetBetListResult(BetListMetaModel metaModel, string uspKey, string apiPath = null)
        {
            return base.GetBetListResult(metaModel, uspKey, apiPath);
        }

        internal new Task<dynamic> GetPagingBetListResult(BetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize, string apiPath = null)
        {
            return base.GetPagingBetListResult(metaModel, uspKey, pageIndex, pageSize, apiPath);
        }
    }
}