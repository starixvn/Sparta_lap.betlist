// --------------------------------------------------------------------------
// <copyright file="RunningBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
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
    using VendorAdapter.Interfaces;

    #endregion Using

    /// <summary>
    /// RunningBetList contains bet list:
    /// Mix Parlay              - bet type: 9
    /// Clean Sheet             - bet type: 13
    /// Double Chance           - bet type: 24
    /// B/O/N team to score     - bet type: 26
    /// To win to nil           - bet type: 27
    /// 3-Way Handicap          - bet type: 28
    /// 1H 3-Way Handicap       - bet type: 453
    /// </summary>
    public class RunningBetListController : ApiBetListBaseController
    {
        public RunningBetListController(
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
        [ApiFunctionAuthorization(PermissionKeys.TotalBetCleanSheet)]
        public dynamic CleanSheet(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.CleanSheet;
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBetToWinToNil)]
        public dynamic ToWinToNil(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.ToWinToNil;
            return ProcessRequest(metaModel);
        }

        [HttpPost]
        [ApiFunctionAuthorization(PermissionKeys.TotalBet3WayHandicap)]
        public dynamic ThreeWayHandicap(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = $"{(int)BetType.ThreeWayHandicap},{(int)BetType.FirstHalfThreeWayHandicap}";
            return ProcessRequest(metaModel, "Lrf_BetList_SelRunningByBettypeList");
        }

        private dynamic ProcessRequest(BetListMetaModel metaModel, string uspKey = "Lrf_BetList_SelRunningByBettype")
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            return GetBetListResult(metaModel, uspKey);
        }
    }
}