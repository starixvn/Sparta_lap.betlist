namespace Fanex.Athena.Api.BetList
{
    using System.Web.Http;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class PragmaticPlayBetListController : ApiBetListBaseController
    {
        public PragmaticPlayBetListController()
        {
        }

        public PragmaticPlayBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter,
            IBetListHTMLBuilder betlistHtmlBuilder)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter, betlistHtmlBuilder)
        {
        }

        [HttpPost]
        public dynamic Detail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}