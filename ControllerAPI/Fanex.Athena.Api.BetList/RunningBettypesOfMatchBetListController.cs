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

        [CLSCompliant(false)]
        public RunningBettypesOfMatchBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter)
        {
        }

        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            return GetBetListResult(metaModel, "Lrf_BetList_SelRunningByMultipleBettypeMatch");
        }
    }
}