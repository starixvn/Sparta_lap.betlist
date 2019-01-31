// --------------------------------------------------------------------------
// <copyright file="OutStandingCompletedBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;

    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class OutStandingCompletedBetListController : ApiBetListBaseController
    {
        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";

        public OutStandingCompletedBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, bridgeClientService, balanceUpDataAccess)
        {
        }

        [HttpGet]
        public async Task<dynamic> Index(
            int page,
            int rows,
            [FromUri]BetListMetaModel metaModel)
        {
            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.ProductCodeList = RemoveInvalidProducts(metaModel.ProductCodeList, BetListCustomerOutstandingName);
            return await GetPagingBetListResult(metaModel, "Lrf_BetList_CustomerList_Mem", page, rows).ConfigureAwait(false);
        }
    }
}