// --------------------------------------------------------------------------
// <copyright file="WinlossByLeagueBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByLeague)]
    public class WinlossByLeagueBetListController : ApiBetListBaseController
    {
        public WinlossByLeagueBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, bridgeClientService, balanceUpDataAccess)
        {
        }

        [HttpPost]
        public async Task<dynamic> Index(WinLossByLeagueBetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_WinlostByLeague_Mem";
            var url = "http://localhost:55050/BetList/GetWinLossByLeagueData";
            return await GetBetListResult(metaModel, UspKey, url).ConfigureAwait(false);
        }
    }
}