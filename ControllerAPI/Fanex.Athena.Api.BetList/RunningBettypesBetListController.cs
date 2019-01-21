// --------------------------------------------------------------------------
// <copyright file="RunningBettypesBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Threading.Tasks;
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    /// <summary>
    /// RunningBetList contains bet list:
    /// Mix Parlay              - bet type: 9
    /// Clean Sheet             - bet type: 13
    /// Double Chance           - bet type: 24
    /// B/O/N team to score     - bet type: 26
    /// To win to nil           - bet type: 27
    /// 3-Way Handicap          - bet type: 28.
    /// </summary>
    public class RunningBettypesBetListController : ApiBetListBaseController
    {
        private const string DoubleChangeBettypes = "24,151,186,431,410";

        private const string BONTeamToScoreBettypes = "26,145,146,433,170,188,427";
        
        public RunningBettypesBetListController(
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

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetMixParlay)]
        public dynamic MixParlay(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = ((int)BetType.CombinationMixParlay).ToString();
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetDoubleChance)]
        public dynamic DoubleChange(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = DoubleChangeBettypes;
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetBONTeamToScore)]
        public dynamic BONTeamToScore(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = BONTeamToScoreBettypes;
            return ProcessRequest(metaModel);
        }

        private async Task<dynamic> ProcessRequest(BetListMetaModel metaModel)
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_SelRunningByBettypeList";
            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }
    }
}