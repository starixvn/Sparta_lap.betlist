// --------------------------------------------------------------------------
// <copyright file="RunningMatchChoiceBetListController.cs" company="">
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

    public class RunningMatchChoiceBetListController : ApiBetListBaseController
    {
        public RunningMatchChoiceBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, betListApiClient, balanceUpDataAccess)
        {
        }

        [HttpPost]
        public async Task<dynamic> Index(BetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            return await GetBetListResult(metaModel, "Lrf_BetList_Forecast_SelByMatch").ConfigureAwait(false);
        }
    }
}