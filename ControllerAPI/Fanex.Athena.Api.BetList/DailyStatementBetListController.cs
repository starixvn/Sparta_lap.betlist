// --------------------------------------------------------------------------
// <copyright file="DailyStatementBetListController.cs" company="">
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
    public class DailyStatementBetListController : ApiBetListBaseController
    {
        public DailyStatementBetListController(
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
        public async Task<dynamic> Index(BetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_SelByDayAll_Mem";
            const string UspKeyOld = "Lrf_BetList_SelByDayAll_Mem_Old";

            var balanceUp = GetBalanceUp();
            var uspKey = metaModel.FromDate < balanceUp.BalanceUpDay ? UspKeyOld : UspKey;

            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);

            const int RunningTicketTransType = 3;
            var result = await GetBetListResult(metaModel, uspKey).ConfigureAwait(false);
            
            return new
            {
                result.Main,
                result.Total,
                result.LeagueName,
                IsShowTotal = metaModel.TransType != RunningTicketTransType
            };
        }
    }
}