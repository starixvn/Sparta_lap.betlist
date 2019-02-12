// --------------------------------------------------------------------------
// <copyright file="RunningBetListController.cs" company="">
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
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

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
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, betListApiClient, balanceUpDataAccess)
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

        private async Task<dynamic> ProcessRequest(BetListMetaModel metaModel, string uspKey = "Lrf_BetList_SelRunningByBettype")
        {
            if (!IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            return await GetBetListResult(metaModel, uspKey).ConfigureAwait(false);
        }
    }
}