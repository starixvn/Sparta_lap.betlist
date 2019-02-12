// --------------------------------------------------------------------------
// <copyright file="MicroGamingRngBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;

    #region Using

    using System.Web.Http;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    #endregion Using

    public class MicroGamingRngBetListController : ApiBetListBaseController
    {
        public MicroGamingRngBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, betListApiClient, balanceUpDataAccess)
        {
        }

        [HttpPost]
        public async Task<dynamic> Detail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";
            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }
    }
}