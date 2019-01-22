namespace Fanex.Athena.Api.BetList.UnitTests
{
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.MetaModel;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    internal class FakeApiBetListBaseController : ApiBetListBaseController
    {
        internal FakeApiBetListBaseController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, bridgeClientService, balanceUpDataAccess)
        {
        }

        internal new dynamic GetBetListResult(BetListMetaModel metaModel, string uspKey)
        {
            return base.GetBetListResult(metaModel, uspKey);
        }

        internal new dynamic GetPagingBetListResult(BetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
        {
            return base.GetPagingBetListResult(metaModel, uspKey, pageIndex, pageSize);
        }
    }
}