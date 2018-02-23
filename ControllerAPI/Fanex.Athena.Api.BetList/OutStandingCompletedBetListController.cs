// --------------------------------------------------------------------------
// <copyright file="OutStandingCompletedBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
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

        [CLSCompliant(false)]
        public OutStandingCompletedBetListController(
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
            if (!metaModel.IsValid ||
                (metaModel.CurrencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId)))
            {
                return null;
            }

            metaModel.ProductCodeList = RemoveInvalidProducts(metaModel.ProductCodeList, BetListCustomerOutstandingName);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            const string UspKey = "Lrf_BetList_SelAllByDate_Mem";
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);    // Need -2 miliseconds, it better than -1.
            var result = GetBetListResult(metaModel, UspKey);
            return new
            {
                result.Main,
                result.LeagueName,
                result.Total,
                IsShowTotal = true
            };
        }
    }
}