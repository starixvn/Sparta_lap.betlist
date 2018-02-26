namespace Fanex.Athena.Api.BetList.UnitTests
{
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.MetaModel;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class FakeApiBetListBaseController : ApiBetListBaseController
    {
        public FakeApiBetListBaseController()
        {
        }

        public FakeApiBetListBaseController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter)
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