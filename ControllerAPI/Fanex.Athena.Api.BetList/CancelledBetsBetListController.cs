// --------------------------------------------------------------------------
// <copyright file="CancelledBetsBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Service.Core.Utility.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCancelledBets)]
    public class CancelledBetsBetListController : ApiBetListBaseController
    {
        public CancelledBetsBetListController(
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

            const string UspKey = "Lrf_BetList_SelDeleted";
            metaModel.BetType = 13;

            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }
    }
}