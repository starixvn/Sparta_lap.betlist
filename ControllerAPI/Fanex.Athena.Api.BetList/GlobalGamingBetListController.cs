namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.BetList.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    public class GlobalGamingBetListController : ApiBetListBaseController
    {
        public GlobalGamingBetListController(
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
        public Task<dynamic> Detail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}