// --------------------------------------------------------------------------
// <copyright file="RunningBettypesOfMatchBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Web.Http;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class RunningBettypesOfMatchBetListController : ApiBetListBaseController
    {
        public RunningBettypesOfMatchBetListController()
        {
        }

        public RunningBettypesOfMatchBetListController(
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
        public dynamic Index(BetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            return GetBetListResult(metaModel, "Lrf_BetList_SelRunningByMultipleBettypeMatch");
        }
    }
}