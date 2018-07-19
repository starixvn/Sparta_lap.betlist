// --------------------------------------------------------------------------
// <copyright file="OutStandingCompletedBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class OutStandingCompletedBetListController : ApiBetListBaseController
    {
        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";

        public OutStandingCompletedBetListController()
        {
        }

        public OutStandingCompletedBetListController(
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

        [HttpGet]
        public dynamic Index(
            int page,
            int rows,
            [FromUri]BetListMetaModel metaModel)
        {
            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.ProductCodeList = RemoveInvalidProducts(metaModel.ProductCodeList, BetListCustomerOutstandingName);
            return GetPagingBetListResult(metaModel, "Lrf_BetList_CustomerList_Mem", page, rows);
        }
    }
}