// --------------------------------------------------------------------------
// <copyright file="MixParlayBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.MetaModel;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    public class MixParlayBetListController : ApiBetListBaseController
    {
        public MixParlayBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter,
            IBetListHTMLBuilder betlistHtmlBuilder)
        : base(sessionService, productService, languageService, bridgeClientService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter, betlistHtmlBuilder)
        {
        }

        [HttpGet]
        public async Task<dynamic> Detail(long transId, int custId)
        {
            var metaModel = new MixParlayDetailMetaModel
            {
                CustId = custId,
                TransId = transId,
                UserCurrencyList = UserCurrencyList
            };
            var betlistAPIUrl = new Uri($"http://localhost:55050/MixParlayBetList/Detail");
            return await BridgeClientService.PostAsJsonAsync<MixParlayDetailMetaModel, dynamic>(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<dynamic> NewParlayDetail(long transId, string betTeam, string betCheck, decimal handicap1, decimal handicap2, int custId)
        {
            var metaModel = new NewParlayDetailMetaModel
            {
                TransId = transId,
                BetTeam = betTeam,
                BetCheck = betCheck,
                Handicap1 = handicap1,
                Handicap2 = handicap2,
                CustId = custId,
                UserCurrencyList = UserCurrencyList
            };
            var betlistAPIUrl = new Uri($"http://localhost:55050/MixParlayBetList/NewParlayDetail");
            return await BridgeClientService.PostAsJsonAsync<NewParlayDetailMetaModel, dynamic>(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }
    }
}