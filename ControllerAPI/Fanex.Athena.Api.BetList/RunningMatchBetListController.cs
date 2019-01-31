// --------------------------------------------------------------------------
// <copyright file="RunningMatchBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    public class RunningMatchBetListController : ApiBetListBaseController
    {
        public RunningMatchBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, bridgeClientService, balanceUpDataAccess)
        {
        }

        [HttpPost]
        public async Task<dynamic> Index(BetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_SelRunningByBettypeMatch";
            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }
    }
}