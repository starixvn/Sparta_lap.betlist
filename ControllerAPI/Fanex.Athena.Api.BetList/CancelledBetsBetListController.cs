// --------------------------------------------------------------------------
// <copyright file="CancelledBetsBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;

    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCancelledBets)]
    public class CancelledBetsBetListController : ApiBetListBaseController
    {
        private const int BetType = 13;

        public CancelledBetsBetListController(
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
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_SelDeleted";
            metaModel.BetType = BetType;

            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }
    }
}