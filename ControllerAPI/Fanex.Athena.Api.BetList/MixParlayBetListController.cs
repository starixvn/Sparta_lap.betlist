// --------------------------------------------------------------------------
// <copyright file="MixParlayBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Athena.BetList.Model.MetaModel;
    using DataAccess.Core.Configuration.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    public class MixParlayBetListController : ApiBetListBaseController
    {
        public MixParlayBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, betListApiClient, balanceUpDataAccess)
        {
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Detail(long transId, int custId)
        {
            var metaModel = new MixParlayDetailMetaModel
            {
                CustId = custId,
                TransId = transId,
                UserCurrencyList = UserCurrencyList
            };
            return await BridgeClientService.PostAsync("/MixParlayBetList/Detail", metaModel).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> NewParlayDetail(long transId, string betTeam, string betCheck, decimal handicap1, decimal handicap2, int custId)
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
            return await BridgeClientService.PostAsync("/MixParlayBetList/NewParlayDetail", metaModel).ConfigureAwait(false);
        }
    }
}